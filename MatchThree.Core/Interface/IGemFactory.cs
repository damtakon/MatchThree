using MatchThree.Core.MatchThree;
using Microsoft.Xna.Framework;

namespace MatchThree.Core.Interface
{
    public interface IGemFactory
    {
        /// <summary>
        /// Creation of a gem by a factory
        /// </summary>
        /// <param name="position">Coordinate where the gem should be at the end of the path</param>
        /// <returns></returns>
        Gem Create(Rectangle position);
    }
}