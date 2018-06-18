package com.github.utransnet.utranscalc.server.web.components;

import com.github.utransnet.utranscalc.server.data.services.BaseObjectMaterialService;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectService;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.vaadin.navigator.View;
import com.vaadin.ui.Button;
import com.vaadin.ui.HorizontalLayout;
import com.vaadin.ui.Notification;
import com.vaadin.ui.VerticalLayout;

import java.util.List;
import java.util.stream.Collectors;

/**
 * Created by Artem on 05.06.2018.
 */
public class BaseObjectList extends VerticalLayout implements View {


    public BaseObjectList(BaseObjectService baseObjectService, BaseObjectMaterialService baseObjectMaterialService, MaterialService materialService) {


        List<BaseObjectView> views = baseObjectService.findAll()
                .stream()
                .map(baseObject -> new BaseObjectView(baseObject, baseObjectService, baseObjectMaterialService, materialService))
                .peek(this::addComponent)
                .collect(Collectors.toList());


    }
}
