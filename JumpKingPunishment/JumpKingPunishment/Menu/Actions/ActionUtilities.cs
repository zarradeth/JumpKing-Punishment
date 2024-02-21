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

        public PunishmentPadState(Traverse jkPadState)
        {
            up = jkPadState.Field("up").GetValue<bool>();
            down = jkPadState.Field("down").GetValue<bool>();
            left = jkPadState.Field("left").GetValue<bool>();
            right = jkPadState.Field("right").GetValue<bool>();
            jump = jkPadState.Field("jump").GetValue<bool>();
            confirm = jkPadState.Field("confirm").GetValue<bool>();
            cancel = jkPadState.Field("cancel").GetValue<bool>();
            pause = jkPadState.Field("pause").GetValue<bool>();
            boots = jkPadState.Field("boots").GetValue<bool>();
            snake = jkPadState.Field("snake").GetValue<bool>();
            restart = jkPadState.Field("restart").GetValue<bool>();
        }
    }

    /// <summary>
    /// Contains helper/utility functions that are useful for creating menu actions/items
    /// </summary>
    public static class ActionUtilities
    {
        public static object ControllerManagerInstance
        {
            get
            {
                if (cachedControllerManagerInstance == null)
                {
                    cachedControllerManagerInstance = Traverse.CreateWithType("ControllerManager").Field("instance").GetValue();
                }
                return cachedControllerManagerInstance;
            }
        }
        private static object cachedControllerManagerInstance;

        public static object MenuControllerInstance
        {
            get
            {
                if (cachedMenuController == null)
                {
                    if (ControllerManagerInstance != null)
                    {
                        cachedMenuController = Traverse.Create(ControllerManagerInstance).Field("_menu_controller").GetValue();
                    }
                }

                return cachedMenuController;
            }
        }
        private static object cachedMenuController;

        /// <summary>
        /// Reads and returns the current pad state from the <see cref="JumpKing.Controller.ControllerManager"/>
        /// This is the current pad state without any consumption/touching from the menu system
        /// </summary>
        /// <returns>A <see cref="PunishmentPadState"/> containing the current PadState form the <see cref="JumpKing.Controller.ControllerManager"/></returns>
        public static PunishmentPadState GetControllerManagerPadState()
        {
            // Pretty much everything with the controller is internal so we can't access it without jumping through some hoops
            if (ControllerManagerInstance == null)
            {
                return new PunishmentPadState();
            }

            var getPadStateMethod = AccessTools.Method("JumpKing.Controller.ControllerManager:GetPadState");
            object padState = getPadStateMethod.Invoke(ControllerManagerInstance, null);

            var stateTraverse = Traverse.Create(padState);
            return new PunishmentPadState(stateTraverse);
        }

        /// <summary>
        /// Reads and returns the current pad state from the <see cref="JumpKing.Controller.MenuController"/>
        /// This pad state includes consumption of input and modifications to the state from the menu system
        /// </summary>
        /// <returns>A <see cref="PunishmentPadState"/> containing the current PadState form the <see cref="JumpKing.Controller.MenuController"/></returns>
        public static PunishmentPadState GetMenuControllerPadState()
        {
            // Pretty much everything with the controller is internal so we can't access it without jumping through some hoops
            if (MenuControllerInstance == null)
            {
                return new PunishmentPadState();
            }

            var getPadStateMethod = AccessTools.Method("JumpKing.Controller.MenuController:GetPadState");
            object padState = getPadStateMethod.Invoke(MenuControllerInstance, null);

            var stateTraverse = Traverse.Create(padState);
            return new PunishmentPadState(stateTraverse);
        }

        /// <summary>
        /// Calls <see cref="JumpKing.Controller.MenuController:ConsumePadPresses"/> on the <see cref="JumpKing.Controller.MenuController"/> instance on the
        /// <see cref="JumpKing.Controller.ControllerManager"/>.
        /// This helper is needed as the ControllerManager and MenuController are internal and not exposed
        /// </summary>
        public static void ConsumePadPresses()
        {
            if (MenuControllerInstance != null)
            {
                var ConsumePadPressesMethod = AccessTools.Method("JumpKing.Controller.MenuController:ConsumePadPresses");
                ConsumePadPressesMethod.Invoke(MenuControllerInstance, null);
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
