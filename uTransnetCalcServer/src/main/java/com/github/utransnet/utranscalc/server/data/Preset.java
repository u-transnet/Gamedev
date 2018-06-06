package com.github.utransnet.utranscalc.server.data;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.ToString;

import javax.persistence.*;
import javax.validation.constraints.Min;
import javax.validation.constraints.NotNull;
import java.io.Serializable;
import java.util.Set;

/**
 * Created by Artem on 31.05.2018.
 */
@Entity
@Data
@EqualsAndHashCode(of = {"id"})
@ToString(exclude = {"presetMaterials"})
public class Preset implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;

    @NotNull(message = "Name is required")
    private String name;

    @OneToMany(mappedBy = "preset", cascade = CascadeType.ALL, orphanRemoval = true, fetch = FetchType.EAGER)
    Set<PresetMaterial> presetMaterials;
}
