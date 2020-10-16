using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Shared
{
    public class MainGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Action<ContentManager> _loadContent;
        private Action _autoFacInit;

        /// <summary>
        /// Main game scene
        /// </summary>
        /// <param name="loadContent">Action for loading any instance if needed (can be null)</param>
        /// <param name="autoFacInit">Action for autofac initialize any instance if needed (can be null)</param>
        public MainGame(Action<ContentManager> loadContent = null, Action autoFacInit = null)
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _loadContent = loadContent;
            _autoFacInit = autoFacInit;
        }

        protected override void Initialize()
        {
            //Main graphics settings
            Window.Title = "Simple Match Three for Game Forest";
            Window.Position = new Point(0, 0);
            Window.IsBorderless = true;
            Window.AllowAltF4 = false;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            SceneManager.Init();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _loadContent?.Invoke(Content);
            _loadContent = null;
            _autoFacInit?.Invoke();
            _autoFacInit = null;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.LoadContent();
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
            SceneManager.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }
}
