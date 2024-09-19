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
        try {
            return playerRepository.findAll();
        } catch (Exception e) {
            throw new RuntimeException("Error while trying to get all players");
        }
    }

    // Exibe um jogador específico
    public Player getPlayerById(Long id) {
        Optional<Player> newPlayer = playerRepository.findById(id);
        return newPlayer.orElseThrow(() -> new PlayerNotFoundException("Player with ID " + id + " not found"));
    }

    // Cria um jogador
    public Player createPlayer(Player player) {
        if (playerRepository.existsById(player.getId())) {
            throw new RuntimeException("Player already exists");
        }
        try {
            return playerRepository.save(player);
        } catch (Exception e) {
            throw new RuntimeException("Error while trying to create player");
        }
    }

    // Atualiza um jogador
    @Transactional
    public Player updatePlayer(Long id, Player player) {
        if (!playerRepository.existsById(id)) {
            throw new PlayerNotFoundException("Player with ID " + id + " not found");
        }
        player.setId(id);
        try {
            return playerRepository.save(player);
        } catch (Exception e) {
            throw new RuntimeException("Error while trying to update player");
        }
    }

    // Deleta um jogador
    @Transactional
    public void deletePlayer(Long id) {
        if (!playerRepository.existsById(id)) {
            throw new PlayerNotFoundException("Player with ID " + id + " not found");
        }
        try {
            playerRepository.deleteById(id);
        } catch (Exception e) {
            throw new RuntimeException("Error while trying to delete player");
        }
    }

    // Exibe jogadores com pontuação maior ou igual a uma pontuação específica
    public List<Player> getPlayersByPontuacao(int pontuacao) {
        try {
            Sort sortByScoreDescending = Sort.by(Sort.Order.desc("pontuacao"));
            return playerRepository.findByPontuacaoGreaterThanEqual(pontuacao, sortByScoreDescending);
        } catch (Exception e) {
            throw new RuntimeException("Error while trying to get players by score");
        }
    }
}
