﻿using System;
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
        private Action _autoFacInit;

        /// <summary>
        /// Main game scene
        /// </summary>
        /// <param name="loadContent">Action for loading any instance if needed (can be null)</param>
        /// <param name="autoFacInit">Action for autofac initialize any instance if needed (can be null)</param>
        public MainGame(Action<ContentManager> loadContent = null, Action autoFacInit = null)
        {
            //Main graphics settings
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920, 
                PreferredBackBufferHeight = 1080
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Simple Match Three for Game Forest";
            Window.IsBorderless = false;
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
            _autoFacInit?.Invoke();
            _autoFacInit = null;
            _spriteBatch = new SpriteBatch(GraphicsDevice);
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
            SceneManager.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);
        }
    }
}