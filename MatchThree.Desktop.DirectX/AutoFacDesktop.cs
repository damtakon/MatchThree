using MatchThree.Core;
using MatchThree.Core.Input;
using MatchThree.Desktop.DirectX.Input;

namespace MatchThree.Desktop.DirectX
{
    public static class AutoFacDesktop
    {
        public static void Register()
        {
            AutoFacFactory.RegisterInstance(new MouseInput(Content.Mouse), typeof(VectorInput));
        }
    }
}