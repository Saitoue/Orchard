using Common;
using HarmonyLib;
using Netcode;
using Orchard.Framework;
using StardewValley.Menus;
using StardewValley.TerrainFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Orchard.Patches
{

    [HarmonyPatch(typeof(FruitTree))]
    [HarmonyPatch(nameof(FruitTree.shake))]
    

    [HarmonyPatch(nameof(FruitTree.dayUpdate))]
    internal class moreFruit
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var helper = new ILHelper(instructions);


            var result = helper.findNextString("fruitsOnTree").findNextString("fruitsOnTree").findNextString("fruitsOnTree").advance(2).Remove().Insert(new CodeInstruction(OpCodes.Ldarg_0))
                .Insert(new CodeInstruction(OpCodes.Call , AccessTools.Method(typeof(FruitTreeExtention), nameof(FruitTreeExtention.getAdditionalFruits)))).flush();

            return result;
        }
    }
}
