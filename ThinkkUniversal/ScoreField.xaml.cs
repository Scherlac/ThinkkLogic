using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ThinkkCommon;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class ScoreField : UserControl
    {
        private States State;

        public ScoreField()
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

            this.Hole.Opacity = hidden ? 0.0 : 1.0;

            var c = color & States.ColorMask;

            this.Button.Opacity = c == States.Empty || hidden ? 0.0 : 1.0;

            if (c != States.Empty)
            {
                var colorName = c.ToString();
                if (MainPage.ColorNames.ContainsKey(colorName))
                {
                    this.ButtonGradientStop.Color = MainPage.ColorNames[colorName];
                }
            }

        }
    }
}
