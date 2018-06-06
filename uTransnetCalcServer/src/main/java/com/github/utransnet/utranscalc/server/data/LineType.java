package com.github.utransnet.utranscalc.server.data;

/**
 * Created by Artem on 31.05.2018.
 */
public enum LineType {
    SOFT(1),
    RIGID(0);

    int id;

    LineType(int id) {
        this.id = id;
    }
}
