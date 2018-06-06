package com.github.utransnet.utranscalc.server.web;

import com.github.utransnet.utranscalc.server.data.*;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.github.utransnet.utranscalc.server.data.services.PresetMaterialService;
import com.github.utransnet.utranscalc.server.data.services.PresetService;
import com.github.utransnet.utranscalc.server.web.components.PresetConfigGrid;
import com.vaadin.server.VaadinRequest;
import com.vaadin.spring.annotation.SpringUI;
import com.vaadin.ui.Grid;
import com.vaadin.ui.UI;
import com.vaadin.ui.VerticalLayout;
import com.vaadin.ui.renderers.TextRenderer;
import org.vaadin.crudui.crud.CrudOperation;
import org.vaadin.crudui.crud.impl.GridCrud;
import org.vaadin.crudui.form.CrudFormFactory;

import java.util.Set;

/**
 * Created by Artem on 04.06.2018.
 */

@SpringUI(path = "preset")
public class PresetUI extends UI {

    private GridCrud<Material> materialsGrid = new GridCrud<>(Material.class);


    private final PresetService presetService;
    private final MaterialService materialService;
    private final PresetMaterialService presetMaterialService;

    public PresetUI(
            PresetService presetService,
            MaterialService materialService,
            PresetMaterialService presetMaterialService
    ) {
        this.presetService = presetService;
        this.materialService = materialService;
        this.presetMaterialService = presetMaterialService;
    }

    @Override
    protected void init(VaadinRequest request) {
        VerticalLayout layout = new VerticalLayout();

        layout.setWidth("100%");

        presetService.findAll().forEach(preset -> {
            layout.addComponent(new PresetConfigGrid(
                    preset,
                    presetMaterialService,
                    materialService,
                    presetService
            ));
        });
        layout.addComponentsAndExpand(materialsGrid);






        materialsGrid.setWidth("100%");
        CrudFormFactory<Material> materialFormFactory = materialsGrid.getCrudFormFactory();
        materialFormFactory.setUseBeanValidation(true);
        materialFormFactory.setVisibleProperties(CrudOperation.READ, "id", "name", "unit", "income");
        materialFormFactory.setVisibleProperties(CrudOperation.ADD, "name", "unit", "income");
        materialFormFactory.setVisibleProperties(CrudOperation.UPDATE, "name", "unit", "income");
        materialFormFactory.setVisibleProperties(CrudOperation.DELETE, "id", "name", "unit", "income");
        materialsGrid.getGrid().setColumns("id", "name", "unit", "income");

        materialsGrid.setFindAllOperation(materialService::findAll);
        materialsGrid.setAddOperation(materialService::saveAndFlush);
        materialsGrid.setUpdateOperation(materialService::saveAndFlush);
        materialsGrid.setDeleteOperation(materialService::delete);

        setContent(layout);
    }
}
