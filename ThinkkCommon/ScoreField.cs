using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkkCommon
{
    public class ScoreField
    {
        public States State;
        public double HoleOpacity { get; private set; }
        public double ButtonOpacity { get; private set; }
        public Colors ButtonGradientStopColor { get; private set; }


        public ScoreField()
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

            this.HoleOpacity = hidden ? 0.0 : 1.0;
            this.ButtonOpacity = c == States.Empty || hidden ? 0.0 : 1.0;

            if (c != States.Empty)
            {
                var colorName = c.ToString();
                if (GameBoard.ColorNames.ContainsKey(colorName))
                {
                    ButtonGradientStopColor = GameBoard.ColorNames[colorName];
                }
            }

        }

    }
}
