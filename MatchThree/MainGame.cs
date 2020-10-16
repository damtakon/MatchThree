using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree
{
    public class MainGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //Main graphics settings
            IsMouseVisible = true;
            Window.Title = "Simple Match Three for Game Forest";
            Window.Position = new Point(0, 0);
            Window.IsBorderless = false;
            Window.AllowAltF4 = false;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            if (Control.FromHandle(Window.Handle) is Form f)
                f.FormClosing += FormClosing;
            SceneManager.Init();
            base.Initialize();
        }

        private void FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Exit?","", MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
        }

        protected override void LoadContent()
        {
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
