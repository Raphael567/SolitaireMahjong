package br.com.MJS_API.services;

import br.com.MJS_API.entities.Peca;
import br.com.MJS_API.repositories.PecaRepository;
import jakarta.transaction.Transactional;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class PecaService {
    @Autowired
    private PecaRepository pecaRepository;

    public List<Peca> getAllPecas() {
        return pecaRepository.findAll();
    }

    public Peca getPecaBySimbolo(String simbolo) {
        return pecaRepository.findBySimbolo(simbolo);
    }

    public Peca getPecaByCor(String cor) {
        return pecaRepository.findByCor(cor);
    }
}
