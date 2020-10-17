using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Interface
{
    public interface IDrawable
    {
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="spriteBatch">Can be used to draw textures</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}