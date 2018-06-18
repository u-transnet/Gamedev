package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.BaseObject;
import org.springframework.data.jpa.repository.JpaRepository;

/**
 * Created by Artem on 04.06.2018.
 */
public interface BaseObjectRepository extends JpaRepository<BaseObject, Long> {
}
