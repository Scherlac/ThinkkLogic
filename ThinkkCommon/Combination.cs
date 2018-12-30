using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ThinkkCommon
{
    public class Combination
    {

        public double Height;

        public List<ColorField> Places;
        public List<ScoreField> Scores;
        //private Color BaseColor;
        private ICommand DoubleTappedCommand;
        private double ScoreViewOpacity;
        //private Visibility ScoreViewVisibility;

        public Combination()
        {
            // FIXME
            //BaseColor = (Color)ColorConverter.ConvertFromString("#00E0DECF");
            var S1 = new ScoreField();
            var S2 = new ScoreField();
            var S3 = new ScoreField();
            var S4 = new ScoreField();

            Scores = new List<ScoreField>() { S1, S2, S3, S4 };

            var C1 = new ColorField();
            var C2 = new ColorField();
            var C3 = new ColorField();
            var C4 = new ColorField();

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
            ScoreViewOpacity = 0.0;
            foreach (var pl in Places)
            {
                pl.Hide();
            }
        }

        public void Empty()
        {
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

            ScoreViewOpacity = 0.8;

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

        public void ScoreField_DoubleTapped()
        {
            var sender = default(object);

            var ce = DoubleTappedCommand?.CanExecute(sender);
            if (ce.GetValueOrDefault(false))
            {
                DoubleTappedCommand.Execute(sender);
            }
        }

        public void Disable()
        {
            DoubleTappedCommand = null;

            ScoreViewOpacity = 1.0;

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
