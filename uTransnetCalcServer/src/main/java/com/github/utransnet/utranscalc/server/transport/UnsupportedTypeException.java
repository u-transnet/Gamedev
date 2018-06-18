package com.github.utransnet.utranscalc.server.transport;

import com.github.utransnet.utranscalc.Protos;

/**
 * Created by Artem on 07.06.2018.
 */
public class UnsupportedTypeException extends Exception {

    Protos.Type type;

    public UnsupportedTypeException(Protos.Type type) {
        super("Unsupported type: " + type.name());
        this.type = type;
    }
}
