using BankButtons.Buttons;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace BankButtons;

public class BankPlayer : ModPlayer
{
    private static bool VoidVisible;

    public override void ProcessTriggers(TriggersSet triggersSet)
    {
        if (ButtonsUI.Dragging) return;

        foreach (Button button in BankButtons.Buttons)
        {
            if (button.BankChestId == BankIds.VoidVault) VoidVisible = button.Visible;
            if (!button.Visible) continue;

            if (button.Bind?.JustPressed == true) button.KeybindPress();

            if (!button.Hovering) continue;
            Main.blockMouse = true;

            bool quickstack = Config.Instance.QuickstackingEnabled && Main.keyState.PressingShift();
            if (quickstack) Main.cursorOverride = 9;

            if (PlayerInput.Triggers.JustPressed.MouseLeft)
            {
                if (quickstack) QuickStack(button);
                else button.MouseLeft();
            }

            if (PlayerInput.Triggers.JustPressed.MouseRight) button.MouseRight();
        }
    }

    public static void OpenBank(Button button)
    {
        Player Player = Main.LocalPlayer;

        if (Player.chest != button.BankChestId)
        {
            Main.playerInventory = true;

            Player.chest = button.BankChestId;
            BankButtons.LastOpenedBank = button.BankChestId;

            Point pos = Player.Center.ToTileCoordinates();
            Player.chestX = pos.X;
            Player.chestY = pos.Y;

            Player.SetTalkNPC(-1);
            Main.SetNPCShopIndex(0);

            SoundEngine.PlaySound(button.GetSound(true));
        }
        else
        {
            Player.chest = -1;
            BankButtons.LastOpenedBank = null;
            SoundEngine.PlaySound(button.GetSound(false));
        }
    }

    public static void QuickStack(Button button)
    {
        Player Player = Main.LocalPlayer;

        int chest = Player.chest;
        Player.chest = button.BankChestId;
        ChestUI.QuickStack(ContainerTransferContext.FromUnknown(Player));
        Player.chest = chest;
    }

    public static bool ItemInInventoryOrVoidBag(int id)
    {
        if (Main.LocalPlayer.HasItem(id)) return true;
        return VoidVisible && Main.LocalPlayer.bank4.item.Any(item => item.type == id);
    }
}
