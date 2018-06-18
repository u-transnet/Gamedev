package com.github.utransnet.utranscalc.server.transport.messages;

import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.data.BaseObject;
import com.google.protobuf.GeneratedMessageV3;
import lombok.Data;

/**
 * Created by Artem on 06.06.2018.
 */
@Data
public class BaseObjectInfo implements Message<Protos.BaseObject> {
    private long id;
    private String name;
    private int type;
    private int minSize;
    private int maxSize;

    public BaseObjectInfo() {

    }

    public BaseObjectInfo(BaseObject baseObject) {
        id = baseObject.getId();
        name = baseObject.getName();
        type = baseObject.getObjectType().id;
        minSize = baseObject.getMinSize();
        maxSize = baseObject.getMaxSize();

    }

    @Override
    public Protos.BaseObject toProto() {
        return Protos.BaseObject.newBuilder()
                .setId(id)
                .setName(name)
                .setType(type)
                .setMinSize(minSize)
                .setMaxSize(maxSize)
                .build();
    }
}
