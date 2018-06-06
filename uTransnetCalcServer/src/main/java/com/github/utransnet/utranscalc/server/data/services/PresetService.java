package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Material;
import com.github.utransnet.utranscalc.server.data.PresetMaterial;
import com.github.utransnet.utranscalc.server.data.Preset;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Artem on 31.05.2018.
 */
@Service
public class PresetService extends AbstractService<Preset, PresetRepository> {


    private final PresetMaterialService presetMaterialService;
    private final MaterialService materialService;

    public PresetService(PresetRepository repository, PresetMaterialService presetMaterialService, MaterialService materialService) {
        super(repository);
        this.presetMaterialService = presetMaterialService;
        this.materialService = materialService;
    }

    @Transactional
    public List<PresetMaterial> getPresetMaterialsForPreset(Preset preset) {
        List<PresetMaterial> list = new ArrayList<>();
        List<PresetMaterial> allByPreset = presetMaterialService.findAllByPreset(preset);
        List<Material> materials = materialService.findAll();

        materials.forEach(material -> {
            PresetMaterial presetMaterial = allByPreset.stream()
                    .filter(presetMaterialTmp -> presetMaterialTmp.getMaterial() == material)
                    .findFirst()
                    .orElseGet(() -> createPresetMaterial(material, preset));
            list.add(presetMaterial);
        });

        return list;
    }

    private PresetMaterial createPresetMaterial(Material material, Preset preset) {
        PresetMaterial newPresetMaterial = new PresetMaterial(preset, material);

        preset.getPresetMaterials().add(newPresetMaterial);
        material.getPresetMaterials().size();
        material.getPresetMaterials().add(newPresetMaterial);
        presetMaterialService.saveAndFlush(newPresetMaterial);

        this.saveAndFlush(preset);
        materialService.saveAndFlush(material);


        return newPresetMaterial;
    }

    @Override
    public void delete(Preset entity) {
        entity.getPresetMaterials().forEach(presetMaterialService::delete);
        super.delete(entity);
    }
}
