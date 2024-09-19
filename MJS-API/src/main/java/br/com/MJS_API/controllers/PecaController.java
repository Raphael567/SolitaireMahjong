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
    public ResponseEntity<List<Peca>> getAllPecas(){
        List<Peca> pecas = pecaService.getAllPecas();
        return new ResponseEntity<>(pecas, HttpStatus.OK);
    }

    @GetMapping("/simbolo/{simbolo}")
    public ResponseEntity<List<Peca>> getPecaBySimbolo(@PathVariable String simbolo){
        List<Peca> pecas = pecaService.getPecaBySimbolo(simbolo);
        return new ResponseEntity<>(pecas, HttpStatus.OK);
    }

    @GetMapping("/cor/{cor}")
    public ResponseEntity<List<Peca>> getPecaByCor(@PathVariable String cor){
        List<Peca> pecas = pecaService.getPecaByCor(cor);
        return new ResponseEntity<>(pecas, HttpStatus.OK);
    }

}
