using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace Orchard
{
    /// <summary>
    /// Utility methodes to enable new functionality
    /// </summary>
    internal static class FruitTreeExtention
    {

        

        public static bool fertilize(this FruitTree tree)
        {
            if (tree.modData.ContainsKey("fertilizer") && tree.modData["fertilizer"] == "true")
            {
                StardewValley.Game1.showRedMessageUsingLoadString("Strings\\StringsFromCSFiles:TreeFertilizer2");
                tree.currentLocation.playSound("cancel");
                return false;
            }
            else if(tree.modData.ContainsKey("fertilizer"))
            {
                tree.modData["fertilizer"] = "true";
                tree.currentLocation.playSound("dirtyHit");
                return true;
            }
            else
            {
                tree.modData.Add("fertilizer", "true");
                tree.currentLocation.playSound("dirtyHit");
                return true;
            }

        }

        public static bool fertilizerFades(this FruitTree tree)
        {
            if (tree.modData.ContainsKey("fertilizer") && tree.growthStage.Equals(4))
            {
                tree.modData["fertilizer"] = "false";
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool isFertilized(this FruitTree tree)
        {
            if (tree.modData.ContainsKey("fertilizer"))
            {
                switch (tree.modData["fertilizer"])
                {
                    case "true":
                        return true;

                    case "false":
                        return false;

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool hasDroppedSapling(this FruitTree tree)
        {
            if (tree.modData.ContainsKey("sapling")){
                switch (tree.modData["sapling"])
                {
                    case "true":
                        return true;

                    case "false":
                        return false;

                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static bool addDroppedSapling(this FruitTree tree)
        {
            if (tree.modData.ContainsKey("sapling"))
            {
                tree.modData["sapling"] = "true";
                return true;
            }
            else
            {
                tree.modData.Add("sapling", "true");
                return true;
            }
        }

        public static bool refreshSapling(this FruitTree tree)
        {
            if (tree.modData.ContainsKey("sapling"))
            {
                tree.modData["sapling"] = "false";
                return true;
            }
            return false;
        }

        public static bool findCloseBeeHouse(this FruitTree tree)
        {
            Queue<Vector2> queue = new Queue<Vector2>();
            HashSet<Vector2> visited = new HashSet<Vector2>();

            queue.Enqueue(tree.currentTileLocation);
            visited.Add(tree.currentTileLocation);

            for(int i = 0; i < 80; i++)
            {
                if (queue.Count <= 0)
                {
                    break;
                }

                Vector2 current = queue.Dequeue();

                if (tree.currentLocation.isObjectAtTile((int)current.X, (int)current.Y) && tree.currentLocation.getObjectAt((int)current.X * 64, (int)current.Y * 64).Name.Equals("Bee House"))
                {
                    return true;
                }
                foreach (Vector2 nearby in Utility.getAdjacentTileLocations(current))
                {
                    if (!visited.Contains(nearby) && (int)Math.Abs(current.X - tree.currentTileLocation.X) + (int)Math.Abs(current.Y - tree.currentTileLocation.Y) < 5)
                    {
                        queue.Enqueue(nearby);
                        visited.Add(nearby);
                    }
                }
            }

            return false;
        }

        
    }
}
