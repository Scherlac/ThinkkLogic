using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThinkkLogic
{
    /// <summary>
    /// Interaction logic for ColorField.xaml
    /// </summary>
    public partial class ColorField : UserControl
    {
        private States State;

        public ColorField()
        {
            State = States.Unknown;
            InitializeComponent();
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

            this.Text.Visibility = hidden ? Visibility.Visible : Visibility.Hidden;
            this.Hole.Visibility = hidden ? Visibility.Hidden : Visibility.Visible;

            var c = color & States.ColorMask;

            this.Button.Visibility = c == States.Empty || hidden ? Visibility.Hidden : Visibility.Visible;

            if (c != States.Empty)
            {
                var colorName = c.ToString();
                this.ButtonGradientStop.Color = (Color)ColorConverter.ConvertFromString(colorName);
            }

        }

        internal void Activate(ICommand relayCommand)
        {
            this.FlipCommand.Command = relayCommand;
        }

        public bool Evaluate(ColorField field)
        {
            var toTest = field.State;
            return ((State ^ toTest) & States.ColorMask) == 0;
       }

        public void Flip()
        {
            if (null == this.FlipCommand)
            {
                return;
            }

            if (State.HasFlag(States.Hidden | States.RightColor | States.RigthPlace))
            {
                return;
            }

            var s = (States)((uint)(State + 1) % (uint)(States.ColorMod));
            Update(s);
        }

        internal void Disable()
        {
            //var rc = this.FlipCommand.Command as RelayCommand;
            //rc.Disable();

            this.FlipCommand.Command = null;

        }
    }

}
