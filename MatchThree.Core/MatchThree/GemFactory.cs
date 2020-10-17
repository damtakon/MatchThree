using System;
using System.Linq.Expressions;
using MatchThree.Core.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MatchThree.Core.MatchThree
{
    public sealed class RandomGemFactory : IGemFactory
    {
        private readonly Texture2D[] _texture2D;
        private readonly Random _random;

        public RandomGemFactory(params Texture2D[] texture2D)
        {
            _texture2D = texture2D;
            _random = new Random();
        }

        public Gem Create(Rectangle position)
        {
            var startPosition = new Rectangle(position.X, 0, position.Width, position.Height);
            return new Gem(_texture2D[_random.Next(_texture2D.Length)], startPosition, position);
        }
    }
}