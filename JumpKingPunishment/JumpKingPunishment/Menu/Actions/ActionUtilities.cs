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
        private static object cachedControllerManagerInstance;
        private static object cachedMenuController;

        /// <summary>
        /// Gets <see cref="JumpKing.Controller.ControllerManager.instance"/> from the game, uses caching to improve performance on subsequent gets
        /// as JumpKing shouldn't update this value
        /// </summary>
        /// <returns>An object that is <see cref="JumpKing.Controller.ControllerManager.instance"/></returns>
        public static object GetControllerManagerInstance()
        {
            if (cachedControllerManagerInstance == null)
            {
                cachedControllerManagerInstance = Traverse.CreateWithType("ControllerManager").Field("instance").GetValue();
            }
            return cachedControllerManagerInstance;
        }

        /// <summary>
        /// Gets <see cref="JumpKing.Controller.ControllerManager._menu_controller"/> from the game, uses caching to improve performance on subsequent gets
        /// as JumpKing shouldn't update this value
        /// </summary>
        /// <returns>An object that is <see cref="JumpKing.Controller.ControllerManager._menu_controller"/></returns>
        public static object GetMenuControllerInstance()
        {
            if (cachedMenuController == null)
            {
                object controllerManager = GetControllerManagerInstance();
                if (controllerManager != null)
                {
                    cachedMenuController = Traverse.Create(controllerManager).Field("_menu_controller").GetValue();
                }
            }

            return cachedMenuController;
        }

        /// <summary>
        /// Reads and returns the current pad state from the <see cref="JumpKing.Controller.ControllerManager"/>
        /// This is the current pad state without any consumption/touching from the menu system
        /// </summary>
        /// <returns>A <see cref="PunishmentPadState"/> containing the current PadState form the <see cref="JumpKing.Controller.ControllerManager"/></returns>
        public static PunishmentPadState GetControllerManagerPadState()
        {
            // Pretty much everything with the controller is internal so we can't access it without jumping through some hoops
            var controllerManagerInstance = GetControllerManagerInstance();
            if (controllerManagerInstance == null)
            {
                return new PunishmentPadState();
            }

            var getPadStateMethod = AccessTools.Method("JumpKing.Controller.ControllerManager:GetPadState");
            object padState = getPadStateMethod.Invoke(controllerManagerInstance, null);

            var stateTraverse = Traverse.Create(padState);
            return new PunishmentPadState
            {
                up = stateTraverse.Field("up").GetValue<bool>(),
                down = stateTraverse.Field("down").GetValue<bool>(),
                left = stateTraverse.Field("left").GetValue<bool>(),
                right = stateTraverse.Field("right").GetValue<bool>(),
                jump = stateTraverse.Field("jump").GetValue<bool>(),
                confirm = stateTraverse.Field("confirm").GetValue<bool>(),
                cancel = stateTraverse.Field("cancel").GetValue<bool>(),
                pause = stateTraverse.Field("pause").GetValue<bool>(),
                boots = stateTraverse.Field("boots").GetValue<bool>(),
                snake = stateTraverse.Field("snake").GetValue<bool>(),
                restart = stateTraverse.Field("restart").GetValue<bool>()
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
            var menuControllerInstance = GetMenuControllerInstance();
            if (menuControllerInstance == null)
            {
                return new PunishmentPadState();
            }

            var getPadStateMethod = AccessTools.Method("JumpKing.Controller.MenuController:GetPadState");
            object padState = getPadStateMethod.Invoke(menuControllerInstance, null);

            var stateTraverse = Traverse.Create(padState);
            return new PunishmentPadState
            {
                up = stateTraverse.Field("up").GetValue<bool>(),
                down = stateTraverse.Field("down").GetValue<bool>(),
                left = stateTraverse.Field("left").GetValue<bool>(),
                right = stateTraverse.Field("right").GetValue<bool>(),
                jump = stateTraverse.Field("jump").GetValue<bool>(),
                confirm = stateTraverse.Field("confirm").GetValue<bool>(),
                cancel = stateTraverse.Field("cancel").GetValue<bool>(),
                pause = stateTraverse.Field("pause").GetValue<bool>(),
                boots = stateTraverse.Field("boots").GetValue<bool>(),
                snake = stateTraverse.Field("snake").GetValue<bool>(),
                restart = stateTraverse.Field("restart").GetValue<bool>()
            };
        }

        /// <summary>
        /// Calls <see cref="JumpKing.Controller.MenuController:ConsumePadPresses"/> on the <see cref="JumpKing.Controller.MenuController"/> instance on the
        /// <see cref="JumpKing.Controller.ControllerManager"/>.
        /// This helepr is needed as the ControllerManager and MenuController are internal and not exposed
        /// </summary>
        public static void ConsumePadPresses()
        {
            var menuControllerInstance = GetMenuControllerInstance();
            if (menuControllerInstance != null)
            {
                var ConsumePadPressesMethod = AccessTools.Method("JumpKing.Controller.MenuController:ConsumePadPresses");
                ConsumePadPressesMethod.Invoke(menuControllerInstance, null);
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
