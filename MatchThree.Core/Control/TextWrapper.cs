using System;
using System.Text;
using MatchThree.Core.Extension;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.Control
{
    public class TextWrapper : IUpdateDrawable
    {
        protected SpriteFont Font;
        protected Vector2 Position;
        protected Color TextColor;
        protected Rectangle Container;
        protected string TextForDraw;
        protected string SettedText;

        public TextWrapper(SpriteFont font, Rectangle container, string text = "", Color? textColor = null)
        {
            Font = font;
            Container = container;
            TextColor = textColor ?? Color.White;
            Position.SetXY(Container);
            Text = text;
        }

        public string Text
        {
            get => SettedText;
            set
            {
                SettedText = value;
                TextForDraw = TextWrap(value);
            }
        }

        private string TextWrap(string text)
        {
            if (Font.MeasureString(Text).X <= Container.Width)
                return text;

            var one = Font.MeasureString("2").X;
            var maxCharsInLine = (int) (Container.Width / one);
            var split = text.Split(' ');
            var lineLen = 0;
            var newText = new StringBuilder();
            foreach (var line in split)
            {
                if (lineLen == 0)
                {
                    newText.Append(line);
                    lineLen = line.Length;
                }
                else if (lineLen + line.Length > maxCharsInLine)
                {
                    newText.Append(Environment.NewLine);
                    newText.Append(line);
                    lineLen = line.Length;
                }
                else
                {
                    newText.Append(' ');
                    newText.Append(line);
                    lineLen += line.Length;
                }
            }

            return newText.ToString();
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawString(Font, TextForDraw, Position, TextColor);
        }
    }
}