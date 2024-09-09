CREATE DATABASE MJS_PLAYER

USE MJS_PLAYER

CREATE TABLE player (
	player_id BIGINT NOT NULL IDENTITY PRIMARY KEY,
	nome VARCHAR(50) NOT NULL,
	pontuacao INT NOT NULL DEFAULT 0,
	data_registro DATETIME DEFAULT GETDATE()
);

INSERT INTO player (nome, pontuacao) VALUES ('Carlos Silva', 1500);

INSERT INTO player (nome) VALUES ('Ana Souza');

INSERT INTO player (nome, pontuacao) VALUES ('Mariana Oliveira', 2000);

INSERT INTO player (nome) VALUES ('Pedro Mendes');
