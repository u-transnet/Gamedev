package com.github.utransnet.utranscalc.server.data;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.ToString;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.io.Serializable;
import java.util.Set;

import static javax.persistence.CascadeType.*;

/**
 * Created by Artem on 04.06.2018.
 */
@Entity
@Data
@EqualsAndHashCode(of = {"id"})
@ToString(exclude = {"presetMaterials", "baseObjectMaterials"})
@Table(
        uniqueConstraints = @UniqueConstraint(columnNames = {"name"})
)
public class Material implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;

    @NotNull
    private String name;

    @NotNull
    private Units unit;

    private boolean income = false;

    @OneToMany(mappedBy = "material", cascade = {PERSIST, MERGE, REFRESH, DETACH}, orphanRemoval = true)
    private Set<PresetMaterial> presetMaterials;

    @OneToMany(mappedBy = "material", cascade = {PERSIST, MERGE, REFRESH, DETACH}, orphanRemoval = true)
    private Set<BaseObjectMaterial> baseObjectMaterials;
}
