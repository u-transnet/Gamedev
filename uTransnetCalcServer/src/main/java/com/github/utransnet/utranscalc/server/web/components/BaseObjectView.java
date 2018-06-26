package com.github.utransnet.utranscalc.server.web.components;

import com.github.utransnet.utranscalc.server.data.BaseObject;
import com.github.utransnet.utranscalc.server.data.BaseObjectMaterial;
import com.github.utransnet.utranscalc.server.data.Material;
import com.github.utransnet.utranscalc.server.data.ObjectType;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectMaterialService;
import com.github.utransnet.utranscalc.server.data.services.BaseObjectService;
import com.github.utransnet.utranscalc.server.data.services.MaterialService;
import com.vaadin.data.Binder;
import com.vaadin.data.BinderValidationStatus;
import com.vaadin.navigator.View;
import com.vaadin.ui.*;
import org.jetbrains.annotations.NotNull;
import org.vaadin.crudui.crud.CrudOperation;
import org.vaadin.crudui.crud.impl.GridCrud;
import org.vaadin.crudui.form.CrudFormFactory;
import org.vaadin.viritin.fields.EnumSelect;
import org.vaadin.viritin.fields.IntegerField;
import org.vaadin.viritin.fields.LabelField;
import org.vaadin.viritin.fields.MTextField;
import org.vaadin.viritin.layouts.MFormLayout;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Artem on 05.06.2018.
 */
public class BaseObjectView extends VerticalLayout implements View {

    private final Binder<BaseObject> binder;
    private final List<Component> components = new ArrayList<>(5);


    private Button saveButton;
    private Button editButton;

    public BaseObjectView(BaseObject baseObject, BaseObjectService baseObjectService, BaseObjectMaterialService baseObjectMaterialService, MaterialService materialService) {
        setSizeFull();

        MFormLayout formLayout = new MFormLayout();
        binder = new Binder<>(BaseObject.class);

        LabelField id = new LabelField("id");
        formLayout.add(id);
        binder.bind(id, BaseObject::getId, BaseObject::setId);

        MTextField name = new MTextField("Name");
        formLayout.add(name);
        components.add(name);
        binder.bind(name, BaseObject::getName, BaseObject::setName);

        EnumSelect<ObjectType> objectType = new EnumSelect<>("Object type", ObjectType.class);
        formLayout.add(objectType);
        components.add(objectType);
        binder.bind(objectType, BaseObject::getObjectType, BaseObject::setObjectType);

        IntegerField minSize = new IntegerField("Min size");
        formLayout.add(minSize);
        components.add(minSize);
        binder.bind(minSize, BaseObject::getMinSize, BaseObject::setMinSize);

        IntegerField maxSize = new IntegerField("Max size");
        formLayout.add(maxSize);
        components.add(maxSize);
        binder.bind(maxSize, BaseObject::getMaxSize, BaseObject::setMaxSize);

        binder.bindInstanceFields(baseObject);
        binder.setBean(baseObject);

        addComponent(formLayout);

        if(baseObject.getId() > 0) { // saved object
            GridCrud<BaseObjectMaterial> materialsGrid = createMaterialsGrid(baseObject, baseObjectMaterialService, materialService);
            addComponent(materialsGrid);

            createControls(baseObjectService);
            setEnabled(false);
        }

    }

    @NotNull
    private GridCrud<BaseObjectMaterial> createMaterialsGrid(BaseObject baseObject, BaseObjectMaterialService baseObjectMaterialService, MaterialService materialService) {
        GridCrud<BaseObjectMaterial> materialsGrid = new GridCrud<>(BaseObjectMaterial.class);

        materialsGrid.setFindAllOperation(() -> baseObjectMaterialService.findAllByBaseObject(baseObject));
        materialsGrid.setAddOperation(entity -> {
            entity.setBaseObject(baseObject);
            return baseObjectMaterialService.saveAndFlush(entity);
        });
        materialsGrid.setUpdateOperation(baseObjectMaterialService::saveAndFlush);
        materialsGrid.setDeleteOperation(baseObjectMaterialService::delete);
        CrudFormFactory<BaseObjectMaterial> materialFormFactory = materialsGrid.getCrudFormFactory();
        materialFormFactory.setUseBeanValidation(true);
        materialFormFactory.setVisibleProperties(CrudOperation.READ, "id", "material", "amount", "onExploitation", "userEditable", "groupNumber", "numberInGroup");
        materialFormFactory.setVisibleProperties(CrudOperation.ADD, "material", "amount", "onExploitation", "userEditable", "groupNumber", "numberInGroup");
        materialFormFactory.setVisibleProperties(CrudOperation.UPDATE, "material", "amount", "onExploitation", "userEditable", "groupNumber", "numberInGroup");
        materialFormFactory.setVisibleProperties(CrudOperation.DELETE, "id", "material", "amount", "onExploitation", "userEditable", "groupNumber", "numberInGroup");
        materialsGrid.getGrid().setColumns("id", "material", "amount", "onExploitation", "userEditable", "groupNumber", "numberInGroup");
        materialFormFactory.setFieldProvider("material", () -> {
            ComboBox<Material> comboBox = new ComboBox<>("Material", materialService.findAll());
            comboBox.setEmptySelectionAllowed(false);
            comboBox.setItemCaptionGenerator(Material::getName);
            return comboBox;
        });
        return materialsGrid;
    }

    private void createControls(BaseObjectService baseObjectService) {
        saveButton = new Button(
                "Save changes",
                click -> {
                    baseObjectService.save(getBean());
                    editButton.setEnabled(true);
                    saveButton.setEnabled(false);
                    setEnabled(false);
                    Notification.show("Saved");
                }
        );
        saveButton.setEnabled(false);

        editButton = new Button(
                "Edit",
                click -> {
                    editButton.setEnabled(false);
                    saveButton.setEnabled(true);
                    setEnabled(true);
                }
        );

        Button deleteButton = new Button(
                "Delete",
                click -> {
                    baseObjectService.delete(getBean());
                    ((VerticalLayout)getParent()).removeComponent(this);
                }
        );


        addComponent(new HorizontalLayout(editButton, saveButton, deleteButton));
    }

    public BaseObject getBean() {
        return binder.getBean();
    }

    public BinderValidationStatus<BaseObject> validate() {
        return binder.validate();
    }

    @Override
    public void setEnabled(boolean enabled) {
        components.forEach(component -> component.setEnabled(enabled));
    }
}
