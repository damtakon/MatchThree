using System;
using System.Collections.Generic;
using System.Linq;
using MatchThree.Core.MatchThree;
using MatchThree.Core.MatchThree.Event;

namespace MatchThree.Core.Extension
{
    public static class GemExtension
    {
        /// <summary>
        /// Gem request on the left
        /// </summary>
        /// <param name="gems">Gem grid on the board</param>
        /// <param name="gem">Which gem to watch</param>
        /// <param name="number">How many elements</param>
        /// <returns>Could he find the gem and the gem itself</returns>
        public static (bool, Gem) Left(this Gem[,] gems, Gem gem, int number)
        {
            return Right(gems, gem, -number);
        }

        /// <summary>
        /// Gem request on the right
        /// </summary>
        /// <param name="gems">Gem grid on the board</param>
        /// <param name="gem">Which gem to watch</param>
        /// <param name="number">How many elements</param>
        /// <returns>Could he find the gem and the gem itself</returns>
        public static (bool, Gem) Right(this Gem[,] gems, Gem gem, int number)
        {
            var columns = gems.GetLength(0);
            if (gem.XPosition + number >= columns || gem.XPosition + number < 0)
                return (false, null);
            var nextGem = gems[gem.XPosition + number, gem.YPosition];
            return (gem.Equals(nextGem), nextGem);
        }

        /// <summary>
        /// Gem request on the top
        /// </summary>
        /// <param name="gems">Gem grid on the board</param>
        /// <param name="gem">Which gem to watch</param>
        /// <param name="number">How many elements</param>
        /// <returns>Could he find the gem and the gem itself</returns>
        public static (bool, Gem) Top(this Gem[,] gems, Gem gem, int number)
        {
            return Bottom(gems, gem, -number);
        }

        /// <summary>
        /// Gem request on the bottom
        /// </summary>
        /// <param name="gems">Gem grid on the board</param>
        /// <param name="gem">Which gem to watch</param>
        /// <param name="number">How many elements</param>
        /// <returns>Could he find the gem and the gem itself</returns>
        public static (bool, Gem) Bottom(this Gem[,] gems, Gem gem, int number)
        {
            var lines = gems.GetLength(1);
            if (gem.YPosition + number >= lines || gem.YPosition + number < 0)
                return (false, null);
            var nextGem = gems[gem.XPosition, gem.YPosition + number];
            return (gem.Equals(nextGem), nextGem);
        }


        /// <summary>
        /// Recalculate matches per gem grid
        /// </summary>
        /// <param name="gems">Gem grid on the board</param>
        /// <param name="recalculate">List of gems for which you need to recalculate</param>
        /// <returns>List of events for destroying lines</returns>
        public static List<LineDestroyEventArgs> FindMatches(this Gem[,] gems, List<Gem> recalculate)
        {
            var lists = new List<LineDestroyEventArgs>();
            foreach (var gem in recalculate)
            {
                var find = lists.FirstOrDefault(events => events.Line.Any(x => ReferenceEquals(x, gem)));
                var matchLine = find?.Line ?? new List<Gem>();

                var matchLineHorizontal = FindMatchLine(gem, gems.Right, gems.Left);
                var matchLineVertical = FindMatchLine(gem, gems.Bottom, gems.Top);

                matchLine.AddUnique(matchLineHorizontal);
                matchLine.AddUnique(matchLineVertical);

                if (find == null && matchLine.Count > 0)
                    lists.Add(new LineDestroyEventArgs(matchLine, gem));
            }

            return lists;
        }

        /// <summary>
        /// Search for lines matching
        /// </summary>
        /// <param name="gem">Which gem to watch</param>
        /// <param name="methodOne">First method to search</param>
        /// <param name="methodTwo">Second method to search</param>
        /// <returns>List of gems if found another null</returns>
        private static List<Gem> FindMatchLine(Gem gem, Func<Gem, int, (bool, Gem)> methodOne, Func<Gem, int, (bool, Gem)> methodTwo)
        {
            var line = new List<Gem> { gem };
            Find(gem, line, methodOne, 1);
            Find(gem, line, methodTwo, 1);

            if (line.Count >= 3)
                return line;

            return null;
        }

        /// <summary>
        /// Recursive method for finding identical gems
        /// </summary>
        /// <param name="gem">Which gem to watch</param>
        /// <param name="line">List of found identical gems</param>
        /// <param name="method">Method to search</param>
        /// <param name="number">Bias</param>
        private static void Find(Gem gem, ICollection<Gem> line, Func<Gem, int, (bool, Gem)> method, int number)
        {
            var (result, nextGem) = method.Invoke(gem, number);

            if (result)
            {
                line.Add(nextGem);
                Find(gem, line, method, ++number);
            }
        }
    }
}