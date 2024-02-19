using HarmonyLib;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// Contains pad state information from Jump King's controller system
    /// This matches the <see cref="JumpKing.Controller.PadState"/> as we cannot directly use that type
    /// </summary>
    public struct PunishmentPadState
    {
        public bool up;
        public bool down;
        public bool left;
        public bool right;
        public bool jump;
        public bool confirm;
        public bool cancel;
        public bool pause;
        public bool boots;
        public bool snake;
        public bool restart;
    }

    /// <summary>
    /// Contains helper/utility functions that are useful for creating menu actions/items
    /// </summary>
    public static class ActionUtilities
    {
        /// <summary>
        /// Reads and returns the current pad state from the <see cref="JumpKing.Controller.ControllerManager"/>
        /// This is the current pad state without any consumption/touching from the menu system
        /// </summary>
        /// <returns>A <see cref="PunishmentPadState"/> containing the current PadState form the <see cref="JumpKing.Controller.ControllerManager"/></returns>
        public static PunishmentPadState GetControllerManagerPadState()
        {
            // Pretty much everything with the controller is internal so we can't access it without jumping through some hoops
            var ControllerManagerInstance = Traverse.CreateWithType("ControllerManager").Field("instance").GetValue();
            if (ControllerManagerInstance == null)
            {
                return new PunishmentPadState();
            }

            var GetPadStateMethod = AccessTools.Method("JumpKing.Controller.ControllerManager:GetPadState");
            object PadState = GetPadStateMethod.Invoke(ControllerManagerInstance, null);

            var StateTraverse = Traverse.Create(PadState);
            return new PunishmentPadState
            {
                up = StateTraverse.Field("up").GetValue<bool>(),
                down = StateTraverse.Field("down").GetValue<bool>(),
                left = StateTraverse.Field("left").GetValue<bool>(),
                right = StateTraverse.Field("right").GetValue<bool>(),
                jump = StateTraverse.Field("jump").GetValue<bool>(),
                confirm = StateTraverse.Field("confirm").GetValue<bool>(),
                cancel = StateTraverse.Field("cancel").GetValue<bool>(),
                pause = StateTraverse.Field("pause").GetValue<bool>(),
                boots = StateTraverse.Field("boots").GetValue<bool>(),
                snake = StateTraverse.Field("snake").GetValue<bool>(),
                restart = StateTraverse.Field("restart").GetValue<bool>()
            };
        }

        /// <summary>
        /// Reads and returns the current pad state from the <see cref="JumpKing.Controller.MenuController"/>
        /// This pad state includes consumption of input and modifications to the state from the menu system
        /// </summary>
        /// <returns>A <see cref="PunishmentPadState"/> containing the current PadState form the <see cref="JumpKing.Controller.MenuController"/></returns>
        public static PunishmentPadState GetMenuControllerPadState()
        {
            // Pretty much everything with the controller is internal so we can't access it without jumping through some hoops
            var ControllerManagerInstance = Traverse.CreateWithType("ControllerManager").Field("instance").GetValue();
            if (ControllerManagerInstance == null)
            {
                return new PunishmentPadState();
            }

            var MenuControllerInstance = Traverse.Create(ControllerManagerInstance).Field("_menu_controller").GetValue();
            if (MenuControllerInstance == null)
            {
                return new PunishmentPadState();
            }

            var GetPadStateMethod = AccessTools.Method("JumpKing.Controller.MenuController:GetPadState");
            object PadState = GetPadStateMethod.Invoke(MenuControllerInstance, null);

            var StateTraverse = Traverse.Create(PadState);
            return new PunishmentPadState
            {
                up = StateTraverse.Field("up").GetValue<bool>(),
                down = StateTraverse.Field("down").GetValue<bool>(),
                left = StateTraverse.Field("left").GetValue<bool>(),
                right = StateTraverse.Field("right").GetValue<bool>(),
                jump = StateTraverse.Field("jump").GetValue<bool>(),
                confirm = StateTraverse.Field("confirm").GetValue<bool>(),
                cancel = StateTraverse.Field("cancel").GetValue<bool>(),
                pause = StateTraverse.Field("pause").GetValue<bool>(),
                boots = StateTraverse.Field("boots").GetValue<bool>(),
                snake = StateTraverse.Field("snake").GetValue<bool>(),
                restart = StateTraverse.Field("restart").GetValue<bool>()
            };
        }

        /// <summary>
        /// Calls <see cref="JumpKing.Controller.MenuController:ConsumePadPresses"/> on the <see cref="JumpKing.Controller.MenuController"/> instance on the
        /// <see cref="JumpKing.Controller.ControllerManager"/>.
        /// This helepr is needed as the ControllerManager and MenuController are internal and not exposed
        /// </summary>
        public static void ConsumePadPresses()
        {
            var ControllerManagerInstance = Traverse.CreateWithType("ControllerManager").Field("instance").GetValue();
            if (ControllerManagerInstance != null)
            {
                var MenuControllerInstance = Traverse.Create(ControllerManagerInstance).Field("_menu_controller").GetValue();
                if (MenuControllerInstance != null)
                {
                    var ConsumePadPressesMethod = AccessTools.Method("JumpKing.Controller.MenuController:ConsumePadPresses");
                    ConsumePadPressesMethod.Invoke(MenuControllerInstance, null);
                }
            }
        }

        /// <summary>
        /// Draws a sprite with a certain coloring, this helper exists as <see cref="Sprite"/> doesn't expose a draw function that takes a color
        /// and it saves the color on the sprite, so changing it without reverting it would impact all instances of rendering the sprite.
        /// </summary>
        /// <param name="sprite">The <see cref="Sprite"/> to draw</param>
        /// <param name="color">The color to draw with</param>
        /// <param name="x">The x location to draw at</param>
        /// <param name="y">The y location to draw at</param>
        /// <param name="effects">Any <see cref="SpriteEffect"/> to draw with</param>
        public static void DrawSpriteColored(Sprite sprite, Color color, float x, float y, SpriteEffects effects = SpriteEffects.None)
        {
            Color originalColor = sprite.GetColor();
            sprite.SetColor(color);
            sprite.Draw(x, y, effects);
            sprite.SetColor(originalColor);
        }

        /// <summary>
        /// Draws a sprite with a certain coloring, this helper exists as sprite don't expose a draw function that takes a color
        /// and it saves the color on the sprite, so changing it without reverting it would impact all instances of rendering the sprite.
        /// </summary>
        /// <param name="sprite">The <see cref="Sprite"/> to draw</param>
        /// <param name="color">The color to draw with</param>
        /// <param name="DrawArea">A <see cref="Rectangle"/> for the area to draw the sprite to</param>
        /// <param name="effects">Any <see cref="SpriteEffect"/> to draw with</param>
        public static void DrawSpriteColored(Sprite sprite, Color color, Rectangle DrawArea, SpriteEffects effects = SpriteEffects.None)
        {
            Color originalColor = sprite.GetColor();
            sprite.SetColor(color);
            sprite.Draw(DrawArea, effects);
            sprite.SetColor(originalColor);
        }
    }
}
