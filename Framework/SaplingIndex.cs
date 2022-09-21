using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchard.Framework
{
    internal class SaplingIndex
    {

        /// <summary>
        /// returns index of saplings based on index of the fruit on the tree;
        /// </summary>
        /// <param name="fruitIndex"></param>
        /// <returns></returns>
        public static int getSaplingIndex(int fruitIndex)
        {

            switch (fruitIndex)
            {
                ///apricot
                case 634:
                    return 629;

                ///orange
                case 635:
                    return 630;

                ///peach
                case 636:
                    return 631;

                ///pomegranat
                case 637:
                    return 632;

                ///cherry
                case 638:
                    return 628;

                ///apple
                case 613:
                    return 633;

                ///mango
                case 834:
                    return 835;

                ///banana
                case 91:
                    return 69;

                default:
                    return fruitIndex;

            }
        }
    }
}
