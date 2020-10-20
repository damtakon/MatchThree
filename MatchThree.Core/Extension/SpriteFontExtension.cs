using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Extension
{
    public static class SpriteFontExtension
    {
        /// <summary>
        /// Finding the center on the X axis
        /// </summary>
        /// <param name="spriteFont">Font</param>
        /// <param name="text">Text</param>
        /// <returns>X Center</returns>
        public static float CenterX(this SpriteFont spriteFont, string text)
        {
            return Global.VirtualWidth / 2 - spriteFont.MeasureString(text).X / 2;
        }
    }
}