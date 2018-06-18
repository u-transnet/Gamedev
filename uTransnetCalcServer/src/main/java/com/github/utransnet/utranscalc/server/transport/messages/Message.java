package com.github.utransnet.utranscalc.server.transport.messages;

import com.google.protobuf.GeneratedMessageV3;

/**
 * Created by Artem on 07.06.2018.
 */
public interface Message<T extends GeneratedMessageV3> {
    T toProto();
}
