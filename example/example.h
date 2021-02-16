#ifndef MESSAGE_H
#define MESSAGE_H

struct Message {
    int isUrgent;
    char* subject;
    char* text;
};

void sendMessage(struct Message message);

#endif
