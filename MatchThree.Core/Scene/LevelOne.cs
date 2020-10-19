using System.Collections.Generic;
using Autofac;
using MatchThree.Core.Input;
using MatchThree.Core.MatchThree;
using MatchThree.Core.MatchThree.Bonus;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MatchThree.Core.Enum;

#if DEBUG
using Microsoft.Xna.Framework.Input;

#endif

namespace MatchThree.Core.Scene
{
    public class LevelOne : GameSceneBase
    {
        private readonly List<IGemBonusFactory> _bonusFactories = new List<IGemBonusFactory>();
        private Board _board;
        private Score _score;

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

            _score = new Score(font, new Rectangle(2790, 50, 1000, 200));
            var timer = new Timer(font, new Rectangle(2790, 500, 1000, 200), OnTimeExpired, 60);

            var lineBonusFactory = new LineBonusFactory(Content.Load<Texture2D>(GameResource.LineHorizontalPath),
                Content.Load<Texture2D>(GameResource.LineVerticalPath),
                Content.Load<Texture2D>(GameResource.BreakerPath));
            var bombBonusFactory = new BombBonusFactory(Content.Load<Texture2D>(GameResource.BombPath));
            _bonusFactories.Add(bombBonusFactory);
            _bonusFactories.Add(lineBonusFactory);

            var cellTexture2D = Content.Load<Texture2D>(GameResource.CellPath);
            var boardContainer = new Rectangle(50, 50, 2740, 2060);
            _board = new Board(cellTexture2D, boardContainer, 8, 8, gemFactory, vectorInput);
            foreach (var bonus in _bonusFactories)
                _board.LineDestroy += bonus.LineDestroy;
            _board.LineDestroy += _score.LineDestroy;
            _board.GemDestroy += _score.GemDestroy;

            UpdateDrawables.Add(timer);
            UpdateDrawables.Add(_score);
            UpdateDrawables.Add(_board);
            UpdateDrawables.Add(vectorInput);
        }

        private void OnTimeExpired()
        {
            SceneManager.ChangeScene(SceneEnum.GameOver);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                SceneManager.ChangeScene(SceneEnum.MainMenu);
#endif
        }

        public override void UnloadContent()
        {
            foreach (var bonus in _bonusFactories)
                _board.LineDestroy -= bonus.LineDestroy;
            _board.LineDestroy -= _score.LineDestroy;
            _bonusFactories.Clear();
            _score = null;
            _board = null;
            base.UnloadContent();
        }
    }
}