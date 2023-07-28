using BankButtons.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace BankButtons;

public class ButtonsUI : UIState
{
    internal static bool Dragging;
    private static bool BlockDrag;
    private static Vector2 LastDragPos;

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (Dragging) Drag();

        int offset = 0;

        List<Button> buttonsToDraw = new();
        foreach (Button button in BankButtons.Buttons)
        {
            if (!button.Visible) continue;

            if (Config.Instance.PreDrawOffset)
            {
                offset -= (Config.Instance.Horizontal ? button.GetIcon.Width : button.GetIcon.Height) + 2;
            }

            buttonsToDraw.Add(button);
        }

        foreach (Button button in buttonsToDraw)
        {
            Texture2D tex = button.GetIcon;
            Vector2 position = new(Config.Instance.XPosition, Config.Instance.YPosition);

            if (Config.Instance.Horizontal)
            {
                position.X += offset;
                offset += tex.Width + 2;
            }
            else
            {
                position.Y += offset;
                offset += tex.Height + 2;
            }

            Rectangle rect = new((int)position.X, (int)position.Y, tex.Width, tex.Height);

            button.Hovering = rect.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y);
            bool inChest = Main.LocalPlayer.chest == button.BankChestId;

            spriteBatch.Draw(tex, position, Color.White);
            if (Dragging)
            {
                spriteBatch.Draw(button.Border.Value, position, Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.25f % 1f, 1f, 0.75f));
                continue;
            }

            if (button.Hovering)
            {
                Main.instance.MouseTextHackZoom(button.GetHoverText);

                if (BlockDrag) BlockDrag = false;
                else if (Config.Instance.DraggingEnabled && PlayerInput.Triggers.JustPressed.MouseMiddle)
                {
                    Dragging = true;
                    LastDragPos = Main.MouseScreen;
                    continue;
                }
            }
            else if (Main.LocalPlayer.chest != button.BankChestId) { continue; }

            spriteBatch.Draw(button.Border.Value, position, button.Hovering ? Color.Yellow : Color.White);
        }
    }

    private static void Drag()
    {
        Main.blockMouse = true;

        Vector2 drag = Vector2.Subtract(Main.MouseScreen, LastDragPos);
        LastDragPos = Main.MouseScreen;
        Config.Instance.XPosition += (int)drag.X;
        Config.Instance.YPosition += (int)drag.Y;
        Main.instance.MouseTextHackZoom($"({Config.Instance.XPosition}, {Config.Instance.YPosition})");

        if (PlayerInput.Triggers.JustPressed.MouseLeft)
            Config.Instance.Horizontal = !Config.Instance.Horizontal;

        if (PlayerInput.Triggers.JustPressed.MouseMiddle || !Config.Instance.DraggingEnabled)
        {
            Dragging = false;
            BlockDrag = true;
            Config.Save();
        }
    }
}
