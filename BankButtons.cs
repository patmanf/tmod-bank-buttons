using BankButtons.Buttons;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BankButtons;

public class BankIds
{
    public const int PiggyBank = -2;
    public const int Safe = -3;
    public const int DefendersForge = -4;
    public const int VoidVault = -5;
}

public class BankButtons : Mod
{
    internal static Mod Mod;

    public static List<Button> Buttons { get; set; }
    internal static int? LastOpenedBank;

    public override void Load()
    {
        Mod = this;

        On_Player.HandleBeingInChestRange += ChestRange;
        On_Player.QuickStackAllChests += NearQuickstack;

        Buttons = new() {
            new PigButton(
                bankChestId: BankIds.PiggyBank,
                itemTypes: new List<int>() { ItemID.MoneyTrough, ItemID.PiggyBank },
                tileTypes: new List<int>() { TileID.PiggyBank },
                petBuff: BuffID.ChesterPet,
                hoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.PiggyBank"),
                petHoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.Chester"),
                icon: ModContent.Request<Texture2D>("BankButtons/Icons/Pig"),
                iconPet: ModContent.Request<Texture2D>("BankButtons/Icons/Chester"),
                border: ModContent.Request<Texture2D>("BankButtons/Icons/PigBorder"),
                bind: KeybindLoader.RegisterKeybind(this, "OpenPiggyBank", "None"),
                sound: SoundID.Item59,
                petOpenSound: SoundID.ChesterOpen,
                petCloseSound: SoundID.ChesterClose
            ),
            new Button(
                bankChestId: BankIds.Safe,
                itemTypes: new List<int>() { ItemID.Safe },
                tileTypes: new List<int>() { TileID.Safes },
                hoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.Safe"),
                icon: ModContent.Request<Texture2D>("BankButtons/Icons/Safe"),
                border: ModContent.Request<Texture2D>("BankButtons/Icons/SafeBorder"),
                bind: KeybindLoader.RegisterKeybind(this, "OpenSafe", "None"),
                sound: SoundID.Unlock
            ),
            new Button(
                bankChestId: BankIds.DefendersForge,
                itemTypes: new List<int>() { ItemID.DefendersForge },
                tileTypes: new List<int>() { TileID.DefendersForge },
                hoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.DefendersForge"),
                icon: ModContent.Request<Texture2D>("BankButtons/Icons/Forge"),
                border: ModContent.Request<Texture2D>("BankButtons/Icons/ForgeBorder"),
                bind: KeybindLoader.RegisterKeybind(this, "OpenDefendersForge", "None"),
                sound: SoundID.DD2_EtherianPortalSpawnEnemy
            ),
            new VoidButton(
                bankChestId: BankIds.VoidVault,
                itemOpen: ItemID.VoidLens,
                itemClosed: ItemID.ClosedVoidBag,
                itemTypes: new List<int>() { ItemID.VoidVault },
                tileTypes: new List<int>() { TileID.VoidVault },
                hoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.VoidBag"),
                openHoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.VoidBagOpen"),
                closedHoverText: Language.GetOrRegister("Mods.BankButtons.Tooltips.VoidBagClosed"),
                icon: ModContent.Request<Texture2D>("BankButtons/Icons/Void"),
                iconClosed: ModContent.Request<Texture2D>("BankButtons/Icons/VoidClosed"),
                border: ModContent.Request<Texture2D>("BankButtons/Icons/VoidBorder"),
                bind: KeybindLoader.RegisterKeybind(this, "OpenVoidBag", "None"),
                sound: SoundID.Item130
            )
        };
    }

    public override void Unload()
    {
        Mod = null;
        Buttons = null;
        On_Player.HandleBeingInChestRange -= ChestRange;
        On_Player.QuickStackAllChests -= NearQuickstack;
    }

    private void ChestRange(On_Player.orig_HandleBeingInChestRange orig, Player player)
    {
        if (player.chest == LastOpenedBank) return;
        if (LastOpenedBank != null) LastOpenedBank = null;

        orig.Invoke(player);
    }

    private void NearQuickstack(On_Player.orig_QuickStackAllChests orig, Player player)
    {
        orig.Invoke(player);

        if (Config.Instance.QuickstackNearby) {
            foreach (Button button in Buttons)
            {
                if (!button.Visible) continue;
                BankPlayer.QuickStack(button);
            }
        }
    }
}
