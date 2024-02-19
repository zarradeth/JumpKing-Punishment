namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// An interface for implementing widgets that can have focus in tandem with a <see cref="PunishmentFocusCompatableMenuSelector"/>
    /// </summary>
    public interface IPunishmentFocusable
    {
        /// <summary>
        /// Returns true if the widget has focus
        /// </summary>
        /// <returns>Whether the widget has focus or not</returns>
        bool HasFocus();
    }
}
