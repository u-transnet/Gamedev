package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Preset;
import org.springframework.data.jpa.repository.JpaRepository;

/**
 * Created by Artem on 31.05.2018.
 */
public interface PresetRepository extends JpaRepository<Preset, Long> {

    Preset findByName(String name);
}
