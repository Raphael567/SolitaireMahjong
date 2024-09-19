using CommunityToolkit.Mvvm.ComponentModel;

namespace SolitaireMahjongApp.Models
{
    public partial class Tile : ObservableObject
    {
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public Color color;

        [ObservableProperty]
        public bool isExposed;

        public int Row;

        public int Column;
    }
}