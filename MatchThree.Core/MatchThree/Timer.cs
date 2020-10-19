using System;
using MatchThree.Core.Control;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    public sealed class Timer : IUpdateDrawable
    {
        private readonly TextWrapper _textWrapper;
        private readonly Action _onTimeExpired;
        private double _secondDouble;
        private int _second;

        public Timer(SpriteFont spriteFont, Rectangle container, Action onTimeExpired, int seconds)
        {
            _textWrapper = new TextWrapper(spriteFont, container, $"Time: {seconds}");
            _onTimeExpired = onTimeExpired;
            _second = seconds;
            _secondDouble = seconds;
        }

        public void Update(GameTime gameTime)
        {
            _secondDouble -= gameTime.ElapsedGameTime.TotalSeconds;
            _second = (int)_secondDouble;
            var text = $"Time: {_second}";
            if (!_textWrapper.Text.Equals(text))
                _textWrapper.Text = text;
            if(_second <= 0)
                _onTimeExpired.Invoke();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            _textWrapper.Draw(spriteBatch, gameTime);
        }
    }
}