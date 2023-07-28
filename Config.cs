using System;
using System.ComponentModel;
using System.Reflection;
using Terraria;
using Terraria.ModLoader.Config;

namespace BankButtons;

internal class Config : ModConfig
{
    public override ConfigScope Mode => ConfigScope.ClientSide;
    public static Config Instance;

    [DefaultValue(true)]
    public bool QuickstackEnabled;

    [DefaultValue(true)]
    public bool QuickstackNearby;

    [DefaultValue(false)]
    public bool Chester;

    [DefaultValue(true)]
    public bool PreDrawOffset;

    [Header("Position")]
    [DefaultValue(false)]
    public bool Horizontal;

    [DefaultValue(false)]
    public bool DraggingEnabled;

    [DefaultValue(570)]
    [Range(0, int.MaxValue)]
    [Increment(10)]
    public int XPosition;

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