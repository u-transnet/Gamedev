package com.github.utransnet.utranscalc.server.transport.messages;

import com.github.utransnet.utranscalc.Protos;
import com.github.utransnet.utranscalc.server.data.Preset;
import com.google.protobuf.GeneratedMessageV3;
import lombok.Data;

/**
 * Created by Artem on 06.06.2018.
 */
@Data
public class PresetMessage implements Message<Protos.Preset> {
    private long id;
    private String name;

    public PresetMessage(Preset preset) {
        id = preset.getId();
        name = preset.getName();
    }

    @Override
    public Protos.Preset toProto() {
        return Protos.Preset.newBuilder()
                .setId(id)
                .setName(name)
                .build();
    }
}
