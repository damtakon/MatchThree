using MatchThree.Core;
using MatchThree.Core.Extension;
using MatchThree.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MatchThree.Desktop.DirectX.Input
{
    public class MouseInput : VectorInput
    {
        private readonly Texture2D _texture2D;
        private MouseState _state;
        private Vector2? _startPressed;
        private Vector2 _position;
        private readonly GameWindow _window;
        private Rectangle _box;

        public MouseInput(Texture2D texture2D, GameWindow window)
        {
            _texture2D = texture2D;
            // Fix MonoGame Mouse always 0,0 position TODO: Find normal fix c:
            _window = window;
            _box = new Rectangle(0, 0, 40, 40);
        }

        public override void Update(GameTime gameTime)
        {
            var currentState = Mouse.GetState(_window);

            //Fix any display size scale
            
            var currentPosition = Vector2.Transform(currentState.Position.ToVector2(), Global.ScaleMatrix);

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
            _box.SetXY(_position);
            _state = currentState;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture2D, _box, Color.White);
        }
    }
}