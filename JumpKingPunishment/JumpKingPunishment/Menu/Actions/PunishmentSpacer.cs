using BehaviorTree;
using JumpKing;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;
using Microsoft.Xna.Framework;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// A spacer class for rendering a line between menu items
    ///     Note: This class probably does not work as you would expect as Jump King handles UnSelectable menu items poorly and it's not easy to fix
    ///     this without creating a new MenuSelector class or patching the <see cref="JumpKing.PauseMenu.BT.MenuSelector.FixIndex"/> function
    /// </summary>
    public class PunishmentSpacer : IBTnode, IMenuItem, UnSelectable
    {
        private Point size;
        private Color color;

        /// <summary>
        /// The ctor for creating a <see cref="PunishmentSpacer"/>
        /// </summary>
        /// <param name="size">The size of the underline</param>
        /// <param name="color">The color of the underline when rendered</param>
        public PunishmentSpacer(Point size, Color color)
        {
            this.size = size;
            this.color = color;
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.Draw"/> to render the spacer
        /// </summary>
        public void Draw(int x, int y, bool selected)
        {
            Game1.spriteBatch.Draw(Game1.instance.contentManager.Pixel.texture, new Rectangle(x, y, size.X, size.Y), color);
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.GetSize"/> to get the size of the spacer
        /// </summary>
        public Point GetSize()
        {
            return size;
        }

        /// <summary>
        /// Implements <see cref="IBTnode.MyRun"/> for the spacer
        /// </summary>
        protected override BTresult MyRun(TickData p_data)
        {
            return BTresult.Failure;
        }
    }
}