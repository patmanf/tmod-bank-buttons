using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BankButtons.Buttons;

public class VoidButton : Button
{
    public int ItemOpen;
    public int ItemClosed;

    public LocalizedText OpenHoverText;
    public LocalizedText ClosedHoverText;

    public Asset<Texture2D> IconClosed;

    public VoidButton(int bankChestId,
        int itemOpen,
        int itemClosed,
        List<int> itemTypes,
        List<int> tileTypes,
        LocalizedText hoverText,
        LocalizedText openHoverText,
        LocalizedText closedHoverText,
        Asset<Texture2D> icon,
        Asset<Texture2D> iconClosed,
        Asset<Texture2D> border,
        ModKeybind bind,
        SoundStyle? sound = null,
        SoundStyle? closeSound = null
    ) : base(bankChestId, itemTypes, tileTypes, hoverText, icon, border, bind, sound, closeSound)
    {
        ItemOpen = itemOpen;
        ItemClosed = itemClosed;
        IconClosed = iconClosed;
        OpenHoverText = openHoverText;
        ClosedHoverText = closedHoverText;
    }

    private bool HasItem => Main.LocalPlayer.HasItem(ItemOpen);
    private bool HasClosedItem => Main.LocalPlayer.HasItem(ItemClosed);
    private bool Closed => !HasItem && HasClosedItem;
    private bool Togglable => HasItem || HasClosedItem;

    public override bool Visible => HasItem || HasClosedItem || base.Visible;
    public override Texture2D GetIcon => (Closed ? IconClosed : Icon).Value;
    public override string GetHoverText => (Togglable ? (Closed ? ClosedHoverText : OpenHoverText) : HoverText).Value;

    public override void MouseRight()
    {
        if (!Togglable) return;

        int find = Closed ? ItemClosed : ItemOpen;
        int replace = Closed ? ItemOpen : ItemClosed;
        bool replaced = false;

        for (int i = 0; i < Main.LocalPlayer.inventory.Length; i++)
        {
            Item item = Main.LocalPlayer.inventory[i];
            if (item.type == find)
            {
                item.ChangeItemType(replace);
                replaced = true;
            }
        }

        if (replaced) SoundEngine.PlaySound(SoundID.Grab);
    }
}
