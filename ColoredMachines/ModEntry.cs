using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Object = StardewValley.Object;

namespace ColoredMachines
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        private static readonly string[] MachineTypes = { "Keg", "Preserves Jar", "Cask", "Oil Maker", "Cheese Press", "Mayonnaise Machine" };

        private static readonly Dictionary<GameLocation, List<Vector2>> TrackedMachines = new Dictionary<GameLocation, List<Vector2>>();

        private ModConfig config;
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.SaveLoaded += SaveLoaded;
            helper.Events.World.ObjectListChanged += ObjectListChanged;
            helper.Events.Display.RenderedWorld += RenderedWorld;
            
           config = this.Helper.ReadConfig<ModConfig>();
        }

        private void SaveLoaded(object sender, SaveLoadedEventArgs a)
        {
            //Populate list of machines
            foreach (GameLocation location in Game1.locations)
            {

                List<Vector2> machinesInLocation;
                if (!TrackedMachines.ContainsKey(location))
                {
                    machinesInLocation = new List<Vector2>();
                    TrackedMachines[location] = machinesInLocation;
                }

                machinesInLocation = TrackedMachines[location];
                foreach (var o in location.objects.Pairs)
                {
                    if (MachineTypes.Contains(o.Value.Name))
                    {
                        machinesInLocation.Add(o.Key);
                    }
                }
            }
        }

        private void ObjectListChanged(object sender, ObjectListChangedEventArgs e)
        {
            GameLocation location = e.Location;
            List<Vector2> machinesInLocation;
            if (!TrackedMachines.ContainsKey(location))
            {
                machinesInLocation = new List<Vector2>();
                TrackedMachines[location] = machinesInLocation;
            }
            machinesInLocation = TrackedMachines[location];
            foreach (KeyValuePair<Vector2, Object> pair in e.Added)
            {
                if (MachineTypes.Contains(pair.Value.Name))
                {
                    machinesInLocation.Add(pair.Key);
                    Monitor.Log("Adding machine at location " + pair.Key.X + ", " + pair.Key.Y);
                }
            }
            foreach (KeyValuePair<Vector2, Object> pair in e.Removed)
            {
                if (MachineTypes.Contains(pair.Value.Name))
                {
                    machinesInLocation.Remove(pair.Key);
                    Monitor.Log("Removing machine at location " + pair.Key.X + ", " + pair.Key.Y);
                }
            }
        }

        private void RenderedWorld(object sender, RenderedWorldEventArgs e)
        {
            if (TrackedMachines.ContainsKey(Game1.currentLocation))
            {
                foreach (Vector2 tile in TrackedMachines[Game1.currentLocation])
                {
                    Object obj = Game1.currentLocation.getObjectAtTile((int) tile.X, (int) tile.Y);
                    Color color = determineColor(obj);
                    if (color != Color.White)
                    {
                        float alpha = Math.Min(Math.Max(config.AlphaValue, 0), 1);
                        int x = (int) tile.X;
                        int y = (int) tile.Y;
                        Vector2 vector2 = obj.getScale() * 4f;
                        Vector2 local = Game1.GlobalToLocal(Game1.viewport,
                            new Vector2(x * 64, y * 64 - 64));
                        Rectangle destinationRectangle = new Rectangle(
                            (int) (local.X - vector2.X / 2.0) + (obj.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0),
                            (int) (local.Y - vector2.Y / 2.0) + (obj.shakeTimer > 0 ? Game1.random.Next(-1, 2) : 0),
                            (int) (64.0 + vector2.X), (int) (128.0 + vector2.Y / 2.0));
                        e.SpriteBatch.Draw(Game1.bigCraftableSpriteSheet, destinationRectangle,
                            new Rectangle?(Object.getSourceRectForBigCraftable(obj.showNextIndex.Value
                                ? obj.ParentSheetIndex + 1
                                : obj.ParentSheetIndex)), color * alpha, 0.0f, Vector2.Zero, SpriteEffects.None,
                            (float) (Math.Max(0.0f, ((y + 1) * 64 - 24) / 10000f) +
                                     (obj.ParentSheetIndex == 105 ? 0.00350000010803342 : 0.0) +
                                     x * 9.99999974737875E-06));
                    }
                }
            }
        }

        private Color determineColor(Object obj)
        {
            Color color = Color.White;
            if (obj.readyForHarvest.Value)
            {
                color = ColorConverter.getColorFromString(config.ColorOptions[obj.Name]["ready"]);
            }
            else if (obj.MinutesUntilReady > 0)
            {
                color = ColorConverter.getColorFromString(config.ColorOptions[obj.Name]["processing"]);
            }
            else
            {
                color = ColorConverter.getColorFromString(config.ColorOptions[obj.Name]["empty"]);
            }
/*           if (obj.Name == "Keg")
            {
                if (obj.readyForHarvest.Value)
                {
                    color = ColorConverter.getColorFromString(config.KegReadyColor);
                }
                    
                else if (obj.MinutesUntilReady > 0)
                {
                    color = ColorConverter.getColorFromString(config.KegProcessingColor);
                }
                else
                {
                    color = ColorConverter.getColorFromString(config.KegEmptyColor);
                }
            }
            else if (obj.Name == "Preserves Jar")
            {
                if (obj.readyForHarvest.Value)
                {
                    color = ColorConverter.getColorFromString(config.PreservesJarReadyColor);
                }

                else if (obj.MinutesUntilReady > 0)
                {
                    color = ColorConverter.getColorFromString(config.PreservesJarProcessingColor);
                }
                else
                {
                    color = ColorConverter.getColorFromString(config.PreservesJarEmptyColor);
                }
            }
*/
            return color;
        }
    }
}