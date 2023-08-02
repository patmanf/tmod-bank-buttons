using System;
using System.ComponentModel;
using System.Reflection;
using Terraria;
using Terraria.ModLoader.Config;

namespace BankButtons;

[Label("Settings [i:87]")]
internal class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;
    public static Config Instance;

    [Label("Quickstack Enabled")]
    [Tooltip("Shift clicking a button will quickstack into that bank")]
    [DefaultValue(true)]
    public bool QuickstackEnabled;

    [Label("Quickstack to Nearby Chests Enabled")]
    [Tooltip("quickstacking to nearby chests will also quickstack into all banks")]
    [DefaultValue(true)]
    public bool QuickstackNearby;

    [Label("Chester")]
    [Tooltip("piggy bank icon is replaced with Chester when equipped :)")]
    [DefaultValue(false)]
    public bool Chester;

    [Label("Buttons Always Visible")]
    [Tooltip("buttons will be visible even without the bank in your inventory/void bag/placed nearby")]
    [DefaultValue(false)]
    public bool AlwaysVisible;

    [Label("Pre Draw Offset Thingy")]
    [Tooltip("origin is  the last button instead of the first button,")]
    [DefaultValue(true)]
    public bool PreDrawOffset;

    [Header("Position [i:393]")]
    
    [Label("Horizontal Buttons")]
    [DefaultValue(false)]
    public bool Horizontal;

    [Label("Dragging Allowed")]
    [Tooltip("pressing the middle mouse button will drag the buttons around\npress middle mouse again to stop dragging\nwhile dragging, pressing leftmouse will toggle horizontal button mode\nand rightmouse will toggle the 'predraw offset thingy'")]
    [DefaultValue(false)]
    public bool DraggingEnabled;

    [Label("X Position")]
    [DefaultValue(570)]
    [Range(0, int.MaxValue)]
    [Increment(10)]
    public int XPosition;

    [Label("Y Position")]
    [DefaultValue(230)]
    [Range(0, int.MaxValue)]
    [Increment(10)]
    public int YPosition;


    internal static void Save()
    {
        MethodInfo SaveMethodInfo = typeof(ConfigManager).GetMethod("Save", BindingFlags.Static | BindingFlags.NonPublic);
        if (SaveMethodInfo != null)
        {
            SaveMethodInfo.Invoke(null, new object[] { Instance });
            return;
        }
        throw new Exception("bankbuttons config saving failed");
    }
}