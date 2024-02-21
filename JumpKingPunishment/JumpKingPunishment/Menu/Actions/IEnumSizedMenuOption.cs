using JumpKing.PauseMenu.BT.Actions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// An interface that partially implements <see cref="IOptions"/> so it automatically sizes the option to the largest entry in an enum
    /// </summary>
    public abstract class IEnumSizedMenuOption : IOptions
    {
        Point largestOptionSize;

        /// <summary>
        /// The ctor for creating a <see cref="IEnumSizedMenuOption"/>
        /// </summary>
        /// <param name="optionCount">The number of values this option an display</param>
        /// <param name="defaultOption">The default option to initalize the option to</param>
        /// <param name="edgeMode">The <see cref="EdgeMode"/> to use for rendering the option</param>
        /// <param name="font">The <see cref="SpriteFont"/> to use for rendering the option</param>
        /// <exception cref="ArgumentNullException"></exception>
        public IEnumSizedMenuOption(int defaultOption, EdgeMode edgeMode, SpriteFont font) : base(1, defaultOption, edgeMode, font)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }

            OptionCount = GetNumOptions();

            largestOptionSize = new Point(0, 0);
            for (int i = 0; i < OptionCount; i++)
            {
                Point size = font.MeasureString(GetOptionDisplayText(i)).ToPoint();
                if (size.X > largestOptionSize.X)
                {
                    largestOptionSize = size;
                }
            }
        }

        /// <summary>
        /// Gets the number of options this option can display
        /// </summary>
        /// <returns>An int for the number of options this option can display, most likely the number of entries in an enum</returns>
        public abstract int GetNumOptions();

        /// <summary>
        /// Gets the display text for the given option
        /// </summary>
        /// <param name="option">Which option to get display text for</param>
        /// <returns>The display text as a string for the option, most likely the name string from an enum</returns>
        public abstract string GetOptionDisplayText(int option);

        /// <summary>
        /// Implements <see cref="IOptions.CurrentOptionName"/> to use the <see cref="GetOptionDisplayText"/> function.
        /// </summary>
        /// <returns>The string that will be displayed by IOptions for this option</returns>
        protected override string CurrentOptionName()
        {
            return GetOptionDisplayText(base.CurrentOption);
        }

        /// <summary>
        /// Implements <see cref="IOptions.GetSize"/> to return the width of the option, taking the longest option display text into account
        /// </summary>
        /// <returns></returns>
        public override Point GetSize()
        {
            // Return the length of the longest string in the enum
            Point size = largestOptionSize;
            size.X += GetExtraWidth();
            // GetExtraWidth doesn't account for indenting so add a bit more on top of that
            size.X += 16;

            return size;
        }
    }
}
