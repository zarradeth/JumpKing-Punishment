using BehaviorTree;
using JumpKing;
using JumpKing.Controller;
using JumpKing.PauseMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace JumpKingPunishment.Menu.Actions
{
    /// <summary>
    /// An interface that partially implements <see cref="IBTnode"/>, <see cref="IMenuItem"/>, and <see cref="IPunishmentFocusable"/> to allow creating
    /// text input for string type options. Jump King does not provide their own class with comparable functionality.
    /// </summary>
    public abstract class IPunishmentTextInput : IBTnode, IMenuItem, IPunishmentFocusable
    {
        private SpriteFont font;
        private int valueDisplayWidth;
        private int displayNameFixedWidth;
        private bool hideWhenNotFocused;

        private bool isFocused;
        private string value;

        Keys[] lastPressedKeys;

        /// <summary>
        /// The ctor for creating a <see cref="IPunishmentTextInput"/>
        /// </summary>
        /// <param name="font">What font to render the menu item with</param>
        /// <param name="initialValue">The initial value of the text input</param>
        /// <param name="valueDisplayWidth">How long, in pixels, the input field should render as</param>
        /// <param name="displayNameFixedWidth">An optional fixed width to render the display name of the input field- useful for lining fields up relatively</param>
        /// <param name="hideWhenNotFocused">If true the input field will render as '*' when it's not being actively edited to hide its contents</param>
        public IPunishmentTextInput(SpriteFont font, string initialValue, int valueDisplayWidth = 100, int displayNameFixedWidth = -1, bool hideWhenNotFocused = false)
        {
            if (font == null)
            {
                throw new ArgumentNullException(nameof(font));
            }
            this.font = font;
            this.valueDisplayWidth = valueDisplayWidth;
            this.displayNameFixedWidth = displayNameFixedWidth;
            this.hideWhenNotFocused = hideWhenNotFocused;

            this.value = initialValue;
        }

        /// <summary>
        /// Gets whether or not the text input can be changed
        /// </summary>
        protected abstract bool CanChange();

        /// <summary>
        /// Called when the text input on the item is changed
        /// </summary>
        /// <param name="newValue">The new string value of the input item</param>
        protected abstract void OnTextChange(string newValue);

        /// <summary>
        /// Gets the display name for this menu item
        /// </summary>
        /// <returns>A string containing the text that will be rendered for this menu item</returns>
        protected abstract string GetDisplayName();

        /// <summary>
        /// Checks whether the given character is allowed to be entered into the text input
        /// </summary>
        /// <param name="c">The character that we are attempting to add</param>
        /// <returns>A bool indicating whether or not the character is allowed in the input field</returns>
        protected abstract bool IsAllowedCharacter(char c);

        /// <summary>
        /// Implements <see cref="IPunishmentFocusable.HasFocus"/> and returns true when the field is being edited/is selected
        /// </summary>
        public bool HasFocus() { return isFocused; }

        /// <summary>
        /// Implements <see cref="IBTnode.MyRun"/> to handle the text input behavior
        /// </summary>
        protected override BTresult MyRun(TickData p_data)
        {
            if (!CanChange())
            {
                return BTresult.Failure;
            }

            if (!isFocused)
            {
                PadState padState = ControllerManager.instance.MenuController.GetPadState();
                if (padState.confirm)
                {
                    isFocused = true;
                    ControllerManager.instance.MenuController.ConsumePadPresses();
                    lastPressedKeys = Keyboard.GetState().GetPressedKeys();
                    Game1.instance.Window.TextInput += OnTextInput;
                    return BTresult.Success;
                }
            }
            else
            {
                // Keyboard input handling, use keyboard state directly
                KeyboardState keyboardState = Keyboard.GetState();
                Keys[] keys = keyboardState.GetPressedKeys();
                // Determine new key presses
                Keys[] newKeys = keys.Except(lastPressedKeys).ToArray();
                lastPressedKeys = keys;

                if (keyboardState.IsKeyDown(Keys.LeftControl) || keyboardState.IsKeyDown(Keys.RightControl))
                {
                    // If left or right control is pressed down only look for copy/paste
                    if (newKeys.Contains(Keys.C))
                    {
                        // Copy
                        if (value.Length > 0)
                        {
                            System.Windows.Forms.Clipboard.SetText(value);
                        }
                    }
                    else if (newKeys.Contains(Keys.V))
                    {
                        // Paste
                        string clipboardContents = System.Windows.Forms.Clipboard.GetText();
                        foreach (char c in clipboardContents)
                        {
                            if (IsAllowedCharacter(c))
                            {
                                value += c;
                            }
                        }
                        OnTextChange(value);
                    }
                }
                else if (newKeys.Contains(Keys.Enter))
                {
                    isFocused = false;
                    ControllerManager.instance.MenuController.ConsumePadPresses();
                    Game1.instance.Window.TextInput -= OnTextInput;
                    return BTresult.Success;
                }
                else if (newKeys.Contains(Keys.Delete))
                {
                    // Backspace is handled in OnTextInput, Delete doesn't route through that
                    if (value.Length > 0)
                    {
                        value = value.Remove(value.Length - 1);
                        OnTextChange(value);
                    }
                }
            }

            return BTresult.Failure;
        }

        /// <summary>
        /// Bound to <see cref="Game1.instance.Window.TextInput"/> when the text input is active to handle text input
        /// </summary>
        protected void OnTextInput(object sender, TextInputEventArgs e)
        {
            if (IsAllowedCharacter(e.Character))
            {
                value += e.Character;
                OnTextChange(value);
            }
            else if (e.Key == Keys.Back)
            {
                if (value.Length > 0)
                {
                    value = value.Remove(value.Length - 1);
                    OnTextChange(value);
                }
            }
        }

        /// <summary>
        /// Checks and Formats the provided text to fit within a certain width (perpending it with '...' and removing the starting characters if it doesn't fit)
        /// </summary>
        /// <param name="text">The text to be formatted and returned</param>
        /// <param name="width">The width in pixels that the text should be fit to</param>
        /// <returns>The formatted text, or the original string if no changes are needed</returns>
        public string FormatTextToWidth(string text, int width)
        {
            if (text.Length == 0)
            {
                return text;
            }

            int stringLength = font.MeasureString(text).ToPoint().X;
            if (stringLength < width)
            {
                return text;
            }

            int choppedCount = 1;
            while (font.MeasureString("..." + text.Substring(choppedCount)).ToPoint().X > width)
            {
                choppedCount++;
                if (choppedCount == text.Length)
                {
                    // We can't fit anything in the width...?
                    return text;
                }
            }

            return "..." + text.Substring(choppedCount);
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.Draw"/> to render the text input
        /// </summary>
        public void Draw(int x, int y, bool selected)
        {
            Color mainColor = CanChange() ? Color.White : Color.Gray;
            Color displayNameColors = mainColor;
            if (isFocused)
            {
                displayNameColors = Color.Yellow;
            }

            int currentX = x;
            Game1.spriteBatch.DrawString(font, GetDisplayName(), new Vector2(currentX, y), displayNameColors);
            if (displayNameFixedWidth >= 0)
            {
                currentX += displayNameFixedWidth;
            }
            else
            {
                currentX += font.MeasureString(GetDisplayName()).ToPoint().X;
            }

            Game1.spriteBatch.DrawString(font, "[", new Vector2(currentX, y), mainColor);
            currentX += font.MeasureString("[").ToPoint().X;

            if (hideWhenNotFocused && !isFocused)
            {
                Game1.spriteBatch.DrawString(font, FormatTextToWidth(new string('*', value.Length), valueDisplayWidth), new Vector2(currentX, y), mainColor);
            }
            else
            {
                Game1.spriteBatch.DrawString(font, FormatTextToWidth(value, valueDisplayWidth), new Vector2(currentX, y), mainColor);
            }
            currentX += valueDisplayWidth;

            Game1.spriteBatch.DrawString(font, "]", new Vector2(currentX, y), mainColor);
        }

        /// <summary>
        /// Implements <see cref="IMenuItem.GetSize"/> to get the size of the text input
        /// </summary>
        public Point GetSize()
        {
            Point size = font.MeasureString("[]").ToPoint();
            if (displayNameFixedWidth >= 0)
            {
                size.X += displayNameFixedWidth;
            }
            else
            {
                size.X += font.MeasureString(GetDisplayName()).ToPoint().X;
            }
            size.X += valueDisplayWidth;
            return size;
        }
    }
}
