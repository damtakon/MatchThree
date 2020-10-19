using Autofac;
using MatchThree.Core.Enum;
using MatchThree.Core.Input;
using MatchThree.Core.MatchThree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MatchThree.Core.Scene
{
    public class LevelOne : GameSceneBase
    {
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            var font = Content.Load<SpriteFont>(GameResource.FontPath);
            VectorInput vectorInput;
            using (var scope = AutoFacFactory.Container.BeginLifetimeScope())
                vectorInput = scope.Resolve<VectorInput>();
            var gemFactory = new RandomGemFactory(Content.Load<Texture2D>(GameResource.Gem1Path),
                Content.Load<Texture2D>(GameResource.Gem2Path), Content.Load<Texture2D>(GameResource.Gem3Path),
                Content.Load<Texture2D>(GameResource.Gem4Path), Content.Load<Texture2D>(GameResource.Gem5Path));
            var board = new Board(Content.Load<Texture2D>(GameResource.CellPath), new Rectangle(50, 50, 2740, 2060), 8,
                8, gemFactory, vectorInput);
            UpdateDrawables.Add(board);
            UpdateDrawables.Add(vectorInput);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(SceneEnum.MainMenu);
            }
        }
    }
}