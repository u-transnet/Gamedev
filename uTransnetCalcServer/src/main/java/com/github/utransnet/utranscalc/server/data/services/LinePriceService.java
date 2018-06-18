package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.Line;
import com.github.utransnet.utranscalc.server.data.LineType;
import org.springframework.stereotype.Service;

import java.util.Optional;

/**
 * Created by Artem on 31.05.2018.
 */
@Service
public class LinePriceService extends AbstractService<Line, LinePriceRepository> {


    public LinePriceService(LinePriceRepository linePriceRepository) {
        super(linePriceRepository);
    }

    public Optional<Line> findByLineType(LineType lineType) {
        return repository.findFirstByLineType(lineType);
    }

}
