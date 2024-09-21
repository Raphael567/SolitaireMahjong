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
	pontuacao INT NOT NULL,
	imagem VARCHAR(255) NOT NULL
);

drop table Peca

INSERT INTO Player (nome, pontuacao) VALUES ('Carlos Silva', 1500);
INSERT INTO Player (nome) VALUES ('Ana Souza');
INSERT INTO Player (nome, pontuacao) VALUES ('Mariana Oliveira', 2000);
INSERT INTO Player (nome) VALUES ('Pedro Mendes');

INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Leque', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Lumin�ria', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Drag�o', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Caractere', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Flor', 1);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Vermelho', 'Carpa', 1);

INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Amarelo', 'Leque', 2, 'leque_amarelo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Amarelo', 'Lumin�ria', 2, 'luminaria_amarelo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Amarelo', 'Drag�o', 2, 'dragao_amarelo.png');
-- INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Amarelo', 'Caractere', 2, 'caractere_amarelo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Amarelo', 'Flor', 2, 'flor_amarelo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Amarelo', 'Carpa', 2, 'carpa_amarelo.png');

INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Azul', 'Leque', 3, 'leque_azul.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Azul', 'Lumin�ria', 3, 'luminaria_azul.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Azul', 'Drag�o', 3, 'dragao_azul.png');
-- INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Azul', 'Caractere', 3, 'caractere_azul.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Azul', 'Flor', 3, 'flor_azul.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Azul', 'Carpa', 3, 'carpa_azul.png');

INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Leque', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Lumin�ria', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Drag�o', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Caractere', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Flor', 5);
INSERT INTO Peca (cor, simbolo, pontuacao) VALUES ('Verde', 'Carpa', 5);

INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Roxo', 'Leque', 10, 'leque_roxo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Roxo', 'Lumin�ria', 10, 'luminaria_roxo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Roxo', 'Drag�o', 10, 'dragao_roxo.png');
-- INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Roxo', 'Caractere', 10, 'caractere_roxo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Roxo', 'Flor', 10, 'flor_roxo.png');
INSERT INTO Peca (cor, simbolo, pontuacao, imagem) VALUES ('Roxo', 'Carpa', 10, 'carpa_roxo.png');
