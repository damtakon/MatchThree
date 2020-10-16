using MatchThree.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree
{
    public static class SceneManager
    {
        private static IGameScene _currentScene;

        /// <summary>
        /// Init all scene and main scene
        /// </summary>
        public static void Init()
        {
        }

        public static void LoadContent()
        {
            if (!_currentScene.IsLoaded)
                _currentScene.LoadContent();
        }

        public static void UnloadContent()
        {
            if (_currentScene.IsLoaded)
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