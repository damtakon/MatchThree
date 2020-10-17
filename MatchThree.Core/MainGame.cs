using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core
{
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Action<ContentManager> _loadContent;
        private Action<GameWindow> _autoFacInit;
        private Texture2D _background;
        private Matrix _transformMatrix;

        /// <summary>
        /// Main game scene
        /// </summary>
        /// <param name="loadContent">Action for loading any instance if needed (can be null)</param>
        /// <param name="autoFacInit">Action for autofac initialize any instance if needed (can be null)</param>
        public MainGame(Action<ContentManager> loadContent = null, Action<GameWindow> autoFacInit = null)
        {
            //Main graphics settings
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
            };

            //_graphics = new GraphicsDeviceManager(this)
            //{
            //    PreferredBackBufferWidth = 1920,
            //    PreferredBackBufferHeight = 1080
            //};

            var scaleX = _graphics.PreferredBackBufferWidth / Global.VirtualWidth;
            var scaleY = _graphics.PreferredBackBufferHeight / Global.VirtualHeight;
            var scaleX2 = Global.VirtualWidth / _graphics.PreferredBackBufferWidth;
            var scaleY2 = Global.VirtualHeight / _graphics.PreferredBackBufferHeight;
            _transformMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);
            Global.ScaleMatrix = Matrix.CreateScale(scaleX2, scaleY2, 1.0f);
            Content.RootDirectory = "Content";
            Window.Title = "Simple Match Three for Game Forest";
            Window.Position = Point.Zero;
            Window.IsBorderless = true;
            Window.AllowAltF4 = false;
            _loadContent = loadContent;
            _autoFacInit = autoFacInit;
            
        }

        protected override void Initialize()
        {
            SceneManager.Init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _loadContent?.Invoke(Content);
            _loadContent = null;
            _autoFacInit?.Invoke(Window);
            _autoFacInit = null;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _background = Content.Load<Texture2D>(GameResource.BackgroundPath);
            SceneManager.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
            SceneManager.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            SceneManager.Update(gameTime);
            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);
            _spriteBatch.Begin(transformMatrix: _transformMatrix);
            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            SceneManager.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
