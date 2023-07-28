using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
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
            delegate {
                ButtonsInterface.Draw(Main.spriteBatch, new GameTime());
                return true;
            },
            InterfaceScaleType.UI
        ));
    }
}
