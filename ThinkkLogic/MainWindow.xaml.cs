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
using ThinkkCommon;

namespace ThinkkLogic
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Combination Puzzle;
        private List<Combination> Attempts;

        private int CurrentAttempt;
        private GameOver GameOverWindow;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }



        private void NewGame()
        {
            var height = 60;
            var attemptsCount = 10;

            Puzzle = Combination.CreatePuzzle(1);
            Puzzle.Height = height;
            Puzzle.Hide();
            PuzzleView.Children.Clear();
            PuzzleView.Children.Add(Puzzle);

            AttemptsView.Children.Clear();
            Attempts = new List<Combination>(attemptsCount);
            for (var i = 0; i < attemptsCount; i++)
            {
                var a = new Combination();
                Attempts.Add(a);
                a.Height = height;
                a.Empty();
                AttemptsView.Children.Add(a);
            }

            SetAttempt(0);
        }

        private void SetAttempt(int attempt)
        {
            if (attempt >= Attempts.Count)
            {
                // No more attempt
                GameOver();
                return;
            }

            Attempts[attempt].Activate(new RelayCommand(
                (o) =>
                {
                    return true;
                },
                (o) =>
                {
                    var c = attempt;
                    var won = Attempts[c].Evaluate(Puzzle);
                    Evaluate(o);
                    Attempts[c].Disable();

                    if (won)
                    {
                        YouWon();
                    }
                    else
                    {
                        SetAttempt(c + 1);

                    }
                }
                ));

        }

        private void YouWon()
        {
            Puzzle.Show();

            GameOverWindow = new GameOver(new RelayCommand(
                (o) => { return true; },
                (o) =>
                {
                    NewGame();
                    GameOverWindow.Hide();
                    GameOverWindow.Close();
                    GameOverWindow = null;
                }
                ));

            GameOverWindow.Won();
            GameOverWindow.Show();

            GameOverWindow.Play();


        }

        private void GameOver()
        {
            Puzzle.Show();

            GameOverWindow = new GameOver(new RelayCommand(
                (o) => { return true; },
                (o) =>
                {
                    NewGame();
                    GameOverWindow.Hide();
                    GameOverWindow.Close();
                    GameOverWindow = null;
                }
                ));

            GameOverWindow.Lose();
            GameOverWindow.Show();

            GameOverWindow.Play();

        }

        private void Evaluate(object o)
        {
            //throw new NotImplementedException();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (null != GameOverWindow)
            {
                GameOverWindow.Close();
                GameOverWindow = null;
            }

            base.OnClosed(e);
        }
    }


}

