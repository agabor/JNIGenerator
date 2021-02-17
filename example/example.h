#ifndef MESSAGE_H
#define MESSAGE_H

struct Message {
    char* subject;
    char* text;
};

int sendMessage(struct Message message);

#endif
