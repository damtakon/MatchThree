using System;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    public class Gem : IUpdateDrawable, IEquatable<Gem>
    {
        protected readonly Texture2D Texture2D;
        protected Rectangle ClickBox;
        protected Vector2 Position;
        protected Rectangle EndPosition;
        protected int Speed = 1000;

        public Gem(Texture2D texture2D, Rectangle startPosition, Rectangle endPosition)
        {
            Texture2D = texture2D;
            EndPosition = endPosition;
            ClickBox = startPosition;
            Position = new Vector2(startPosition.X, startPosition.Y);
        }

        public void Update(GameTime gameTime)
        {
            if (Math.Abs(Position.Y - EndPosition.Y) > 0.1)
            {
                var h = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;
                if (EndPosition.Y < Position.Y + h)
                    Position.Y = EndPosition.Y;
                else
                    Position.Y += h;
                ClickBox.Y = (int)Position.Y;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture2D, ClickBox, Color.White);
        }

        public bool Equals(Gem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Texture2D, other.Texture2D);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Gem) obj);
        }

        public override int GetHashCode()
        {
            return (Texture2D != null ? Texture2D.GetHashCode() : 0);
        }
    }
}