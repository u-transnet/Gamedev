package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.*;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

/**
 * Created by Artem on 04.06.2018.
 */
public interface BaseObjectMaterialRepository extends JpaRepository<BaseObjectMaterial, Long> {

    List<BaseObjectMaterial> findAllByBaseObject(BaseObject baseObject);
    List<BaseObjectMaterial> findAllByMaterial(Material material);
}
