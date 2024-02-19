using JumpKing;
using JumpKing.PauseMenu;
using JumpKing.PauseMenu.BT.Actions;
using JumpKing.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// An interface that partially implements <see cref="IToggle"/> and <see cref="IMenuItem"/> to allow creating more flexible toggleable menu items
    /// </summary>
    public abstract class IPunishmentTextToggle : IToggle, IMenuItem
    {
        private Point stringSize;

        private SpriteFont font;
        private Point underlineSize;
        private Color underlineColor;
        private bool preUnderline;

        /// <summary>
        /// The ctor for creating a <see cref="IPunishmentTextToggle"/>
        /// </summary>
        /// <param name="font">What font to render this menu item with</param>
        /// <param name="startValue">The starting value of the menu item</param>
        /// <param name="underlineSize">The size of the underline to render for this menu item</param>
        /// <param name="underlineColor">The color of the underline to render for this menu item</param>
        /// <param name="preUnderline">If true the underline will render before the item, instead of after</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IPunishmentTextToggle(SpriteFont font, bool startValue, Point underlineSize = default(Point), Color underlineColor = default(Color), bool preUnderline = false) : base(startValue)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }

            this.underlineSize = underlineSize;
            this.underlineColor = underlineColor;
            this.preUnderline = preUnderline;

            this.font = font;
        }

        /// <summary>
        /// Returns the name rendered for this menu item
        /// </summary>
        /// <returns>A string containing the text that will be rendered for this menu item</returns>
        protected abstract string GetName();

        /// <summary>
        /// A helper to draw an underline (colored rectangle)
        /// </summary>
        /// <param name="x">The x location to draw at</param>
        /// <param name="y">The y location to draw at</param>
        /// <param name="selected">Whether the menu item is selected or not (so we can un-indent the line)</param>
        void DrawUnderline(int x, int y, bool selected)
        {
            if ((underlineSize.X > 0) && (underlineSize.Y > 0))
            {
                Game1.spriteBatch.Draw(Game1.instance.contentManager.Pixel.texture, new Rectangle(x - (selected ? 5 : 0), y, underlineSize.X, underlineSize.Y), underlineColor);
            }
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.Draw"/> to render the text toggle
        /// </summary>
        public virtual void Draw(int x, int y, bool selected)
        {
            // I was gonna do this with the PunishmentSpacer class but the MenuSelector logic doesn't handle Unselectable
            // items nicely (it doesn't skip over them as you would expect, there is code that would work but it will only
            // run if you aren't starting from a selectable menu option.... seems like a bug tbh)
            int stringY = y;
            if (preUnderline)
            {
                DrawUnderline(x, y, selected);
                stringY += underlineSize.Y;
            }

            TextHelper.DrawString(font, GetName(), new Vector2(x, stringY), CanChange() ? Color.White : Color.Gray, new Vector2(0f, 0f));
            DrawCheckBox(new Vector2(x + stringSize.X + 2, stringY + stringSize.Y / 2), base.toggle);

            if (!preUnderline)
            {
                DrawUnderline(x, stringY + stringSize.Y, selected);
            }
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.GetSize"/> to get the size of the text toggle
        /// </summary>
        public Point GetSize()
        {
            Point checkBoxSize = GetCheckBoxSize();
            stringSize = font.MeasureString(GetName()).ToPoint();

            Point result = stringSize;
            result.X += checkBoxSize.X;
            result.X += 2;

            result.Y += underlineSize.Y;
            result.X = Math.Max(result.X, underlineSize.X);

            return result;
        }
    }
}
