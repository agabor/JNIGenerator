//  Generated code. Do not edit!

#include <stdlib.h>
#include "../example/example.h"
#include "jni.h"




jclass getMessageClass(JNIEnv const *jenv) {
    return (*jenv)->FindClass((JNIEnv *) jenv, "com/jnigen/model/Message");
}

jmethodID getMessageInitMethodId(JNIEnv const *jenv) {
    return (*jenv)->GetMethodID((JNIEnv *) jenv, getMessageClass(jenv), "<init>", "()V");
}

jobject createMessage(JNIEnv const *jenv) {
    return (*jenv)->NewObject((JNIEnv *) jenv, getMessageClass(jenv), getMessageInitMethodId(jenv));
}

jfieldID getMessageFieldID(JNIEnv const *jenv, char *field, char *type) {
    return (*jenv)->GetFieldID((JNIEnv *) jenv,
                               getMessageClass(
                                       jenv),
                               field, type);
}

jobject convertMessageToJobject(JNIEnv const *jenv, struct Message value) {
    jobject obj = createMessage(jenv);
    (*jenv)->SetIntField((JNIEnv*)jenv, obj, getMessageFieldID(jenv, "isUrgent", "I"), value.isUrgent);
    (*jenv)->SetObjectField((JNIEnv*)jenv, obj, getMessageFieldID(jenv, "subject", "Ljava/lang/String;"), (*jenv)->NewStringUTF((JNIEnv *) jenv, value.subject));
    (*jenv)->SetObjectField((JNIEnv*)jenv, obj, getMessageFieldID(jenv, "text", "Ljava/lang/String;"), (*jenv)->NewStringUTF((JNIEnv *) jenv, value.text));
    return obj;
}

struct Message convertJobjectToMessage(JNIEnv const *jenv, jobject obj) {
    struct Message result;
    result.isUrgent = (*jenv)->GetIntField((JNIEnv *) jenv, obj, getMessageFieldID(jenv, "isUrgent", "I"));
    result.subject = (*jenv)->GetStringUTFChars((JNIEnv *) jenv, (*jenv)->GetObjectField((JNIEnv *) jenv, obj, getMessageFieldID(jenv, "subject", "Ljava/lang/String;")), 0);
    result.text = (*jenv)->GetStringUTFChars((JNIEnv *) jenv, (*jenv)->GetObjectField((JNIEnv *) jenv, obj, getMessageFieldID(jenv, "text", "Ljava/lang/String;")), 0);
    return result;
}




JNIEXPORT
void JNICALL
Java_com_jnigen_JniApi_sendMessage(JNIEnv *jenv, jobject instance,
                                              jobject message) {
  sendMessage(
      convertJobjectToMessage(jenv, message)
  );
}
