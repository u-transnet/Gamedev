package com.github.utransnet.utranscalc.server.data;

import lombok.Data;

import javax.persistence.*;
import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Null;

/**
 * Created by Artem on 04.06.2018.
 */
@Entity
@Data
@Table(
        uniqueConstraints = @UniqueConstraint(columnNames={"base_object_id", "material_id", "onExploitation"})
)
public class BaseObjectMaterial {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;

    @ManyToOne()
    @JoinColumn(name = "base_object_id")
    private BaseObject baseObject;

    @ManyToOne()
    @JoinColumn(name = "material_id")
    private Material material;

    @Min(0)
    private int amount;

    private boolean onExploitation;

    private boolean userEditable = false;

    @Min(0)
    int groupNumber;

    @Min(0)
    int numberInGroup;
}
