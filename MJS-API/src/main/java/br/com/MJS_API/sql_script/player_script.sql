CREATE DATABASE MJS_PLAYER
USE MJS_PLAYER

CREATE TABLE Player (
	player_id BIGINT NOT NULL IDENTITY PRIMARY KEY,
	nome VARCHAR(50) NOT NULL,
	pontuacao INT NOT NULL DEFAULT 0,
	data_registro DATETIME DEFAULT GETDATE()
);

drop table Player

CREATE TABLE Peca (
	peca_id BIGINT NOT NULL IDENTITY PRIMARY KEY,
	cor VARCHAR(20) NOT NULL,
	simbolo VARCHAR(30) NOT NULL,
	pontuacao INT NOT NULL
);

INSERT INTO Player (nome, pontuacao) VALUES ('Carlos Silva', 1500);
INSERT INTO Player (nome) VALUES ('Ana Souza');
INSERT INTO Player (nome, pontuacao) VALUES ('Mariana Oliveira', 2000);
INSERT INTO Player (nome) VALUES ('Pedro Mendes');


INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Leque', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'L�mpada', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Drag�o', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Caractere', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Flor', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Carpa', 1);

INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Amarelo', 'Leque', 2);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Amarelo', 'L�mpada', 2);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Amarelo', 'Drag�o', 2);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Amarelo', 'Caractere', 2);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Amarelo', 'Flor', 2);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Amarelo', 'Carpa', 2);

INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Azul', 'Leque', 3);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Azul', 'L�mpada', 3);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Azul', 'Drag�o', 3);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Azul', 'Caractere', 3);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Azul', 'Flor', 3);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Azul', 'Carpa', 3);

INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Leque', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'L�mpada', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Drag�o', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Caractere', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Flor', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Carpa', 5);

INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Roxo', 'Leque', 10);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Roxo', 'L�mpada', 10);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Roxo', 'Drag�o', 10);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Roxo', 'Caractere', 10);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Roxo', 'Flor', 10);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Roxo', 'Carpa', 10);