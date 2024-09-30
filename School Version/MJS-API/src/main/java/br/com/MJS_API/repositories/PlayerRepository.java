package br.com.MJS_API.repositories;

import br.com.MJS_API.entities.Player;
import org.springframework.data.domain.Sort;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface PlayerRepository extends JpaRepository<Player, Long> {
    List<Player> findByPontuacaoGreaterThanEqual(int pontuacao, Sort sort);
}
