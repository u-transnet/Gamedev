package com.github.utransnet.utranscalc.server.transport.messages;

import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.data.BaseObjectMaterial;
import lombok.Data;

/**
 * Created by Artem on 07.06.2018.
 */
@Data
public class BaseObjectMaterialMessage implements Message<Protos.BaseObjectMaterial> {

    private MaterialMessage material;
    private int amount;
    private boolean onExploitation;
    private boolean userEditable;
    int groupNumber;
    int numberInGroup;

    public BaseObjectMaterialMessage(BaseObjectMaterial baseObjectMaterial) {
        material = new MaterialMessage(baseObjectMaterial.getMaterial());
        amount = baseObjectMaterial.getAmount();
        onExploitation = baseObjectMaterial.isOnExploitation();
        userEditable = baseObjectMaterial.isUserEditable();
        groupNumber = baseObjectMaterial.getGroupNumber();
        numberInGroup = baseObjectMaterial.getNumberInGroup();
    }

    @Override
    public Protos.BaseObjectMaterial toProto() {
        return Protos.BaseObjectMaterial.newBuilder()
                .setMaterialId(material.getId())
                .setAmount(amount)
                .setOnExploitation(onExploitation)
                .setUserEditable(userEditable)
                .setGroupNumber(groupNumber)
                .setNumberInGroup(numberInGroup)
                .build();
    }
}
