CREATE DATABASE MJS_PLAYER
USE MJS_PLAYER

CREATE TABLE Player (
	player_id BIGINT NOT NULL IDENTITY PRIMARY KEY,
	nome VARCHAR(50) NOT NULL,
	pontuacao INT NOT NULL DEFAULT 0,
	data_registro DATETIME DEFAULT GETDATE()
);

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

