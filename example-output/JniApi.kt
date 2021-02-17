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