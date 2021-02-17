# JNIGenerator
Generate Kotlin JNI bindings for C headers. Features:
 * Generate Kotlin data classes for C structs.
 * Generate Kotlin bindings for C functions.
 * Supports multiple C header files.

## Usage
```sh
git clone https://github.com/codesharp-hu/JNIGenerator.git
cd JNIGenerator
mkdir generated
cd JNIGenerator
dotnet run -- template/config.json
```
Edit `template/config.json` to
 * set path to C headers
 * set outputh pathes
 * set Kotlin package name(s)
 * set API calss name
 
To customize ouput, you can edit the [Scriban](https://github.com/scriban/scriban) templates located in the `templates` folder.

## Examle Input

```C
#ifndef MESSAGE_H
#define MESSAGE_H

struct Message {
    int isUrgent;
    char* subject;
    char* text;
};

void sendMessage(struct Message message);

#endif
```


## Examle Output

### Message.kt
```Kotlin
//Generated code. Do not edit!

package com.jnigen.model

data class Message (var isUrgent: Int = 0, var subject: String = String(), var text: String = String())
```


### JniApi.kt
```Kotlin
//Generated code. Do not edit!

package com.jnigen

import com.jnigen.model.*

class JniApi {

    external fun sendMessage(message: Message)
    companion object {
        init {
            System.loadLibrary("native-lib")
        }
    }
}
```


### jni-wrapper.c
You can find the generated example JNI code here:
[https://github.com/codesharp-hu/JNIGenerator/blob/master/example-output/jni-wrapper.c](https://github.com/codesharp-hu/JNIGenerator/blob/master/example-output/jni-wrapper.c)

## Rules
### Structs
Structs in the following form are supported:
```C
struct Message {
    int isUrgent;
    char* subject;
    char* text;
};
```
It is important that the opening bracket (`{`) is written in the same line as the `struct`.

`typedef`ed structs (and other `typedef`s) are **not** supported:
```C
typedef struct Message {
    int isUrgent;
    char* subject;
    char* text;
} Message;
```
### Pointers
Pointers are interpreted as arrays, char pointers are interpreted as strings. With the exception of `char*` do not return raw pointers from functions, and do not use them as parameters. Pointers (arrays) must always be wrapped in structs, and must have a corresponding length variable. This length variable must:
 * be of type int,
 * must be named the same as the array with the "Length" suffix,
 * must be directly before the array variable.
 example:
```C
struct Contact {
    char* name;
    char* email;
};
struct Message {
    int isUrgent;
    char* subject;
    char* text;
    struct Contact sender;
    int addresseesLength;
    struct Contact* addressees;
};
```
#### Constant length arrays
Alternatively, if you has constant an array with constant length, you can indicate it with a comment:
```C
struct Message {
    int isUrgent;
    char* subject;
    char* text;
    struct Contact sender;
    int addresseesLength;
    struct Contact* addressees;//array-length:5
};
```

### Exclusion from JNI
You might exclude a function or a struct from the JNI by puttinga `//no-jni` comment directy behind it.
Excluding a struct:
```C
struct Message {
    int isUrgent;
    char* subject;
    char* text;
};//no-jni
```
Excluding a function:


```C
void sendMessage(struct Message message);//no-jni
```
