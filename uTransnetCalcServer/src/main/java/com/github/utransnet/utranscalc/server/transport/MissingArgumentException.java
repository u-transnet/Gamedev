package com.github.utransnet.utranscalc.server.transport;

import com.github.utransnet.utranscalc.Protos;

/**
 * Created by Artem on 07.06.2018.
 */
public class MissingArgumentException extends Exception {
    public MissingArgumentException(Protos.Type type, String argName) {
        super("Missing argument '" + argName + "' in method '" + type.name() + "'");
    }
}
