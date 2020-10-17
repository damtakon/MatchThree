using MatchThree.Desktop.DirectX.Input;
using Shared;
using Shared.Input;

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