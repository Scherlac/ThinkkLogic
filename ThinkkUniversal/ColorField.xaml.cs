using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using ThinkkCommon;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ThinkUniversal
{

    public sealed partial class ColorField : UserControl
    {
        private States State;
        private ICommand FlipCommand;

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

            this.Text.Opacity = hidden ? 1.0 : 0.0;
            this.Hole.Opacity = hidden ? 0.0 : 1.0;

            var c = color & States.ColorMask;

            this.Button.Opacity = c == States.Empty || hidden ? 0.0 : 1.0;

            if (c != States.Empty)
            {
                var colorName = c.ToString();
                // FIXME
                this.ButtonGradientStop.Color = MainPage.ColorNames[colorName];
            }

        }

        internal void Activate(ICommand relayCommand)
        {
            // FIXME
            //this.FlipCommand.Command = relayCommand;
            FlipCommand = relayCommand;
            this.Field.IsDoubleTapEnabled = false;
            this.Field.Tapped += Field_Tapped;
        }

        private void Field_Tapped(object sender, TappedRoutedEventArgs e)
        {
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
            // FIXME:
            //if (null == this.FlipCommand)
            //{
            //    return;
            //}

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

            // FIXME
            //this.FlipCommand.Command = null;
            FlipCommand = null;
            this.Field.IsDoubleTapEnabled = false;
            this.Field.Tapped -= Field_Tapped;

        }
    }
}
