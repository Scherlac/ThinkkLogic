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
    /// Interaction logic for Combination.xaml
    /// </summary>
    public partial class Combination : UserControl
    {
        private List<ColorField> Places;
        private List<ScoreField> Scores;
        private Color BaseColor;

        public Combination()
        {
            InitializeComponent();
            BaseColor = (Color)ColorConverter.ConvertFromString("#00E0DECF");

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

            used.Add(States.Empty);
            used.Add(States.Black);

            switch (level)
            {
                default:
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
            }

            return c;
        }

        public void Hide()
        {
            ScoreView.Visibility = Visibility.Hidden;
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
            this.Evaluate.Command = eval;
            BaseColor.ScA = 0.3F;
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

        public void Disable()
        {
            this.Evaluate.Command = null;

            BaseColor.ScA = 0.0F;
            this.Back.Background = new SolidColorBrush(BaseColor);
            foreach (var pl in Places)
            {
                pl.Disable();
            }
        }


        public bool Evalueate(Combination puzzle)
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
                    li.Add(States.White | States.RigthPlace | States.RightColor);
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
                        li.Add(States.Black | States.RightColor);
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
