using System;
using MatchThree.Core.Control;
using MatchThree.Core.Interface;
using MatchThree.Core.MatchThree.Event;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    public sealed class Score : IUpdateDrawable
    {
        public static double LastScore;
        private readonly TextWrapper _textWrapper;
        private double _value;
        private double _addValue;
        private double _stepValue;


        public Score(SpriteFont spriteFont, Rectangle container)
        {
            _textWrapper = new TextWrapper(spriteFont, container, "Score: 0");
            LastScore = 0;
        }

        public void GemDestroy(object sender, EventArgs e)
        {
            AddPoints(3);
        }

        public void LineDestroy(object sender, LineDestroyEventArgs args)
        {
            AddPoints((int) Math.Pow(3, args.Line.Count));
        }

        private void AddPoints(int points)
        {
            _addValue += points;
            _stepValue = _addValue * 0.5;
            LastScore += points;
        }

        public void Update(GameTime gameTime)
        {
            if (_addValue > 0)
            {
                var step = _stepValue * gameTime.ElapsedGameTime.TotalSeconds;
                if (step > _addValue)
                    step = _addValue;

                _value += step;
                _addValue -= step;

                var text = $"Score: {(int) _value}";
                if (!_textWrapper.Text.Equals(text))
                    _textWrapper.Text = text;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _textWrapper.Draw(spriteBatch, gameTime);
        }
    }
}