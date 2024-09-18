package br.com.MJS_API.controllers;

import br.com.MJS_API.entities.Peca;
import br.com.MJS_API.services.PecaService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/pecas")
public class PecaController {
    @Autowired
    private PecaService pecaService;

    @GetMapping
    public List<Peca> getAllPecas(){
        return pecaService.getAllPecas();
    }

    @GetMapping
    public Peca getPecaBySimbolo(String simbolo){
        return pecaService.getPecaBySimbolo(simbolo);
    }

    @GetMapping
    public Peca getPecaByCor(String cor){
        return pecaService.getPecaByCor(cor);
    }

}
