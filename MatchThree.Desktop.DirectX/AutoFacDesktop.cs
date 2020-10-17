using MatchThree.Core;
using MatchThree.Core.Input;
using MatchThree.Desktop.DirectX.Input;
using Microsoft.Xna.Framework;

namespace MatchThree.Desktop.DirectX
{
    public static class AutoFacDesktop
    {
        public static void Register(GameWindow window)
        {
            AutoFacFactory.RegisterInstance(new MouseInput(Content.Mouse, window), typeof(VectorInput));
        }
    }
}