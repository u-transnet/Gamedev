package com.github.utransnet.utranscalc.server.data;

import lombok.Data;

import javax.persistence.*;
import javax.validation.constraints.Min;
import java.io.Serializable;

/**
 * Created by Artem on 04.06.2018.
 */
@Entity
@Data
@Table(
        uniqueConstraints = @UniqueConstraint(columnNames = {"price_preset_id", "material_id"})
)
public class PresetMaterial implements Serializable {

    private static final long serialVersionUID = 1L;
    @Id
    @GeneratedValue(strategy = GenerationType.AUTO)
    private long id;
    @ManyToOne
    @JoinColumn(name = "price_preset_id")
    private Preset preset;
    @ManyToOne
    @JoinColumn(name = "material_id")
    private Material material;
    @Min(0)
    private int price;

    public PresetMaterial() {
    }

    public PresetMaterial(Preset preset, Material material) {
        this.preset = preset;
        this.material = material;
        this.price = 0;
    }
}
