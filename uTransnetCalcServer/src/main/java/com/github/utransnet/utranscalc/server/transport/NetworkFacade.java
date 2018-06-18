package com.github.utransnet.utranscalc.server.transport;

import com.github.utransnet.utranscalc.server.data.services.BaseObjectService;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.github.utransnet.utranscalc.server.data.services.PresetService;
import com.github.utransnet.utranscalc.server.transport.messages.*;
import org.springframework.stereotype.Component;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;

import static java.util.stream.Collectors.toList;

/**
 * Created by Artem on 06.06.2018.
 */
@Component
public class NetworkFacade {

    private final PresetService presetService;
    private final BaseObjectService baseObjectService;
    private final MaterialService materialService;

    public NetworkFacade(
            PresetService presetService,
            BaseObjectService baseObjectService,
            MaterialService materialService
    ) {
        this.presetService = presetService;
        this.baseObjectService = baseObjectService;
        this.materialService = materialService;
    }

    public List<PresetMessage> listPresets() {
        return presetService.findAll().stream().map(PresetMessage::new).collect(toList());
    }

    @Transactional
    public List<PresetMaterialMessage> getPresetMaterials(Long id) {
        return presetService.getOne(id)
                .getPresetMaterials()
                .stream()
                .map(PresetMaterialMessage::new)
                .collect(toList());
    }

    public List<BaseObjectInfo> listBaseObjects() {
        return baseObjectService.findAll().stream().map(BaseObjectInfo::new).collect(toList());
    }

    @Transactional
    public List<BaseObjectMaterialMessage> getRequiredMaterialsForBaseObject(Long id) {
        return baseObjectService.getOne(id)
                .getBaseObjectMaterials()
                .stream()
                .map(BaseObjectMaterialMessage::new)
                .collect(toList());
    }

    public List<MaterialMessage> listMaterials() {
        return materialService.findAll().stream().map(MaterialMessage::new).collect(toList());
    }
}
