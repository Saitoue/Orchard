using System;
using System.Reflection;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TerrainFeatures;
using StardewValley.Tools;
using Object = StardewValley.Object;

namespace Orchard
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {

        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            var harmony = new Harmony(ModManifest.UniqueID);

            harmony.PatchAll(Assembly.GetExecutingAssembly());

            helper.Events.Input.ButtonPressed += this.OnButtonPressed;

            helper.Events.Content.AssetRequested += this.OnAssetRequested;

    }

        /// <summary>
        /// changes description if tree fertilizer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/ObjectInformation"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<int, string>().Data;

                    string[] fields = data[805].Split('/');
                    fields[5] = "Sprinkle on a wild tree to ensure rapid growth, even in winter. Now also works on fruit trees. ";

                    data[805] = string.Join("/",fields);
                });
            }
        }

#if DEBUG
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {

            
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            if ( e.Button.Equals(SButton.K))
            {
                this.Monitor.Log($"{Game1.player.Name} Started Tree count", LogLevel.Debug);

                foreach(TerrainFeature t in Game1.player.currentLocation.terrainFeatures.Values)
                {
                    if(t is FruitTree)
                    {
                        this.Monitor.Log($"counting Tree", LogLevel.Debug);
                        if (t.modData.ContainsKey("fertilizer"))
                        {
                            this.Monitor.Log($"Status :" + t.modData["fertilizer"] + "  X  " + t.currentTileLocation.X + "  Y  " + t.currentTileLocation.Y  + (t as FruitTree).indexOfFruit, LogLevel.Debug);
                        }
                        else
                        {
                            this.Monitor.Log($"coud not find key", LogLevel.Debug);
                        }
                        
                    }
                }
                int num = 1;
                foreach(Object o in Game1.player.currentLocation.Objects.Values)
                {

                    this.Monitor.Log($"object :" + num + "  " + o.Name, LogLevel.Debug);
                    num++;
                }
#endif
            }
        }
    }
}