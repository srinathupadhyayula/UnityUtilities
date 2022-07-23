using System.Collections.Generic;
using Random = System.Random;

namespace Utilities.Extensions
{
    public static partial class ListExtensions
    {
        /// <summary>
        /// Return a random element from the list.
        /// </summary>
        /// <typeparam name="TListType">The type of list or the type of elements in the list</typeparam>
        /// <param name="listToGetRandomElementFrom">The current <see cref="IList">container</see> to get the
        /// random element from.</param>
        /// <returns>A random element form the list </returns>
        /// <remarks>
        /// <para>This method is heavily based on a similar method in <see cref="https://github.com/DapperDino">DapperDino</see>'s
        /// <see cref="https://github.com/DapperDino/Dapper-Tools">Dapper-Tools</see> package.</para>
        /// </remarks>
        public static TListType GetRandomElement<TListType>(this IList<TListType> listToGetRandomElementFrom)
        {
            // Pick a random item from the list and return it
            return listToGetRandomElementFrom[UnityEngine.Random.Range(0, listToGetRandomElementFrom.Count)];
        }

        /// <summary>
        /// Return a random element from the list and set the out variable <b><paramref name="elementIndex"/></b>
        /// to the index of the returned element.
        /// </summary>
        /// <typeparam name="TListType">The type of list or the type of elements in the list</typeparam>
        /// <param name="listToGetRandomElementFrom">The current <see cref="IList">container</see> to get the
        /// random element from.</param>
        /// <param name="elementIndex">This parameter is set to the index of the random element</param>
        /// <returns>A random element from the list.</returns>
        public static TListType GetRandomElement<TListType>(this IList<TListType> listToGetRandomElementFrom, out int elementIndex)
        {
            elementIndex = UnityEngine.Random.Range(0, listToGetRandomElementFrom.Count);
            return listToGetRandomElementFrom[elementIndex];
        }

        /// <summary>
        /// Remove a random item from the list and returns it.
        /// </summary>
        /// <typeparam name="TListType">The type of list or the type of elements in the list</typeparam>
        /// <param name="listToRemoveRandomElementFrom">The current <see cref="IList">container</see> to
        /// remove random element from.</param>
        /// <returns>Returns the removed element.</returns>
        /// <remarks>
        /// <para>This method is heavily based on a similar method in <see cref="https://github.com/DapperDino">DapperDino</see>'s
        /// <see cref="https://github.com/DapperDino/Dapper-Tools">Dapper-Tools</see> package.</para>
        /// </remarks>
        public static TListType RemoveRandomElement<TListType>(this IList<TListType> listToRemoveRandomElementFrom)
        {
            // Pick a random item from the list and get it's index
            TListType item = listToRemoveRandomElementFrom.GetRandomElement(out int indexToRemoveAt);

            // Remove the item from the list
            listToRemoveRandomElementFrom.RemoveAt(indexToRemoveAt);

            // Return the item that we removed
            return item;
        }

        /// <summary>
        /// Shuffles the elements in the list
        /// </summary>
        /// <typeparam name="TListType">The type of list or the type of elements in the list</typeparam>
        /// <param name="listToShuffle">The current <see cref="IList">container</see> to shuffle.</param>
        /// <remarks>
        /// <para>This method is heavily based on a similar method in <see cref="https://github.com/DapperDino">DapperDino</see>'s
        /// <see cref="https://github.com/DapperDino/Dapper-Tools">Dapper-Tools</see> package.</para>
        /// <br/><b>Algorithm used::<i cref="https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle">Fisher-Yates Shuffle</i></b>
        /// <br/><i>TODO: Modify the method to accept options to choose which shuffle algorithm should be used.</i>
        /// </remarks>
        public static void Shuffle<TListType>(this IList<TListType> listToShuffle)
        {
            var rng = new Random(); // Consider updating the random when a better Random method is implemented

            for (int i = listToShuffle.Count - 1; i > 1; i--)
            {
                int k     = rng.Next(i);
                (listToShuffle[k], listToShuffle[i]) = (listToShuffle[i], listToShuffle[k]);
            }
        }
    }
}