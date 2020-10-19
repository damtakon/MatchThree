using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Extension
{
    public static class SpriteFontExtension
    {
        public static float CenterX(this SpriteFont spriteFont, string text)
        {
            return Global.VirtualWidth / 2 - spriteFont.MeasureString(text).X / 2;
        }
    }
}