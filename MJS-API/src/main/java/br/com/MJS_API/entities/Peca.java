package br.com.MJS_API.entities;

import jakarta.persistence.*;
import lombok.Data;

@Data
@Entity
@Table(name = "Peca")
public class Peca {
    @Id
    @Column(name = "peca_id")
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private String cor;

    private String simbolo;

    private int pontuacao;

    @Column(name = "imagem")
    private String nomeImagem;
}
