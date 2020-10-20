using System.Collections.Generic;
using System.Linq;

namespace MatchThree.Core.Extension
{
    public static class CollectionExtension
    {
        /// <summary>
        /// Adding elements that are unique by reference
        /// </summary>
        /// <typeparam name="T">elements type</typeparam>
        /// <param name="mainCollection">The main collection where elements will be added</param>
        /// <param name="elementsForAdd">Collection of items to add</param>
        public static void AddUnique<T>(this ICollection<T> mainCollection, IEnumerable<T> elementsForAdd)
        {
            if (elementsForAdd != null)
            {
                foreach (var element in elementsForAdd)
                {
                    if (!mainCollection.Any(x => ReferenceEquals(x, element)))
                        mainCollection.Add(element);
                }
            }

        }
    }
}