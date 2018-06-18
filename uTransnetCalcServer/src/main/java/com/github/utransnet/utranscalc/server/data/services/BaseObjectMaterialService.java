package com.github.utransnet.utranscalc.server.data.services;

import com.github.utransnet.utranscalc.server.data.BaseObject;
import com.github.utransnet.utranscalc.server.data.BaseObjectMaterial;
import com.github.utransnet.utranscalc.server.data.Material;
import org.springframework.stereotype.Service;

import java.util.List;

/**
 * Created by Artem on 04.06.2018.
 */
@Service
public class BaseObjectMaterialService extends AbstractService<BaseObjectMaterial, BaseObjectMaterialRepository> {

    private final MaterialRepository materialRepository;
    private final BaseObjectRepository baseObjectRepository;

    public BaseObjectMaterialService(BaseObjectMaterialRepository repository, MaterialRepository materialRepository, BaseObjectRepository baseObjectRepository) {
        super(repository);
        this.materialRepository = materialRepository;
        this.baseObjectRepository = baseObjectRepository;
    }


    public List<BaseObjectMaterial> findAllByBaseObject(BaseObject baseObject) {
        return repository.findAllByBaseObject(baseObject);
    }
    public List<BaseObjectMaterial> findAllByMaterial(Material material) {
        return repository.findAllByMaterial(material);
    }

    @Override
    public void delete(BaseObjectMaterial entity) {
        BaseObject baseObject = baseObjectRepository.getOne(entity.getBaseObject().getId());
        baseObject.getBaseObjectMaterials().remove(entity);
        baseObjectRepository.save(baseObject);

        Material material = materialRepository.getOne(entity.getMaterial().getId());
        material.getBaseObjectMaterials().remove(entity);
        materialRepository.save(material);
        super.delete(entity);
    }

    public void deleteNotSaving(BaseObjectMaterial entity) {
        BaseObject baseObject = baseObjectRepository.getOne(entity.getBaseObject().getId());
        baseObject.getBaseObjectMaterials().remove(entity);

        Material material = materialRepository.getOne(entity.getMaterial().getId());
        material.getBaseObjectMaterials().remove(entity);
        super.delete(entity);
    }
}
