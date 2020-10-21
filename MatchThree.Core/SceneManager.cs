using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static Type _scene;
        private static bool _needChangeScene;
        private static readonly Dictionary<Type, GameSceneBase> GameScenes = new Dictionary<Type, GameSceneBase>();

        public static void LoadContent(ContentManager contentManager)
        {
            AutoFacFactory.Build();
            _contentManager = contentManager;
            LoadAssemble(Assembly.GetExecutingAssembly().FullName);
            _currentScene = GameScenes[typeof(MainMenu)];
            _scene = typeof(MainMenu);
            _currentScene.LoadContent(contentManager);
        }

        public static void ChangeScene<T>() where T : GameSceneBase
        {
            var sceneType = typeof(T);
            if (_scene != sceneType)
            {
                _needChangeScene = true;
                _scene = sceneType;
            }
        }

        private static void ChangeScene()
        {
            _currentScene.UnloadContent();
            _currentScene = GameScenes[_scene];
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

        public static void LoadAssemble(string assemblyName)
        {
            var load = Assembly.Load(assemblyName).GetTypes().Where(x =>
                x.IsClass && x.IsSubclassOf(typeof(GameSceneBase))).ToArray();

            foreach (var type in load)
            {
                var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                var voidConstructor = constructors.FirstOrDefault(x => x.GetParameters().Length == 0);
                if (voidConstructor != null)
                    GameScenes.Add(type, (GameSceneBase) Activator.CreateInstance(type));
                else
                {
                    var firstConstructor = constructors.FirstOrDefault();
                    if (firstConstructor != null)
                    {
                        var parameters = firstConstructor.GetParameters();
                        var resolve = AutoFacFactory.TypesResolve(parameters.Select(x => x.ParameterType).ToArray());
                        GameScenes.Add(type, (GameSceneBase)firstConstructor.Invoke(resolve));
                    }
                }
            }
        }
    }
}