using BehaviorTree;
using ErikMaths;
using JumpKing;
using JumpKing.PauseMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Globalization;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// An interface that partially implements <see cref="IBTnode"/> and <see cref="IMenuItem"/> to allow creating slider options
    /// Has much greater functionality than Jump King's <see cref="JumpKing.PauseMenu.BT.Actions.ISlider"/> class
    /// </summary>
    public abstract class IPunishmentSlider : IBTnode, IMenuItem
    {
        private SpriteFont font;

        private float min;
        private float max;
        private float stepAmount;
        private bool allowHold;
        private float accelerationAmount;
        private bool wholeNumbers;
        private int displayPrecision;
        private int sliderLength;

        private float value;
        private float holdTime;

        /// <summary>
        /// The ctor for creating a <see cref="IPunishmentSlider"/>
        /// </summary>
        /// <param name="font">The font to render the slider with</param>
        /// <param name="min">The minimum value of the slider</param>
        /// <param name="max">The maximum value of the slider</param>
        /// <param name="startValue">The starting value of the slider</param>
        /// <param name="stepAmount">How much the value should change when the player increases/decreases the slider (default 1.0)</param>
        /// <param name="allowHold">Whether the player should be able to hold left/right to continuously change the slider value</param>
        /// <param name="stepAccelerationAmount">If holds are allowed, how much the change speed of the slider value accelerates as left/right is held</param>
        /// <param name="wholeNumbers">Whether the slider can represent/use floating point values or only whole numbers</param>
        /// <param name="displayPrecision">When not using whole values, How many decimal places to display for the slider value</param>
        /// <param name="sliderLength">The length that the slider should render, in pixels</param>
        public IPunishmentSlider(SpriteFont font, float min, float max, float startValue, float stepAmount = 1.0f, bool allowHold = true, float stepAccelerationAmount = 0.0f, bool wholeNumbers = false, int displayPrecision = 2, int sliderLength = 100)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }
            if (max <= min)
            {
                throw new ArgumentOutOfRangeException("Slider max must be larger than min");
            }
            if (sliderLength <= 0)
            {
                throw new ArgumentOutOfRangeException("Slider length must be positive");
            }
            if (stepAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("Slider step amount must be positive");
            }
            if (displayPrecision < 0)
            {
                throw new ArgumentOutOfRangeException("Slider display precision must be positive");
            }

            this.font = font;

            this.min = min;
            this.max = max;
            this.stepAmount = stepAmount;
            this.allowHold = allowHold;
            this.accelerationAmount = stepAccelerationAmount;
            this.wholeNumbers = wholeNumbers;
            this.displayPrecision = displayPrecision;
            this.sliderLength = sliderLength;

            SetValue(startValue);
        }

        /// <summary>
        /// Gets whether the slider can be changed or not
        /// </summary>
        protected abstract bool CanChange();

        /// <summary>
        /// Called when the slider value is changed
        /// </summary>
        /// <param name="newValue">The new value of the slider</param>
        protected abstract void OnSliderChange(float newValue);

        /// <summary>
        /// Gets the display name for this slider option
        /// </summary>
        protected abstract string GetDisplayName();

        /// <summary>
        /// Sets the value of the slider, with proper clamping and rounding
        /// </summary>
        /// <param name="newValue">The new value to set the slider to</param>
        /// <param name="triggerChange">Whether <see cref="OnSliderChange"/> should potentially be called or not</param>
        public void SetValue(float newValue)
        {
            float roundedValue = wholeNumbers ? (float)Math.Round(newValue) : newValue;
            float clampedValue = ErikMath.Clamp<float>(roundedValue, min, max);
            if (value != clampedValue)
            {
                value = clampedValue;
                OnSliderChange(value);
            }
        }

        /// <summary>
        /// Formats the current value of the slider for display (based on the <see cref="wholeNumbers"/> and <see cref="displayPrecision"/> property)
        /// </summary>
        /// <returns>The formatted string for the current value of the slider</returns>
        public string GetFormattedValue()
        {
            if (wholeNumbers)
            {
                return value.ToString("0");
            }
            else
            {
                return string.Format(new NumberFormatInfo() { NumberDecimalDigits = displayPrecision }, "{0:F}", value);
            }
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.Draw"/> to render the slider
        /// </summary>
        public void Draw(int x, int y, bool selected)
        {
            float valuePercent = ((value - min) / (max - min));

            // Main label
            Game1.spriteBatch.DrawString(font, GetDisplayName(), new Vector2(x, y), CanChange() ? Color.White : Color.Gray);

            // Slider
            int sliderX = x + (int)font.MeasureString(GetDisplayName()).X;
            int sliderY = y + 4;
            Color drawColor = CanChange() ? Color.White : Color.Gray;
            // Left
            Sprite sprite = Game1.instance.contentManager.gui.SliderLeft;
            ActionUtilities.DrawSpriteColored(sprite, drawColor, sliderX, sliderY);
            int width = sprite.source.Width;
            // Line
            sprite = Game1.instance.contentManager.gui.SliderLine;
            ActionUtilities.DrawSpriteColored(sprite, drawColor, new Rectangle(sliderX + width, sliderY, sliderLength, sprite.source.Height));
            // Right
            sprite = Game1.instance.contentManager.gui.SliderRight;
            ActionUtilities.DrawSpriteColored(sprite, drawColor, (width + sliderX + sliderLength), sliderY);
            // Cursor
            sprite = Game1.instance.contentManager.gui.SliderCursor;
            ActionUtilities.DrawSpriteColored(sprite, drawColor, (width + sliderX) + (sliderLength * valuePercent), sliderY);

            // Current Value
            string formattedValue = GetFormattedValue();
            Game1.spriteBatch.DrawString(font, formattedValue, new Vector2(((width * 2.0f) + sliderX + sliderLength), y), CanChange() ? Color.White : Color.Gray);
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.GetSize"/> to get the size of the slider
        /// </summary>
        public Point GetSize()
        {
            // Add in the slider length itself
            Point size = new Point(sliderLength, Game1.instance.contentManager.gui.SliderLeft.source.Height * 2 + 1);
            size.X += Game1.instance.contentManager.gui.SliderLeft.source.Width * 3;
            // Add in the display label
            size.X += font.MeasureString(GetDisplayName()).ToPoint().X;
            // Add in the value label
            size.X += font.MeasureString(GetFormattedValue()).ToPoint().X;

            return size;
        }

        /// <summary>
        /// Implements <see cref="IBTnode.MyRun"/> to handle the slider behavior
        /// </summary>
        protected override BTresult MyRun(TickData p_data)
        {
            if (!CanChange())
            {
                return BTresult.Failure;
            }

            // The controller manager is not exposed so we need to dig it up
            // Using pad state from the controller manager and not the menu controller because
            // we want input each frame/don't want the input to be consumed like normal menu input
            PunishmentPadState padState = ActionUtilities.GetControllerManagerPadState();

            if ((!padState.left && !padState.right) || (padState.left && padState.right))
            {
                holdTime = 0.0f;
                return BTresult.Failure;
            }

            bool bHeld = (holdTime > 0.0f);
            if (bHeld && !allowHold)
            {
                return BTresult.Failure;
            }

            // UX for changing values- if the player hasn't changed the value yet allow it
            // but if they haven't held for more then a tenth of a second don't change the value.
            // This makes fine tuning the value easier.
            if (holdTime > 0.0f && holdTime < 0.25f)
            {
                holdTime += p_data.delta_time;
                return BTresult.Failure;
            }
            float stepAmountAcceleration = 0.0f;
            // If the player has held the button for more than a quarter second start accelerating the step amount
            // to make it easier to do large changes. However limit acceleration so it won't go out of control
            if ((accelerationAmount > 0.0f) && (holdTime > 1.0f))
            {
                stepAmountAcceleration = (float)Math.Max(Math.Floor((double)((holdTime - 0.5f) / 0.5f)), 3.0) * accelerationAmount;
            }
            float acceleratedStepAmount = stepAmount + stepAmountAcceleration;

            float newValue = value;
            if (padState.left)
            {
                newValue = value - acceleratedStepAmount;
            }
            if (padState.right)
            {
                newValue = value + acceleratedStepAmount;
            }

            SetValue(newValue);

            bool wasFirstPress = !bHeld;
            holdTime += p_data.delta_time;

            // Success vs Failure for menu items triggers the 'select' sound, to not be annoying make it so
            // we only trigger the sound on the first change made when holding
            return wasFirstPress ? BTresult.Success : BTresult.Failure;
        }
    }
}
