using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThinkkCommon
{
    public class GameBoard
    {
        public static Dictionary<string, Colors> ColorNames;
        protected const int attemptsCount = 10;
        protected const int height = 55;
        public string Hint = ""; 

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
            Attempts = new List<Combination>(attemptsCount);

            for (var i = 0; i < attemptsCount; i++)
            {
                var a = new Combination();
                a.Height = height;
                a.Empty();
                Attempts.Add(a);
            }

            Puzzle = Combination.CreatePuzzle(1);

        }

        public Combination Puzzle;
        public List<Combination> Attempts;
        public int CurrentAttempt;

        public event EventHandler OnWon;
        public event EventHandler OnNewGame;

        public void Refresh()
        {
        }

        public async void NewGame(int level)
        {
            var aa = Attempts;
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

            CurrentAttempt = 0;
            SetAttempt(0);

        }

        private void SetAttempt(int attempt)
        {
            Attempts[CurrentAttempt].Disable();

            if (attempt < 0)
            {
                return;
            }
            if (attempt >= Attempts.Count)
            {
                return;
            }

            CurrentAttempt = attempt;
            Attempts[attempt].Activate(null);

            //new RelayCommand(
                //(o) =>
                //{
                //    return true;
                //},
                //(o) =>
                //{
                //    var c = attempt;
                //    var won = Attempts[c].Evalueate(Puzzle);
                //    Evalueate(o);
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

        public void Evalueate()
        {
            var won = Attempts[CurrentAttempt].Evalueate(Puzzle);
            var next_attempt = CurrentAttempt + 1;


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
