using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkkCommon
{
    public class GameBoard : NotifyPropertyChangedBase
    {
        public static Dictionary<string, Colors> ColorNames;
        protected const int attemptsCount = 10;
        protected const int height = 55;

        public int _Level;
        public int Level
        {
            set { SetField(ref _Level, value); }
            get { return _Level; }
        }

        public string _Hint;
        public string Hint
        {
            set { SetField(ref _Hint, value); }
            get { return _Hint; }
        }

        static GameBoard()
        {
            ColorNames = new Dictionary<string, Colors>();
            foreach (var color in  Enum.GetValues(typeof(Colors)))
            {
                ColorNames.Add(Enum.GetName(typeof(Colors), color), (Colors)color);
            }
        }

        public GameBoard()
        {
            Level = 1;
            Attempts = new List<Combination>(attemptsCount);

            for (var i = 0; i < attemptsCount; i++)
            {
                var a = new Combination();
                a.Height = height;
                a.Empty();
                Attempts.Add(a);
            }

            Puzzle = Combination.CreatePuzzle(Level);

        }

        public Combination Puzzle;
        public List<Combination> Attempts;
        public int CurrentAttemptIndex;
        public Combination CurrentAttempt
        {
            get
            {
                try
                {
                    return Attempts?[CurrentAttemptIndex];
                }
                catch
                {
                    return null;
                }
            }
        }

        public event EventHandler OnWon;
        public event EventHandler OnNewGame;

        public void Refresh()
        {
        }

        public async void NewGame()
        {
            var aa = Attempts;
            var level = Level;
            Puzzle?.Hide();
            Attempts = null;
            
            var p = Combination.CreatePuzzle(level);
            p.Height = height;
            p.Hide();

            foreach (var a in aa)
            {
                a.Empty();
            }

            Attempts = aa;
            Puzzle = p;

            OnWon?.BeginInvoke(this, null, null, null);

            CurrentAttemptIndex = 0;
            SetAttempt(0);

        }

        private void SetAttempt(int attempt)
        {
            Attempts[CurrentAttemptIndex].Disable();

            if (attempt < 0)
            {
                return;
            }
            if (attempt >= Attempts.Count)
            {
                return;
            }

            CurrentAttemptIndex = attempt;
            Attempts[attempt].Activate(null);

            //new RelayCommand(
                //(o) =>
                //{
                //    return true;
                //},
                //(o) =>
                //{
                //    var c = attempt;
                //    var won = Attempts[c].Evaluate(Puzzle);
                //    Evaluate(o);
                //    Attempts[c].Disable();

                //    if (won)
                //    {
                //        YouWon();
                //    }
                //    else
                //    {
                //        SetAttempt(c + 1);

                //    }
                //}
                //));

        }

        private void YouWon()
        {
            OnWon?.BeginInvoke(this, null, null, null);
            Puzzle.Show();
        }

        private void GameOver()
        {
            Puzzle.Show();

        }

        public void Evaluate()
        {
            var won = Attempts[CurrentAttemptIndex].Evaluate(Puzzle);
            var next_attempt = CurrentAttemptIndex + 1;


            if (won)
            {
                next_attempt = 10;
                YouWon();
            }
            else
            {
                if (next_attempt >= attemptsCount)
                {
                    // No more attempt
                    GameOver();
                }
            }
            SetAttempt(next_attempt);
        }


    }
}
