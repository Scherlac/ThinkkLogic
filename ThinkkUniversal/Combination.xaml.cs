using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
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
    public sealed partial class Combination : UserControl
    {
        private List<ColorField> Places;
        private List<ScoreField> Scores;
        private Color BaseColor;
        private ICommand DoubleTappedCommand;

        public Combination()
        {
            InitializeComponent();
            // FIXME
            //BaseColor = (Color)ColorConverter.ConvertFromString("#00E0DECF");

            Scores = new List<ScoreField>() { S1, S2, S3, S4 };
            Places = new List<ColorField>() { C1, C2, C3, C4 };

        }

        public static Combination CreatePuzzle(int level)
        {
            var c = new Combination();
            c.Empty();
            c.Hide();

            var rnd = new Random();
            var used = new List<States>(6);


            switch (level)
            {
                case 1:

                    used.Add(States.Empty);
                    used.Add(States.Black);

                    foreach (var pl in c.Places)
                    {
                        var x = default(States);

                        do
                        {
                            x = (States)rnd.Next(0, 6);
                        } while (used.Contains(x));

                        used.Add(x);
                        pl.Update(x | States.Hidden);
                    }

                    break;
                case 2:

                    foreach (var pl in c.Places)
                    {
                        var x = default(States);

                        do
                        {
                            x = (States)rnd.Next(0, 6);
                        } while ( used.FindAll(s => s == x).Count >= 2 );

                        used.Add(x);
                        pl.Update(x | States.Hidden);
                    }

                    break;
            }

            return c;
        }

        public void Hide()
        {
            ScoreView.Opacity = 0.0;
            foreach (var pl in Places)
            {
                pl.Hide();
            }
        }

        public void Empty()
        {
            ScoreView.Visibility = Visibility.Visible;
            foreach (var pl in Places)
            {
                pl.Update(States.Empty);
            }

            foreach (var sc in Scores)
            {
                sc.Update(States.Empty);
            }
        }

        public void Activate(ICommand eval)
        {
            DoubleTappedCommand = eval;
            // FIXME
            //this.Evaluate.Command = eval;
            this.ScoreField.DoubleTapped += ScoreField_DoubleTapped;
            this.ScoreField.IsDoubleTapEnabled = true;

            BaseColor.A = 0x50; // 0.3F;
            this.Back.Background = new SolidColorBrush(BaseColor);
            foreach (var pl in Places)
            {
                pl.Activate(new RelayCommand(
                    (o) =>
                    {
                        return true;
                    },
                    (o) =>
                    {
                        pl.Flip();
                    }
                    ));
            }

        }

        private void ScoreField_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var ce = DoubleTappedCommand?.CanExecute(sender);
            if (ce.GetValueOrDefault(false))
            {
                DoubleTappedCommand.Execute(sender);
            }
        }


        public void Disable()
        {
            // FIXME
            //this.Evaluate.Command = null;
            DoubleTappedCommand = null;
            this.ScoreField.DoubleTapped -= ScoreField_DoubleTapped;
            this.ScoreField.IsDoubleTapEnabled = false;

            BaseColor.A = 0;

            this.Back.Background = new SolidColorBrush(BaseColor);
            foreach (var pl in Places)
            {
                pl.Disable();
            }
        }


        public bool Evaluate(Combination puzzle)
        {
            var li = new List<States>();
            var puzzleUsed = new List<int>(4);
            var thisUsed = new List<int>(4);
            //throw new NotImplementedException();

            // Evaluate Pass 1
            for (var i = 0; i < 4; i++)
            {
                var pl1 = puzzle.Places[i];
                var pl2 = Places[i];

                if (pl2.Evaluate(pl1))
                {
                    li.Add(States.Green | States.RigthPlace | States.RightColor);
                    puzzleUsed.Add(i);
                }
            }

            for (var i = 0; i < 4; i++)
            {
                if (puzzleUsed.Contains(i))
                {
                    continue;
                }

                var pl1 = puzzle.Places[i];

                for (var j = 0; j < 4; j++)
                {
                    if (puzzleUsed.Contains(j))
                    {
                        continue;
                    }

                    if (thisUsed.Contains(j))
                    {
                        continue;
                    }

                    var pl2 = Places[j];

                    if (pl2.Evaluate(pl1))
                    {
                        li.Add(States.Grey | States.RightColor);
                        thisUsed.Add(j);
                        break;
                    }
                }
            }

            var ret = li.Count == 4;
            var x = 0;

            foreach (var s in li)
            {
                if (!s.HasFlag(States.RigthPlace))
                {
                    ret = false;
                }

                Scores[x].Update(s);
                x++;
            }

            return ret;
        }

        internal void Show()
        {
            foreach (var pl in Places)
            {
                pl.Show();
            }
        }
    }
}
