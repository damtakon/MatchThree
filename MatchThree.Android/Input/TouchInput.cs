using System.Linq;
using MatchThree.Core;
using MatchThree.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace MatchThree.Android.Input
{
    public class TouchInput : VectorInput
    {
        private TouchPanelState _state;
        private Vector2? _startPressed;
        private readonly GameWindow _window;

        public TouchInput(GameWindow window)
        {
            _window = window;
        }

        public override void Update(GameTime gameTime)
        {
            var currentState = TouchPanel.GetState(_window);
            var touchCollection = currentState.GetState();
            if (touchCollection.Count > 0)
            {
                var first = touchCollection.FirstOrDefault();
                var currentPosition = Vector2.Transform(first.Position, Global.ScaleMatrix);
                OnMove(currentPosition);
                if (first.State == TouchLocationState.Pressed)
                {
                    _startPressed ??= currentPosition;
                    OnPressedMove(_startPressed.Value, currentPosition);
                }

                if (first.State == TouchLocationState.Released)
                {
                    _startPressed = null;
                    var lastTouchCollection = _state.GetState();
                    if (lastTouchCollection.Count == 0 || lastTouchCollection.FirstOrDefault().State == TouchLocationState.Pressed)
                        OnPress(currentPosition);
                }

                _state = currentState;
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
        }
    }
}