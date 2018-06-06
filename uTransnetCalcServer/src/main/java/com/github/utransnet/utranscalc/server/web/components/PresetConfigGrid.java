package com.github.utransnet.utranscalc.server.web.components;

import com.github.utransnet.utranscalc.server.data.*;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.github.utransnet.utranscalc.server.data.services.PresetMaterialService;
import com.github.utransnet.utranscalc.server.data.services.PresetService;
import com.vaadin.ui.*;

import java.util.List;
import java.util.Set;

/**
 * Created by Artem on 04.06.2018.
 */
public class PresetConfigGrid extends CustomField<Set<PresetMaterial>> {

    private final Preset preset;
    private final PresetMaterialService service;
    private final MaterialService materialService;
    private final PresetService presetService;

    public PresetConfigGrid(
            Preset preset,
            PresetMaterialService service,
            MaterialService materialService,
            PresetService presetService
    ) {
        this.service = service;
        this.materialService = materialService;
        this.preset = preset;
        this.presetService = presetService;
    }

    @Override
    protected Component initContent() {
        FormLayout formLayout = new FormLayout();
        List<PresetMaterial> presetMaterialsForPreset = presetService.getPresetMaterialsForPreset(preset);
        presetMaterialsForPreset.forEach(presetMaterial -> {
            TextField field = new TextField(
                    presetMaterial.getMaterial().getName(),
                    String.valueOf(presetMaterial.getPrice())
            );
            field.addValueChangeListener(event -> {
                presetMaterial.setPrice(Integer.parseInt(event.getValue()));
                service.save(presetMaterial);
            });
            formLayout.addComponent(field);
        });


        return new VerticalLayout(
                new Label("Preset name: "  + preset.getName()),
                formLayout
        );
    }

    @Override
    protected void doSetValue(Set<PresetMaterial> value) {

    }

    @Override
    public Set<PresetMaterial> getValue() {
        return null;
    }
}
