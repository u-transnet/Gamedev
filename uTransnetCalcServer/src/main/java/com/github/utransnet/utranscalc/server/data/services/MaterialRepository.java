package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Material;
import org.springframework.data.jpa.repository.JpaRepository;

/**
 * Created by Artem on 31.05.2018.
 */
public interface MaterialRepository extends JpaRepository<Material, Long> {
}
