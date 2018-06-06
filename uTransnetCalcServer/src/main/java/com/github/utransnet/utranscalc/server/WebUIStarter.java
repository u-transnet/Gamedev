package com.github.utransnet.utranscalc.server;

import com.github.utransnet.utranscalc.server.data.*;
import com.github.utransnet.utranscalc.server.data.services.LinePriceService;
import com.github.utransnet.utranscalc.server.data.services.PresetService;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.event.EventListener;

import java.util.stream.Stream;

/**
 * Created by Artem on 31.05.2018.
 */

@SpringBootApplication
@EnableAutoConfiguration
@ComponentScan
public class WebUIStarter {


    private final LinePriceService linePriceService;
    private final PresetService presetService;

    public WebUIStarter(LinePriceService linePriceService, PresetService presetService) {
        this.linePriceService = linePriceService;
        this.presetService = presetService;
    }

    public static void main(String[] args) {
        SpringApplication.run(WebUIStarter.class, args);
    }

    @EventListener(ApplicationReadyEvent.class)
    public void doAfterStartup() {
        Stream.of(LineType.values()).forEach(lineType -> {
            linePriceService.findByLineType(lineType).orElseGet(() -> {
                Line line = new Line();
                line.setLineType(lineType);
                linePriceService.saveAndFlush(line);
                return line;
            });
        });

        if(presetService.count() == 0) {
            Preset preset = new Preset();
            preset.setName("default");
            presetService.saveAndFlush(preset);
        }
    }
}
