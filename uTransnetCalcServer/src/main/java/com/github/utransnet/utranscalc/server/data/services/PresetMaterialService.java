package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Material;
import com.github.utransnet.utranscalc.server.data.Preset;
import com.github.utransnet.utranscalc.server.data.PresetMaterial;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * Created by Artem on 04.06.2018.
 */
@Service
public class PresetMaterialService extends AbstractService<PresetMaterial, PresetMaterialRepository> {
    public PresetMaterialService(PresetMaterialRepository repository) {
        super(repository);
    }

    public List<PresetMaterial> findAllByPreset(Preset preset) {
        return repository.findAllByPreset(preset);
    }

    public List<PresetMaterial> findAllByMaterial(Material material) {
        return repository.findAllByMaterial(material);
    }

    @Override
    public void delete(PresetMaterial entity) {
        entity.getMaterial().getPresetMaterials().remove(entity);
        entity.getPreset().getPresetMaterials().remove(entity);
        super.delete(entity);
    }
}
