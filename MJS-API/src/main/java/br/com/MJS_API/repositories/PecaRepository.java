package br.com.MJS_API.repositories;

import br.com.MJS_API.entities.Peca;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import javax.swing.text.html.Option;
import java.util.List;
import java.util.Optional;

public interface PecaRepository extends JpaRepository<Peca, Long> {
    List<Peca> findBySimbolo(String simbolo);
    List<Peca> findByCor(String cor);
}
