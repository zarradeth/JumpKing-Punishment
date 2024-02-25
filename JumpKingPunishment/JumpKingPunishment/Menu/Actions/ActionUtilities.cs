using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// Contains helper/utility functions that are useful for creating/updating/drawing menu actions/items
    /// </summary>
    public static class ActionUtilities
    {
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
