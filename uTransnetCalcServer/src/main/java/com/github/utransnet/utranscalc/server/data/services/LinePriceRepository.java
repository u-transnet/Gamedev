package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Line;
import com.github.utransnet.utranscalc.server.data.LineType;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.Optional;

/**
 * Created by Artem on 31.05.2018.
 */
public interface LinePriceRepository extends JpaRepository<Line, Long> {

    Optional<Line> findFirstByLineType(LineType lineType);
}
