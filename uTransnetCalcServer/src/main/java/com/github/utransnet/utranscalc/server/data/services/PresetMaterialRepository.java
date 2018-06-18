package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Material;
import com.github.utransnet.utranscalc.server.data.PresetMaterial;
import com.github.utransnet.utranscalc.server.data.Preset;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

/**
 * Created by Artem on 31.05.2018.
 */
public interface PresetMaterialRepository extends JpaRepository<PresetMaterial, Long> {

    List<PresetMaterial> findAllByPreset(Preset preset);
    List<PresetMaterial> findAllByMaterial(Material material);
}
