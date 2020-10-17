using MatchThree.Core;
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
        private readonly Rectangle _source;
        private readonly Vector2 _scale;

        public MouseInput(Texture2D texture2D, GameWindow window)
        {
            _texture2D = texture2D;
            // Fix MonoGame Mouse always 0,0 position TODO: Find normal fix c:
            _window = window;
            _source = new Rectangle(0, 0, _texture2D.Width, _texture2D.Height);
            _scale = new Vector2(26f / _texture2D.Width, 26f / _texture2D.Height);
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

            _state = currentState;
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(_texture2D, _position, _source, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}