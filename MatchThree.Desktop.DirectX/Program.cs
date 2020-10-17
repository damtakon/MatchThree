using System;
using Shared;

namespace MatchThree.Desktop.DirectX
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using (var game = new MainGame(Content.Load, AutoFacDesktop.Register))
            game.Run();
        }
    }
}