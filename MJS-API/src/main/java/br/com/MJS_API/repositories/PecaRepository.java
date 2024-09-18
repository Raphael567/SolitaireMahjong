package br.com.MJS_API.repositories;

import br.com.MJS_API.entities.Peca;
import org.springframework.data.jpa.repository.JpaRepository;

public interface PecaRepository extends JpaRepository<Peca, Long> {
    Peca findBySimbolo(String simbolo);
    Peca findByCor(String cor);
}
