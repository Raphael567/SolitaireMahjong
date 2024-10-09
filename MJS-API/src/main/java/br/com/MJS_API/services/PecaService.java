package br.com.MJS_API.services;

import br.com.MJS_API.entities.Peca;
import br.com.MJS_API.exceptions.ImageNotFoundException;
import br.com.MJS_API.exceptions.PecaNotFoundException;
import br.com.MJS_API.repositories.PecaRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.NoSuchFileException;
import java.nio.file.Path;
import java.nio.file.Paths;
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

    public Peca getPecaById(Long id) {
        Optional<Peca> peca = pecaRepository.findById(id);
        if(peca.isEmpty()) {
            throw new PecaNotFoundException("Peça with id " + id + " not found");
        }
        return peca.get();
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

    public byte[] loadImageBytes(String nomeImagem) throws ImageNotFoundException {
        try {
            Path path = Paths.get("E:/SolitaireMahjong/MJS-API/src/main/resources/images/pecas/" + nomeImagem);
            return Files.readAllBytes(path);
        } catch (NoSuchFileException e) {
            throw new ImageNotFoundException("Image not found", e);
        }
        catch (IOException e) {
            e.printStackTrace();
            throw new ImageNotFoundException("Error loading image", e);
        }
    }
}
