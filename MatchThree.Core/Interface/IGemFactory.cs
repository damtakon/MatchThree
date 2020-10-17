using MatchThree.Core.MatchThree;
using Microsoft.Xna.Framework;

namespace MatchThree.Core.Interface
{
    public interface IGemFactory
    {
        Gem Create(Rectangle position);
    }
}