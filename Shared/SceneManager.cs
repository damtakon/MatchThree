using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Shared.Interface;
using Shared.Scene;

namespace Shared
{
    public static class SceneManager
    {
        private static GameSceneBase _currentScene;
        private static ContentManager _contentManager;

        /// <summary>
        /// Init all scene and main scene
        /// </summary>
        public static void Init()
        {
            _currentScene = new MainMenu();
        }

        public static void LoadContent(ContentManager contentManager)
        {
            AutoFacFactory.Build();
            _contentManager = contentManager;
            _currentScene.LoadContent(contentManager);
        }

        public static void UnloadContent()
        {
            _currentScene.UnloadContent();
        }

        public static void Update(GameTime gameTime)
        {
            _currentScene.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _currentScene.Draw(spriteBatch, gameTime);
        }
    }
}