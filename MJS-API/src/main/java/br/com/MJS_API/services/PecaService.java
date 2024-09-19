package br.com.MJS_API.services;

import br.com.MJS_API.entities.Peca;
import br.com.MJS_API.exceptions.PecaNotFoundException;
import br.com.MJS_API.repositories.PecaRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;
import java.util.Optional;

@Service
public class PecaService {
    @Autowired
    private PecaRepository pecaRepository;

    public List<Peca> getAllPecas() {
        try {
            return pecaRepository.findAll();
        } catch (Exception e) {
            throw new RuntimeException("Error while trying to get all pieces");
        }
    }

    public List<Peca> getPecaBySimbolo(String simbolo) {
        List<Peca> pecas = pecaRepository.findBySimbolo(simbolo);
        if(pecas.isEmpty()) {
            throw new PecaNotFoundException("Peça with symbol " + simbolo + " not found");
        }
        return pecas;
    }

    public List<Peca> getPecaByCor(String cor) {
        List<Peca> pecas = pecaRepository.findByCor(cor);
        if(pecas.isEmpty()) {
            throw new PecaNotFoundException("Peça with symbol " + cor + " not found");
        }
        return pecas;
    }
}
