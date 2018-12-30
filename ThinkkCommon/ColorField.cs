using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThinkkCommon
{
    public class ColorField
    {
        public States State;
        private ICommand FlipCommand;
        public double TextOpacity { get; private set; }
        public double HoleOpacity { get; private set; }
        public double ButtonOpacity { get; private set; }
        public Colors ButtonGradientStopColor { get; private set; }

        public ColorField()
        {
            State = States.Unknown;
            Update(States.Empty);
        }

        public void Hide()
        {
            Update(State.AddFlag(States.Hidden));
        }

        public void Show()
        {
            Update(State.RemoveFlag(States.Hidden));
        }

        public void Update(States color)
        {
            if (State == color)
            {
                return;
            }

            State = color;

            var hidden = color.HasFlag(States.Hidden);
            var c = color & States.ColorMask;

            this.TextOpacity = hidden ? 1.0 : 0.0;
            this.HoleOpacity = c != States.Empty || hidden ? 0.0 : 1.0;
            this.ButtonOpacity = c == States.Empty || hidden ? 0.0 : 0.8;

            if (c != States.Empty)
            {
                var colorName = c.ToString();

                ButtonGradientStopColor = GameBoard.ColorNames[colorName];
            }

        }

        internal void Activate(ICommand relayCommand)
        {
            FlipCommand = relayCommand;
        }

        public void Field_Tapped() //object sender, TappedRoutedEventArgs e)
        {
            var sender = default(object);
            var ce = FlipCommand?.CanExecute(sender);
            if (ce.GetValueOrDefault(false))
            {
                FlipCommand.Execute(sender);
            }
        }

        public bool Evaluate(ColorField field)
        {
            var toTest = field.State;
            return ((State ^ toTest) & States.ColorMask) == 0;
        }

        public void Flip()
        {
            var s = (States)((uint)(State + 1) % (uint)(States.ColorMod));
            Update(s);
        }

        internal void Disable()
        {
            FlipCommand = null;
        }

    }
}
