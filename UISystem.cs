using BankButtons.Buttons;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace BankButtons;

public class UISystem : ModSystem
{
    internal static UserInterface ButtonsInterface;
    internal static ButtonsUI ButtonsUI;

    public override void PostSetupContent()
    {
        if (Main.dedServ || Main.netMode == NetmodeID.Server) return;

        ButtonsUI = new ButtonsUI();
        ButtonsUI.Activate();
        ButtonsInterface = new UserInterface();
        ButtonsInterface.SetState(ButtonsUI);
    }

    public override void Unload()
    {
        ButtonsUI = null;
        ButtonsInterface = null;
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        if (!Main.playerInventory) return;

        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Builder Accessories Bar");
        if (index == -1) return;

        layers.Insert(++index, new LegacyGameInterfaceLayer(
            "BankButtons: Buttons",
            delegate
            {
                ButtonsInterface.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI
        ));
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (ButtonsUI.Dragging) return;

        foreach (Button button in BankButtons.Buttons)
        {
            if (button.BankChestId == BankIds.VoidVault) VoidVisible = button.Visible;
            if (!button.Visible) continue;

            if (!(Main.drawingPlayerChat || Main.editSign || Main.editChest || Main.blockInput || Main.LocalPlayer.dead || Main.ingameOptionsWindow || Main.gameMenu || Main.inFancyUI))
            {
                if (button.Bind?.JustPressed == true) button.KeybindPress();
            }

            if (!button.Hovering || !Main.playerInventory) continue;

            Main.blockMouse = true;

            bool quickstack = Config.Instance.QuickstackEnabled && Main.keyState.PressingShift();
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
        Player player = Main.LocalPlayer;
        int chest = player.chest;
        player.chest = button.BankChestId;
        ChestUI.QuickStack(ContainerTransferContext.FromUnknown(player));
        player.chest = chest;
    }

    public static bool ItemInInventoryOrVoidBag(int id)
    {
        if (Main.LocalPlayer.HasItem(id)) return true;
        return VoidVisible && Main.LocalPlayer.bank4.item.Any(item => item.type == id);
    }

    private static bool VoidVisible;
}
