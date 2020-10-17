using System.Collections.Generic;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Scene
{
    public abstract class GameSceneBase : IUpdateDrawable
    {
        protected ContentManager Content;
        protected readonly List<IUpdateDrawable> UpdateDrawables = new List<IUpdateDrawable>();

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
            UpdateDrawables.Clear();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var updateDrawable in UpdateDrawables)
                updateDrawable.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (var updateDrawable in UpdateDrawables)
                updateDrawable.Draw(spriteBatch, gameTime);
        }
    }
}