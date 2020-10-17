using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Shared.Input;

namespace MatchThree.Desktop.DirectX.Input
{
    public class MouseInput : VectorInput
    {
        private readonly Texture2D _texture2D;
        private MouseState _state;
        private Vector2? _startPressed;
        private Vector2 _position;

        public MouseInput(Texture2D texture2D)
        {
            _texture2D = texture2D;
        }

        public override void Update(GameTime gameTime)
        {
            var currentState = Mouse.GetState();
            var currentPosition = currentState.Position.ToVector2();
            if (!_position.Equals(currentPosition))
            {
                OnMove(currentPosition);
                if (currentState.LeftButton == ButtonState.Pressed)
                {
                    _startPressed ??= _position;
                    OnPressedMove(_startPressed.Value, currentPosition);
                }
            }

            _position = currentPosition;
            if (currentState.LeftButton == ButtonState.Released)
            {
                _startPressed = null;
                if (_state.LeftButton == ButtonState.Pressed)
                    OnPress(_position);
            }


            _state = currentState;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture2D, _position, Color.White);
        }
    }
}