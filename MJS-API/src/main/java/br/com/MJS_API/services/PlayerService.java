package br.com.MJS_API.services;

import br.com.MJS_API.entities.Player;
import br.com.MJS_API.exceptions.PlayerNotFoundException;
import br.com.MJS_API.repositories.PlayerRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class PlayerService {
    @Autowired
    private PlayerRepository playerRepository;

    // Exibe todos os jogadores
    public List<Player> getAllPlayers() {
        return playerRepository.findAll();
    }

    // Exibe um jogador específico
    public Player getPlayerById(Long id) {
        return playerRepository.findById(id)
                .orElseThrow(() -> new PlayerNotFoundException("Player with id " + id + " not found"));
    }

    // Cria um jogador
    public Player createPlayer(Player player) {
        return playerRepository.save(player);
    }

    // Atualiza um jogador
    @Transactional
    public Player updatePlayer(Long id, Player player) {
        if (playerRepository.existsById(id)) {
            player.setId(id);
            return playerRepository.save(player);
        } else {
            throw new PlayerNotFoundException("Player with id " + id + " not found");
        }
    }

    // Deleta um jogador
    @Transactional
    public void deletePlayer(Long id) {
        if (playerRepository.existsById(id)) {
            playerRepository.deleteById(id);
        } else {
            throw new PlayerNotFoundException("Player with id " + id + " not found");
        }
    }

    // Exibe jogadores com pontuação maior ou igual a uma pontuação específica
    public List<Player> getPlayersByPontuacao(int pontuacao) {
        Sort sortByScoreDescending = Sort.by(Sort.Order.desc("pontuacao"));
        return playerRepository.findByPontuacaoGreaterThanEqual(pontuacao, sortByScoreDescending);
    }
}
