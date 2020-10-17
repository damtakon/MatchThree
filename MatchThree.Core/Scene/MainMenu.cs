using Autofac;
using MatchThree.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Scene
{
    public class MainMenu : GameSceneBase
    {
        private Texture2D _background;
        private VectorInput _vectorInput;

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            _background = Content.Load<Texture2D>(GameResource.BackgroundPath);
            var font = Content.Load<SpriteFont>(GameResource.FontPath);
            using var scope = AutoFacFactory.Container.BeginLifetimeScope();
            _vectorInput = scope.Resolve<VectorInput>();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            _background = null;
        }

        public override void Update(GameTime gameTime)
        {
            _vectorInput.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            _vectorInput.Draw(spriteBatch, gameTime);
            spriteBatch.End();
        }
    }
}