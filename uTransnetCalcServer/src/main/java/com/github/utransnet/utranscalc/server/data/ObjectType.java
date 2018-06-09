package com.github.utransnet.utranscalc.server.data;

/**
 * Created by Artem on 04.06.2018.
 */
public enum ObjectType {
    link(1),
    point(2),
    station(3);

    public int id;

    ObjectType(int id) {
        this.id = id;
    }
}
