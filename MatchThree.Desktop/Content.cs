using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Desktop
{
    public static class Content
    {
        public static Texture2D Mouse;

        public static void Load(ContentManager contentManager)
        {
            var game = new Game {Content = {RootDirectory = "Content"}};
            
            Mouse = contentManager.Load<Texture2D>(@"Image\cursor");
        }
    }
}