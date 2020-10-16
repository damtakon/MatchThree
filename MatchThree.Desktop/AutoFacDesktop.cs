using System.Threading;
using MatchThree.Desktop.Input;
using Shared;
using Shared.Input;

namespace MatchThree.Desktop
{
    public static class AutoFacDesktop
    {
        public static void Register()
        {
            AutoFacFactory.RegisterInstance(new MouseInput(Content.Mouse), typeof(VectorInput));
        }
    }
}