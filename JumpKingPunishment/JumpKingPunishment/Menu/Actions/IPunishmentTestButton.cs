using BehaviorTree;
using JumpKing.Controller;
using JumpKing.PauseMenu;
using JumpKing.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// An interface that partially implements <see cref="IBTnode"/> and <see cref="IMenuItem"/> to allow creating buttons that just trigger
    /// a function on the derived class and change color when pressed to help 'test' devices
    /// </summary>
    public abstract class IPunishmentTestButton : IBTnode, IMenuItem
    {
        private SpriteFont font;

        /// <summary>
        /// The ctor for creating a <see cref="IPunishmentTestButton"/>
        /// </summary>
        /// <param name="font">What font to render the menu item with</param>
        /// <param name="highlightFrameCount">How many frames to highlight the menu option when enabled (if any)</param>
        public IPunishmentTestButton(SpriteFont font)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }
            this.font = font;
        }

        /// <summary>
        /// Checks whether this menu item can be executed
        /// </summary>
        /// <returns>A bool indicating whether this menu item can be executed</returns>
        protected abstract bool CanExecute();

        /// <summary>
        /// Called when the menu item is selected and activated (/executed) by the player
        /// </summary>
        protected abstract void OnExecute();

        /// <summary>
        /// Gets the display name for this menu item
        /// </summary>
        /// <returns>A string containing the text that will be rendered for this menu item</returns>
        protected abstract string GetDisplayName();

        /// <summary>
        /// Implements <see cref="IMenuItem.Draw"/> to render the slider
        /// </summary>
        public void Draw(int x, int y, bool selected)
        {
            // Polling the pad state in draw is bad but I can't find a better way to do this
            // without other bugs/issues given tick behaviors for menu items and such
            // (This is still bugged in if you press enter then scroll to the item while holding enter
            //  it will be highlighted without being triggered- but that's the least bad of the other bugs)
            bool highlight = false;
            if (selected)
            {
                PadState managerPadState = ControllerManager.instance.GetPadState();
                highlight = managerPadState.confirm;
            }

            Color displayColor = CanExecute() ? highlight ? Color.Yellow : Color.White : Color.Gray;
            TextHelper.DrawString(font, GetDisplayName(), new Vector2(x, y), displayColor, new Vector2(0f, 0f));
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.GetSize"/> to get the size of the test button
        /// </summary>
        public Point GetSize()
        {
            return font.MeasureString(GetDisplayName()).ToPoint();
        }

        /// <summary>
        /// Implements <see cref="IBTnode.MyRun"/> to handle the 
        /// </summary>
        protected override BTresult MyRun(TickData p_data)
        {
            if (!CanExecute())
            {
                return BTresult.Failure;
            }

            PadState padState = ControllerManager.instance.MenuController.GetPadState();
            if (!padState.confirm)
            {
                return BTresult.Failure;
            }

            OnExecute();
            ControllerManager.instance.MenuController.ConsumePadPresses();
            return BTresult.Success;
        }
    }
}
