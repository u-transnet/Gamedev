package com.github.utransnet.utranscalc.server.transport.messages;

import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.data.PresetMaterial;
import lombok.Data;

/**
 * Created by Artem on 07.06.2018.
 */
@Data
public class PresetMaterialMessage implements Message<Protos.PresetMaterial>{

    private MaterialMessage material;
    private int price;

    public PresetMaterialMessage(PresetMaterial presetMaterial) {
        material = new MaterialMessage(presetMaterial.getMaterial());
        price = presetMaterial.getPrice();
    }

    @Override
    public Protos.PresetMaterial toProto() {
        return Protos.PresetMaterial.newBuilder()
                .setMaterialId(material.getId())
                .setPrice(price)
                .build();
    }
}
