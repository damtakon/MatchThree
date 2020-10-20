using Microsoft.Xna.Framework;

namespace MatchThree.Core
{
    public static class Global
    {
        public const float VirtualWidth = 3840;
        public const float VirtualHeight = 2160;
        public static Matrix ScaleMatrix;

        public static void SetWindowsVariables(GameWindow window)
        {
            //Fix for android build
            window.Position = Point.Zero;
            window.IsBorderless = true;
            window.AllowAltF4 = false;
        }
    }
}