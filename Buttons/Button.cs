using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace BankButtons.Buttons;

public class Button
{
    public bool Hovering;

    public int BankChestId;
    public List<int> ItemTypes;
    public List<int> TileTypes;
    public string HoverText;
    public Asset<Texture2D> Icon;
    public Asset<Texture2D> Border;
    public ModKeybind Bind;
    public SoundStyle OpenSound;
    public SoundStyle CloseSound;

    public Button(int bankChestId,
        List<int> itemTypes,
        List<int> tileTypes,
        string hoverText,
        Asset<Texture2D> icon,
        Asset<Texture2D> border,
        ModKeybind bind,
        SoundStyle? sound = null,
        SoundStyle? closeSound = null
    )
    {
        BankChestId = bankChestId;
        ItemTypes = itemTypes;
        TileTypes = tileTypes;
        HoverText = hoverText;
        Icon = icon;
        Border = border;
        Bind = bind;

        OpenSound = sound ?? SoundID.MenuOpen;
        CloseSound = closeSound ?? (sound ?? SoundID.MenuClose);
    }

    public virtual bool Visible => Config.Instance.AlwaysVisible
                                || ItemTypes.Any(UISystem.ItemInInventoryOrVoidBag)
                                || TileTypes.Any(tile => Main.LocalPlayer.IsTileTypeInInteractionRange(tile));

    public virtual Texture2D GetIcon => Icon.Value;
    public virtual string GetHoverText => HoverText;
    public virtual SoundStyle GetSound(bool open) => open ? OpenSound : CloseSound;

    public virtual void MouseLeft() => UISystem.OpenBank(this);
    public virtual void MouseRight() { }
    public virtual void KeybindPress() => UISystem.OpenBank(this);
}
