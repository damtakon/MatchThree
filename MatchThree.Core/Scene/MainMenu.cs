using System;
using Autofac;
using MatchThree.Core.Control;
using MatchThree.Core.Enum;
using MatchThree.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Scene
{
    public class MainMenu : GameSceneBase
    {
        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
            var font = Content.Load<SpriteFont>(GameResource.FontPath);
            VectorInput vectorInput;
            using (var scope = AutoFacFactory.Container.BeginLifetimeScope())
                vectorInput = scope.Resolve<VectorInput>();
            var buttonText = "Play";
            var center = Global.VirtualWidth / 2 - font.MeasureString(buttonText).X / 2;
            var play = new Button(font, vectorInput, PlayClicked, buttonText, new Vector2(center, 1470));
            var closeText = "X";
            var close = new Button(font, vectorInput, CloseClicked, closeText, new Vector2(Global.VirtualWidth - font.MeasureString(closeText).X, 0));

            UpdateDrawables.Add(play);
            UpdateDrawables.Add(close);
            UpdateDrawables.Add(vectorInput);
        }

        private void CloseClicked()
        {
            Environment.Exit(0);
        }

        private void PlayClicked()
        {
            SceneManager.ChangeScene(SceneEnum.LevelOne);
        }
    }
}