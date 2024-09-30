using System.Diagnostics;

namespace SolitaireMahjongApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            };
        }
    }
}
