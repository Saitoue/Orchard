﻿using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Object = StardewValley.Object;


namespace Orchard
{
    internal class FruitTreePatches
    {

        /// <summary>
        /// modifies growth of Fruit Trees
        /// </summary>
        [HarmonyPatch(typeof(FruitTree), nameof(FruitTree.dayUpdate))]
        internal class FruitTreeGrow
        {

            [HarmonyPrefix]
            protected static void Prefix(FruitTree __instance, GameLocation environment)
            {
                if (!FruitTree.IsGrowthBlocked(__instance.currentTileLocation,__instance.currentLocation) && __instance.isFertilized() && __instance.daysUntilMature.Value >= 0 && !Game1.IsWinter)
                {
                    __instance.daysUntilMature.Value--;
                }

                if (__instance.daysUntilMature.Value <= 0 && __instance.findCloseBeeHouse() && !Game1.IsWinter && !__instance.GreenHouseTree)
                {
                    __instance.daysUntilMature.Value--;
                }

                if (!__instance.stump.Value && __instance.growthStage.Value == 4 && !Game1.IsWinter && (__instance.IsInSeasonHere(__instance.currentLocation) || __instance.isFertilized()))
                {
                    Random rand = new Random();

                    ///grows additional fruit if fertilized
                    if (__instance.isFertilized())
                    {
                        __instance.fruitsOnTree.Value = Math.Min(3, __instance.fruitsOnTree.Value + 1);
                    }
                    ///chance to grow additional fruit based on foraging level
                    if (rand.Next(1, 11) <= Game1.player.GetSkillLevel(Farmer.foragingSkill))
                    {
                        __instance.fruitsOnTree.Value = Math.Min(3, __instance.fruitsOnTree.Value + 1);
                    }
                }
            }

        }



        /// <summary>
        /// allows fertilization , adds chance for sapling drop
        /// </summary>
        [HarmonyPatch(typeof(FruitTree), nameof(FruitTree.shake))]
        internal class FruitTreeShake
        {
            [HarmonyPrefix]
            /// saves number of fruits before shake
            protected static void Prefix(FruitTree __instance, ref int __state)
            {

                __state = __instance.fruitsOnTree.Value;

                /// if the current item is tree fertilizer runs fertilize method, removes one item if necessary
                if (Game1.player.CurrentItem is Object && Game1.player.CurrentItem.ParentSheetIndex == 805)
                {
                    if (__instance.fertilize() == true)
                    {
                        Game1.player.removeItemsFromInventory(Game1.player.CurrentItem.ParentSheetIndex, 1);
                    };
                }
            }

            [HarmonyPostfix]
            /// adds chance to drop sapling after shaking tree
            protected static void Postfix(FruitTree __instance, Vector2 tileLocation, int __state)
            {
                Random rand = new Random();
                
                int index = SaplingIndex.getSaplingIndex(__instance.indexOfFruit.Value);

                
                if (__state > 0 && !__instance.hasDroppedSapling())
                {
                    bool drop = false;
                    for (int i = __state; i > 0; i--)
                    {
                        if ((rand.Next(1, 101) + Game1.player.GetSkillLevel(Farmer.foragingSkill)) > 100) drop = true;
                    }

                    if (drop == true)
                    {
                        __instance.addDroppedSapling();
                        Debris sapling = new Debris(new StardewValley.Object(index, 1, false, -1, 0), tileLocation * 64f);
                        __instance.currentLocation.debris.Add(sapling);
                    }
                }
            }
        }


        /// <summary>
        /// removes fertilizer at the end of the Season and reenables the tree to drop a sapling
        /// </summary>
        [HarmonyPatch(typeof(FruitTree), nameof(FruitTree.seasonUpdate))]
        internal class FruitTreeSeasonUpdate
        {

            [HarmonyPrefix]
            protected static void Prefix(FruitTree __instance, bool onLoad)
            {
                if (!onLoad)
                {
                    __instance.fertilizerFades();
                    __instance.refreshSapling();
                }
            }
        }
    }
}