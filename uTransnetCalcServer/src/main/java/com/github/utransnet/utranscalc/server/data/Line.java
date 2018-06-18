package com.github.utransnet.utranscalc.server.data;

import lombok.Data;
import lombok.EqualsAndHashCode;
import lombok.ToString;

import javax.persistence.*;
import javax.validation.constraints.Min;
import java.io.Serializable;
import java.util.Set;

/**
 * Created by Artem on 31.05.2018.
 */
@Entity
@Data
@EqualsAndHashCode(of = {"id"})
@ToString(exclude = {"materials"})
public class Line implements Serializable {

    private static final long serialVersionUID = 1L;

    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;

    private LineType lineType;

    @OneToMany(cascade = CascadeType.ALL)
    @JoinTable(
            name = "line_materials",
            joinColumns = @JoinColumn(name = "line_id", referencedColumnName = "id"),
            inverseJoinColumns = @JoinColumn(name = "material_id", referencedColumnName = "id")
    )
    private Set<Material> materials;
}
