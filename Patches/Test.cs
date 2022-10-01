using Common;
using HarmonyLib;
using Netcode;
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
    internal class Test
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var helper = new ILHelper(instructions);

            var result = helper.findNextString("fruitsOnTree").findNextString("fruitsOnTree").advance().replace(new CodeInstruction(OpCodes.Ldc_I4_1)).flush();


            return result;

        }
    }
}
