using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Scene
{
    public abstract class GameSceneBase : IUpdateDrawable
    {
        protected ContentManager Content;

        /// <summary>
        /// Load needed content for current scene
        /// </summary>
        public virtual void LoadContent(ContentManager content)
        {
            Content = new ContentManager(content.ServiceProvider, content.RootDirectory);
        }

        /// <summary>
        /// Unload content needed only this scene
        /// </summary>
        public virtual void UnloadContent()
        {
            Content?.Unload();
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}