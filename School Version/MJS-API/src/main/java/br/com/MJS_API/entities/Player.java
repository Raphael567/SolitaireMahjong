package br.com.MJS_API.entities;

import jakarta.persistence.*;
import lombok.Data;

import java.time.LocalDateTime;

@Data
@Entity
@Table(name = "Player")
public class Player {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    @Column(name = "player_id")
    private Long id;

    private String nome;

    private int pontuacao;

    @Column(name = "data_registro")
    private LocalDateTime dataRegistro;
}
