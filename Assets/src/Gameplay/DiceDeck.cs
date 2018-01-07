using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Gameplay
{
	public class DiceDeck
	{
        int size;
        int threshold;

        List<int> deck = new List<int>();

        /// <summary>
        /// Create a new dicedeck.
        /// </summary>
        /// <param name="size">The maximum value of the deck, needs to be at least 2</param>
        /// <param name="threshold">lower number means less random. The number of card left in the deck before we shuffle in a new deck into it.</param>
		public DiceDeck(int size, int threshold = 0)
        {
            if (size < 2) throw new Exception("Size must be at least 2");
            if (threshold < 0) threshold = 0;

            this.size = size;
            this.threshold = threshold;

            fillDeck();
        }

        /// <summary>
        /// Returns the next int, value between 1 and the size.
        /// </summary>
        /// <returns>A semirandom value between 1 and size (both inclusive)</returns>
        public int Next()
        {
            int r = deck[0];
            deck.RemoveAt(0);

            if (deck.Count <= threshold)
                fillDeck();

            return r;
        }

        /// <summary>
        /// Gets a float value beteween 0 (inlusive) and 1 (exlusive)
        /// </summary>
        /// <returns></returns>
        public float NextFloat()
        {
            return (float)(Next() - 1) / size;
        }

        /// <summary>
        /// Push in a new set of numbers into the deck and shuffle it.
        /// </summary>
        private void fillDeck()
        {
            for(int i = 1; i <= size; i++)
            {
                deck.Insert(UnityEngine.Random.Range(0, deck.Count+1), i);
            }
        }
	}
}
