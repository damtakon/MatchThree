using MatchThree.Android.Input;
using MatchThree.Core;
using MatchThree.Core.Input;
using Microsoft.Xna.Framework;

namespace MatchThree.Android
{
    public static class AutoFacAndroid
    {
        public static void Register(GameWindow window)
        {
            AutoFacFactory.RegisterInstance(new TouchInput(window), typeof(VectorInput));
        }
    }
}