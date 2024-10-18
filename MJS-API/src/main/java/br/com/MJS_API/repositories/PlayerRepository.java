package br.com.MJS_API.repositories;

import br.com.MJS_API.entities.Player;
import org.springframework.data.domain.Sort;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;

public interface PlayerRepository extends JpaRepository<Player, Long> {
    List<Player> findByPontuacaoGreaterThanEqual(int pontuacao, Sort sort);

    Boolean existsByNome(String nome);
}
