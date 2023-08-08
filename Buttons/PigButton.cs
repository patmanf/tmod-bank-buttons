using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BankButtons.Buttons;

public class PigButton : Button
{
    public int PetBuff;

    public LocalizedText PetHoverText;

    public Asset<Texture2D> IconPet;

    public SoundStyle PetOpenSound;
    public SoundStyle PetCloseSound;

    public PigButton(
        int bankChestId,
        List<int> itemTypes,
        List<int> tileTypes,
        int petBuff,
        LocalizedText hoverText,
        LocalizedText petHoverText,
        Asset<Texture2D> icon,
        Asset<Texture2D> iconPet,
        Asset<Texture2D> border,
        ModKeybind bind,
        SoundStyle? sound = null,
        SoundStyle? closeSound = null,
        SoundStyle? petOpenSound = null,
        SoundStyle? petCloseSound = null
    ) : base(bankChestId, itemTypes, tileTypes, hoverText, icon, border, bind, sound, closeSound)
    {
        PetBuff = petBuff;
        PetHoverText = petHoverText;
        IconPet = iconPet;
        PetOpenSound = petOpenSound ?? OpenSound;
        PetCloseSound = petCloseSound ?? CloseSound;
    }

    private bool Pet => Main.LocalPlayer.HasBuff(PetBuff);

    public override bool Visible => Pet || base.Visible; 
    public override Texture2D GetIcon => (Config.Instance.Chester && Pet ? IconPet : Icon).Value;
    public override string GetHoverText => (Config.Instance.Chester && Pet ? PetHoverText : HoverText).Value;

    public override SoundStyle GetSound(bool open)
    {
        if (Config.Instance.Chester && Pet) { return open ? PetOpenSound : PetCloseSound; }
        return base.GetSound(open);
    }
}
