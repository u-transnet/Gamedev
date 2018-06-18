package com.github.utransnet.utranscalc.server.web;

import com.github.utransnet.utranscalc.server.data.Line;
import com.github.utransnet.utranscalc.server.data.services.LinePriceService;
import com.vaadin.data.converter.StringToIntegerConverter;
import com.vaadin.ui.Component;
import com.vaadin.ui.Label;
import com.vaadin.ui.TextField;
import org.vaadin.viritin.fields.MTextField;
import org.vaadin.viritin.form.AbstractForm;
import org.vaadin.viritin.label.MLabel;
import org.vaadin.viritin.layouts.MFormLayout;
import org.vaadin.viritin.layouts.MVerticalLayout;

/**
 * Created by Artem on 01.06.2018.
 */
public class LinePriceForm extends AbstractForm<Line> {
    private final Line entity;

//    private final LinePriceService service;
//    private final LineType lineType;

    Label label;
    private final TextField cement;
    private final TextField metal;

    public LinePriceForm(LinePriceService service, Line entity) {
        super(Line.class);
        this.entity = entity;
        label = new MLabel(this.entity.getLineType().name() + " prices");
        cement = new MTextField("Cement");
        metal = new MTextField("Metal");


        setSavedHandler(linePrice -> {
            linePrice.setLineType(this.entity.getLineType());
            service.save(linePrice);
        });
    }

    @Override
    protected void bind() {
        /*getBinder()
                .forMemberField(cement)
                .withConverter(new StringToIntegerConverter("Must enter a number"))
                .bind(Line::getCement, Line::setCement);
        getBinder()
                .forMemberField(metal)
                .withConverter(new StringToIntegerConverter("Must enter a number"))
                .bind(Line::getMetal, Line::setMetal);*/
        super.bind();
    }

    @Override
    public void setEntity(Line entity) {
        bind();
        super.setEntity(entity);
    }

    @Override
    protected void lazyInit() {
        if (getCompositionRoot() == null) {
            setCompositionRoot(createContent());
        }
    }

    public void init() {
        setEntity(this.entity);
    }

    @Override
    protected Component createContent() {
        return new MVerticalLayout(
                new MFormLayout(
                        label,
                        cement,
                        metal
                ).withWidth(""),
                getToolbar()
        ).withWidth("");
    }
}
