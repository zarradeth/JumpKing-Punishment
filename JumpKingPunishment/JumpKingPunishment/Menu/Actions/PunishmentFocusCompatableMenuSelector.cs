using BehaviorTree;
using JumpKing;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// A custom MenuSelector that will override the default MenuSelector behavior when an <see cref="IPunishmentFocusable"/> has focus,
    /// which disables most of the normal menu logic until the focus is released.
    /// </summary>
    public class PunishmentFocusCompatableMenuSelector : MenuSelector
    {
        /// <summary>
        /// The ctor for creating a <see cref="PunishmentFocusCompatableMenuSelector"/>
        /// </summary>
        /// <param name="p_format">The GuiFormat for the menu</param>
        public PunishmentFocusCompatableMenuSelector(GuiFormat p_format) : base(p_format)
        {
        }

        /// <summary>
        /// Implements <see cref="IBTnode.MyRun"/> to handle focusable menu items, otherwise uses the base run behavior
        /// </summary>
        protected override BTresult MyRun(TickData p_data)
        {
            // If the current menu item has focus don't run normal logic for the menu until it releases it
            // Just update the menu item and handle select sounds in case the item triggers them
            IPunishmentFocusable focusableMenuItem = m_menu_items[Index] as IPunishmentFocusable;
            if ((focusableMenuItem != null) && focusableMenuItem.HasFocus())
            {
                bool flag = m_last_child_result > BTresult.Running;
                m_last_child_result = (m_menu_items[this.Index] as IBTnode).Run(p_data);
                if (m_last_child_result != BTresult.Failure && flag)
                {
                    Game1.instance.contentManager.audio.menu.OnSelect();
                }
                return BTresult.Running;
            }
            else
            {
                return base.MyRun(p_data);
            }
        }
    }
}
