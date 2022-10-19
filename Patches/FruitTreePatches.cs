using HarmonyLib;
using Microsoft.Xna.Framework;
using Orchard.Framework;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using Object = StardewValley.Object;


namespace Orchard.Patches
{
    internal class FruitTreePatches
    {

        /// <summary>
        /// modifies growth of Fruit Trees
        /// </summary>
        //[harmonypatch(typeof(fruittree), nameof(fruittree.dayupdate))]
        //internal class fruittreegrow
        //{

        //    [harmonyprefix]
        //    protected static void prefix(fruittree __instance, gamelocation environment)
        //    {
        //        if (!fruittree.isgrowthblocked(__instance.currenttilelocation, __instance.currentlocation) && __instance.isfertilized() && __instance.daysuntilmature.value >= 0 && !game1.iswinter)
        //        {
        //            __instance.daysuntilmature.value--;
                    
        //        }

        //        if (__instance.daysuntilmature.value <= 0 && __instance.findclosebeehouse() && !game1.iswinter && !__instance.greenhousetree)
        //        {
        //            __instance.daysuntilmature.value--;
        //        }

        //        if (!__instance.stump.value && __instance.growthstage.value == 4 && !game1.iswinter && (__instance.isinseasonhere(__instance.currentlocation) || __instance.isfertilized()))
        //        {
        //            random rand = new random();

        //            ///grows additional fruit if fertilized
        //            if ((modentry.config.extrafruitfertilizer || (modentry.config.outofseasontrees && !__instance.isinseasonhere(__instance.currentlocation)) && __instance.isfertilized()))
        //            {
        //                __instance.fruitsontree.value = math.min(3, __instance.fruitsontree.value + 1);
        //            }
        //            ///chance to grow additional fruit based on foraging level
        //            if (modentry.config.extrafruitlevel && rand.next(1, 101) <= game1.player.getskilllevel(farmer.foragingskill) * modentry.config.fruitperlevel)
        //            {
        //                __instance.fruitsontree.value = math.min(3, __instance.fruitsontree.value + 1);
        //            }
        //        }
        //    }

        //}
        


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
            /// shaking gives foraging exp
            protected static void Postfix(FruitTree __instance, Vector2 tileLocation, int __state)
            {

                if (ModEntry.Config.expFromTrees)
                {
                    Game1.player.gainExperience(Farmer.foragingSkill, __state * 3);
                }

                if (ModEntry.Config.dropSappling)
                {
                    Random rand = new Random();

                    if ((__state > 0 && !__instance.hasDroppedSapling()))
                    {
                        bool drop = false;
                        for (int i = __state; i > 0; i--)
                        {
                            if (rand.Next(1, 101) + Game1.player.GetSkillLevel(Farmer.foragingSkill) > 100) drop = true;
                        }

                        if (drop == true)
                        {
                            __instance.addDroppedSapling();
                            Debris sapling = new Debris(new Object(__instance.getSapling(), 1, false, -1, 0), tileLocation * 64f);
                            __instance.currentLocation.debris.Add(sapling);
                        }
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
