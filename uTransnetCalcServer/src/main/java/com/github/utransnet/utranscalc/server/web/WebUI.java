package com.github.utransnet.utranscalc.server.web;

import com.github.utransnet.utranscalc.server.data.BaseObject;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectMaterialService;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectService;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.github.utransnet.utranscalc.server.data.services.PresetService;
import com.github.utransnet.utranscalc.server.web.components.BaseObjectList;
import com.github.utransnet.utranscalc.server.web.components.BaseObjectView;
import com.vaadin.data.BinderValidationStatus;
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
    private BaseObjectList[] baseObjectList = new BaseObjectList[1];


    public WebUI(PresetService presetService, BaseObjectService baseObjectService, BaseObjectMaterialService baseObjectMaterialService, MaterialService materialService) {
        this.presetService = presetService;
        this.baseObjectService = baseObjectService;
        this.baseObjectMaterialService = baseObjectMaterialService;
        this.materialService = materialService;
    }

    @Override
    protected void init(VaadinRequest request) {
        VerticalLayout layout = new VerticalLayout();
        layout.setDefaultComponentAlignment(Alignment.MIDDLE_CENTER);
        baseObjectList[0] = new BaseObjectList(baseObjectService, baseObjectMaterialService, materialService);
        layout.addComponent(baseObjectList[0]);


        VerticalLayout popupContent = new VerticalLayout();


        PopupView popup = new PopupView(null, popupContent);
        popup.setHideOnMouseOut(false);
        // Fill the pop-up content when it's popped up
        popup.addPopupVisibilityListener(event -> {
            if (event.isPopupVisible()) {
                popupContent.removeAllComponents();
                BaseObjectView baseObjectView = new BaseObjectView(new BaseObject(), baseObjectService, baseObjectMaterialService, materialService);
                popupContent.addComponent(baseObjectView);
                popupContent.addComponent(
                        new Button(
                                "Save",
                                click -> {
                                    BinderValidationStatus<BaseObject> status = baseObjectView.validate();

                                    if (status.hasErrors()) {
                                        Notification.show("Validation error count: "
                                                + status.getValidationErrors().size());
                                    } else {
                                        try {
                                            baseObjectService.save(baseObjectView.getBean());
                                            layout.removeComponent(baseObjectList[0]);
                                            baseObjectList[0] = new BaseObjectList(baseObjectService, baseObjectMaterialService, materialService);
                                            layout.addComponentAsFirst(baseObjectList[0]);
                                            popup.setPopupVisible(false);
                                        } catch (Exception e) {
                                            Notification.show(e.getMessage());
                                        }
                                    }
                                }
                        )
                );
            }});
        Button button = new Button("Create new base object", click -> popup.setPopupVisible(true));


        layout.addComponent(button);
        layout.addComponent(popup);

        setContent(layout);
    }
}
