package com.github.utransnet.utranscalc.server.transport.messages;

import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.data.Material;
import com.github.utransnet.utranscalc.server.data.Units;
import com.google.protobuf.GeneratedMessageV3;
import lombok.Data;

/**
 * Created by Artem on 06.06.2018.
 */
@Data
public class MaterialMessage implements Message<Protos.Material> {
    private long id;
    private String name;
    private Units unit;
    private boolean income;

    public MaterialMessage(Material material) {
        id = material.getId();
        name = material.getName();
        unit = material.getUnit();
        income = material.isIncome();
    }

    @Override
    public Protos.Material toProto() {
        return Protos.Material.newBuilder()
                .setId(id)
                .setName(name)
                .setUnit(Protos.Units.valueOf(unit.name()))
                .setIncome(income)
                .build();
    }
}
