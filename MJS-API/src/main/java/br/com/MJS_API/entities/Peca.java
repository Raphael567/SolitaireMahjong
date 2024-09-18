package br.com.MJS_API.entities;

import jakarta.persistence.*;
import lombok.Getter;
import org.springframework.stereotype.Service;

@Getter
@Service
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
}
