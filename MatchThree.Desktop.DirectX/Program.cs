using System;
using MatchThree.Core;
using MatchThree.Core.Enum;

namespace MatchThree.Desktop.DirectX
{
    public static class Program
    {
        [STAThread]
        private static void Main()
        {
            using var game = new MainGame(PlatformEnum.Windows, Content.Load, AutoFacDesktop.Register);
            game.Run();
        }
    }
}