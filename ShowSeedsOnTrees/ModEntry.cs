using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.TerrainFeatures;
using xTile.Dimensions;
using static StardewValley.Game1;
using Object = StardewValley.Object;

namespace ShowSeedsOnTrees
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private IMonitor monitor;
        Dictionary<int, Object> seedObjects = new Dictionary<int, Object>();
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            seedObjects.Add(309, new Object(new Vector2(0, 0), 309, 1));
            seedObjects.Add(310, new Object(new Vector2(0, 0), 310, 1));
            seedObjects.Add(311, new Object(new Vector2(0, 0), 311, 1));
            seedObjects.Add(88, new Object(new Vector2(0, 0), 88, 1));

           //helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Display.RenderedWorld += this.DrawSeeds;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // print button presses to the console window
            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.");
        }

        private void DrawSeeds(object sender, RenderedWorldEventArgs args)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            SpriteBatch b = args.SpriteBatch;
            foreach (var kv in player.currentLocation.terrainFeatures.Pairs)
            {
                if (kv.Value is Tree tree)
                {
                    //Monitor.Log("Found a tree");
                    int doDraw = -1;

                    //if (terrainFeature.GetType().IsInstanceOfType(new Tree()))
                    if (tree.hasSeed.Value && !tree.stump.Value && (IsMultiplayer || player.ForagingLevel > 0))
                    {
                        Monitor.Log("found a valid tree");
                        /*
                          bushyTree = 1; oak
                          leafyTree = 2; maple
                          pineTree = 3; pine
                          winterTree1 = 4; oak
                          winterTree2 = 5; maplewwww
                          palmTree = 6; palm
                         */
                        int type = tree.treeType.Value;
                        
                        switch (type)
                        {
                            case 1:
                            case 4:
                                doDraw = 309;
                                break;
                            case 2:
                            case 5:
                                doDraw = 310;
                                break;
                            case 3:
                                doDraw = 311;
                                break;
                            case 6:
                                doDraw = 88;
                                break;
                        }

                        var drawObject = seedObjects[doDraw];
                        Monitor.Log("drawing object " + doDraw);
                        drawObject.drawInMenu(b, Game1.GlobalToLocal(Game1.viewport, new Vector2(kv.Key.X * 64, kv.Key.Y * 64)), 0.8f, 0.5f, 1, false, Color.White, false);
                    }

                    break;
                }
            }
        }
    }
}