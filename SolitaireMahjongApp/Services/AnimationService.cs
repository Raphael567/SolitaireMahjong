using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolitaireMahjongApp.Services
{
    public class AnimationService : IAnimationService
    {
        public async Task AnimateButtonAsync(ImageButton imageButton)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                Debug.WriteLine("Iniciando animação...");
                await AnimateShake(imageButton);
                Debug.WriteLine("Animação concluída.");
            });
        }

        private async Task AnimateShake(ImageButton imageButton)
        {
            try
            {
                await imageButton.FadeTo(0, 200); // Desvanece
                await imageButton.FadeTo(1, 200); // Retorna
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro durante a animação: {ex.Message}");
            }
        }
    }
}
