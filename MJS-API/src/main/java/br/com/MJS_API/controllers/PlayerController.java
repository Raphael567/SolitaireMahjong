package br.com.MJS_API.controllers;

import br.com.MJS_API.entities.Player;
import br.com.MJS_API.services.PlayerService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Optional;

@RestController
@RequestMapping("/players")
public class PlayerController {
    @Autowired
    private PlayerService playerService;

    @GetMapping
    public List<Player> getAllPlayers() {
        return playerService.getAllPlayers();
    }

    // Exibe um jogador específico
    @GetMapping("/{id}")
    public Optional<Player> getPlayerById(@PathVariable Long id) {
        return playerService.getPlayerById(id);
    }

    // Cria um jogador
    @PostMapping
    public Player createPlayer(@RequestBody Player player) {
        return  playerService.createPlayer(player);
    }

    // Atualiza um jogador
    @PutMapping("/{id}")
    public Player updatePlayer(@PathVariable Long id, @RequestBody Player player) {
        return playerService.updatePlayer(id, player);
    }

    // Deleta um jogador
    @DeleteMapping("/{id}")
    public void deletePlayer(@PathVariable Long id) {
        playerService.deletePlayer(id);
    }

    // Exibe jogadores com pontuação maior ou igual a uma pontuação específica
    @GetMapping("/ranking")
    public List<Player> getPlayersByScore(@RequestParam(defaultValue = "0") int pontuacao) {
        return playerService.getPlayersByPontuacao(pontuacao);
    }
}
