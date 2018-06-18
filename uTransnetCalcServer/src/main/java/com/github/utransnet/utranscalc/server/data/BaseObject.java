package com.github.utransnet.utranscalc.server.data;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.ToString;

import javax.persistence.*;
import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;
import java.util.Set;

import static javax.persistence.CascadeType.*;

/**
 * Created by Artem on 04.06.2018.
 */
@Entity
@Data
@EqualsAndHashCode(of = {"id"})
@ToString(exclude = {"baseObjectMaterials"})
@Table(
        uniqueConstraints = @UniqueConstraint(columnNames = {"name"})
)
public class BaseObject {

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;

    @OneToMany(mappedBy = "baseObject", cascade = {PERSIST, MERGE, REFRESH, DETACH}, orphanRemoval = true, fetch = FetchType.EAGER)
    private Set<BaseObjectMaterial> baseObjectMaterials;

    @NotNull
    private String name;

    @NotNull
    private ObjectType objectType;

    @Min(0)
    private int minSize;

    @Min(0)
    private int maxSize;
}
