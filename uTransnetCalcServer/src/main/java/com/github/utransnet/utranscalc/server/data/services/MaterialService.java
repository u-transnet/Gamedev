package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.BaseObjectMaterial;
import com.github.utransnet.utranscalc.server.data.Material;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.stream.Collectors;

/**
 * Created by Artem on 04.06.2018.
 */
@Service
public class MaterialService extends AbstractService<Material, MaterialRepository> {

    private final PresetMaterialService presetMaterialService;
    private final BaseObjectMaterialService baseObjectMaterialService;

    public MaterialService(MaterialRepository repository, PresetMaterialService presetMaterialService, BaseObjectMaterialService baseObjectMaterialService) {
        super(repository);
        this.presetMaterialService = presetMaterialService;
        this.baseObjectMaterialService = baseObjectMaterialService;
    }

    @Override
    @Transactional
    public void delete(Material entity) {
        entity = getOne(entity.getId());
        entity.getPresetMaterials().size();
        entity.getPresetMaterials().forEach(presetMaterialService::delete);

        // avoiding concurrent modification exception
        new ArrayList<>(entity.getBaseObjectMaterials()).forEach(baseObjectMaterialService::deleteNotSaving);
        super.delete(entity);
    }
}
