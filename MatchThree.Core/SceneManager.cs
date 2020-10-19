using System.Collections.Generic;
using MatchThree.Core.Enum;
using MatchThree.Core.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core
{
    public static class SceneManager
    {
        private static GameSceneBase _currentScene;
        private static ContentManager _contentManager;
        private static SceneEnum _scene;
        private static bool _needChangeScene;
        private static Dictionary<SceneEnum, GameSceneBase> _gameScenes;

        /// <summary>
        /// Init all scene and main scene
        /// </summary>
        public static void Init()
        {
            _currentScene = new MainMenu();
            _scene = SceneEnum.MainMenu;
            _gameScenes = new Dictionary<SceneEnum, GameSceneBase>
            {
                {SceneEnum.MainMenu, new MainMenu()},
                {SceneEnum.LevelOne, new LevelOne()}
            };

        }

        public static void LoadContent(ContentManager contentManager)
        {
            AutoFacFactory.Build();
            _contentManager = contentManager;
            _currentScene.LoadContent(contentManager);
        }

        public static void ChangeScene(SceneEnum scene)
        {
            if (_scene != scene)
            {
                _needChangeScene = true;
                _scene = scene;
            }
        }

        private static void ChangeScene()
        {
            _currentScene.UnloadContent();
            _currentScene = _gameScenes[_scene];
            _currentScene.LoadContent(_contentManager);
            _needChangeScene = false;
        }

        public static void UnloadContent()
        {
            _currentScene.UnloadContent();
        }

        public static void Update(GameTime gameTime)
        {
            _currentScene.Update(gameTime);
            if (_needChangeScene)
                ChangeScene();
        }

        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _currentScene.Draw(spriteBatch, gameTime);
        }
    }
}