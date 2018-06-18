package com.github.utransnet.utranscalc.server.web;

import com.github.utransnet.utranscalc.server.data.services.*;
import com.github.utransnet.utranscalc.server.web.components.BaseObjectList;
import com.vaadin.server.VaadinRequest;
import com.vaadin.spring.annotation.SpringUI;
import com.vaadin.ui.*;

/**
 * Created by Artem on 31.05.2018.
 */
@SpringUI
public class WebUI extends UI {


    private final PresetService presetService;
    private final BaseObjectService baseObjectService;
    private final BaseObjectMaterialService baseObjectMaterialService;
    private final MaterialService materialService;
    private final PresetMaterialService presetMaterialService;


    public WebUI(
            PresetService presetService,
            BaseObjectService baseObjectService,
            BaseObjectMaterialService baseObjectMaterialService,
            MaterialService materialService,
            PresetMaterialService presetMaterialService
    ) {
        this.presetService = presetService;
        this.baseObjectService = baseObjectService;
        this.baseObjectMaterialService = baseObjectMaterialService;
        this.materialService = materialService;
        this.presetMaterialService = presetMaterialService;
    }

    @Override
    protected void init(VaadinRequest request) {
        TabSheet tabsheet = new TabSheet();
        tabsheet.addTab(new ObjectsView(baseObjectService, baseObjectMaterialService, materialService))
                .setCaption("Objects");
        tabsheet.addTab(new PresetView(presetService, materialService, presetMaterialService))
                .setCaption("Presets");
        setContent(tabsheet);
    }
}
