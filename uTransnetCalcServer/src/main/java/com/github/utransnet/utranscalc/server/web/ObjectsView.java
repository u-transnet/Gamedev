package com.github.utransnet.utranscalc.server.web;

import com.github.utransnet.utranscalc.server.data.BaseObject;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectMaterialService;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectService;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.github.utransnet.utranscalc.server.web.components.BaseObjectList;
import com.github.utransnet.utranscalc.server.web.components.BaseObjectView;
import com.vaadin.data.BinderValidationStatus;
import com.vaadin.navigator.View;
import com.vaadin.ui.*;

/**
 * Created by Artem on 18.06.2018.
 */
public class ObjectsView extends VerticalLayout implements View {
    private BaseObjectList[] baseObjectList = new BaseObjectList[1];
    private final BaseObjectService baseObjectService;
    private final BaseObjectMaterialService baseObjectMaterialService;
    private final MaterialService materialService;

    public ObjectsView(BaseObjectService baseObjectService, BaseObjectMaterialService baseObjectMaterialService, MaterialService materialService) {
        this.baseObjectService = baseObjectService;
        this.baseObjectMaterialService = baseObjectMaterialService;
        this.materialService = materialService;
        setDefaultComponentAlignment(Alignment.MIDDLE_CENTER);

        baseObjectList[0] = new BaseObjectList(this.baseObjectService, this.baseObjectMaterialService, this.materialService);
        addComponent(baseObjectList[0]);


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
                                            removeComponent(baseObjectList[0]);
                                            baseObjectList[0] = new BaseObjectList(baseObjectService, baseObjectMaterialService, materialService);
                                            addComponentAsFirst(baseObjectList[0]);
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


        addComponent(button);
        addComponent(popup);
    }
}
