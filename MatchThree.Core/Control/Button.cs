using System;
using MatchThree.Core.Input;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Control
{
    public class Button : IUpdateDrawable
    {
        protected SpriteFont Font;
        protected string Text;
        protected Vector2 Position;
        protected Rectangle ClickBox;
        protected Action ClickAction;
        protected VectorInput VectorInput;
        protected Color DefaultColor;
        protected Color HoverColor;
        protected Color CurrentColor;
        protected bool IsSubscribed;

        public Button(SpriteFont font, VectorInput vectorInput, Action clickAction, string text, Vector2 position, Color? defaultColor = null, Color? hoverColor = null)
        {
            Font = font;
            ClickAction = clickAction;
            VectorInput = vectorInput;
            Position = position;
            CurrentColor = DefaultColor;
            DefaultColor = defaultColor ?? Color.White;
            HoverColor = hoverColor ?? Color.Red;
            TextRefresh(text);
            Subscribe();
        }

        protected void TextRefresh(string text)
        {
            Text = text;
            var measureString = Font.MeasureString(Text);
            ClickBox = new Rectangle((int)Position.X, (int)Position.Y, (int)measureString.X, (int)measureString.Y);
        }

        protected virtual void VectorInputOnPress(Vector2 position)
        {
            if (ClickBox.Intersects(new Rectangle((int) position.X, (int) position.Y, 1, 1)))
                ClickAction.Invoke();
        }

        public void Subscribe()
        {
            if (!IsSubscribed)
            {
                VectorInput.Press += VectorInputOnPress;
                VectorInput.Move += VectorInputOnMove;
                IsSubscribed = true;
            }
        }

        private void VectorInputOnMove(Vector2 position)
        {
            if (ClickBox.Intersects(new Rectangle((int) position.X, (int) position.Y, 1, 1)))
                CurrentColor = HoverColor;
            else
                CurrentColor = DefaultColor;
        }

        public void UnSubscribe()
        {
            if (IsSubscribed)
            {
                VectorInput.Press -= VectorInputOnPress;
                IsSubscribed = false;
            }
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Font, Text, Position, CurrentColor);
        }
    }
}