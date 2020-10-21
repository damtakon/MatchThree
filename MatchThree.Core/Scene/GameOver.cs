using Autofac;
using MatchThree.Core.Control;
using MatchThree.Core.Extension;
using MatchThree.Core.Input;
using MatchThree.Core.MatchThree;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Scene
{
    public class GameOver : GameSceneBase
    {
        private readonly VectorInput _vectorInput;

        public GameOver(VectorInput vectorInput)
        {
            _vectorInput = vectorInput;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            var font = Content.Load<SpriteFont>(GameResource.FontPath);
            var gameOverText = "GameOver";
            var gameOver = new TextWrapper(font,
                new Rectangle((int) font.CenterX(gameOverText), 960, (int) Global.VirtualWidth, 250), gameOverText);

            var scoreText = $"Score: {Score.LastScore}";
            var score = new TextWrapper(font,
                new Rectangle((int) font.CenterX(scoreText), 1210, (int) Global.VirtualWidth, 250), scoreText);

            var buttonText = "Ok";
            var ok = new Button(font, _vectorInput, OkClicked, buttonText, new Vector2(font.CenterX(buttonText), 1460));

            UpdateDrawables.Add(gameOver);
            UpdateDrawables.Add(score);
            UpdateDrawables.Add(ok);
            UpdateDrawables.Add(_vectorInput);
        }

        private void OkClicked()
        {
            SceneManager.ChangeScene(typeof(MainMenu));
        }
    }
}