﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace SolitaireMahjongApp.Models
{
    public partial class Tile : ObservableObject
    {
        public int id { get; set; }
        public string cor { get; set; }
        public string simbolo { get; set; }
        public int pontuacao { get; set; }
        public string nomeImagem { get; set; }

        public string CaminhoImagem => $"http://localhost:8080/pecas/image/{id}";

        [ObservableProperty]
        public Color color;
    }
}