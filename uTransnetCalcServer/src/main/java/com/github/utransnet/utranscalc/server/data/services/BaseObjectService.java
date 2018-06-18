package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.BaseObject;
import org.springframework.stereotype.Service;

/**
 * Created by Artem on 04.06.2018.
 */
@Service
public class BaseObjectService extends AbstractService<BaseObject, BaseObjectRepository> {

    private final BaseObjectMaterialService baseObjectMaterialService;

    public BaseObjectService(BaseObjectRepository repository, BaseObjectMaterialService baseObjectMaterialService) {
        super(repository);
        this.baseObjectMaterialService = baseObjectMaterialService;
    }

    @Override
    public void delete(BaseObject entity) {
        entity = getOne(entity.getId());
        entity.getBaseObjectMaterials().forEach(baseObjectMaterialService::deleteNotSaving);
        super.delete(entity);
    }
}
