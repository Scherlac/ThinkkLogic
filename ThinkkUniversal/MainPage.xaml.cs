using System.Collections.Generic;
using System.Reflection;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using ThinkkCommon;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ThinkUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Dictionary<string, Color> ColorNames;

        static MainPage()
        {
            ColorNames = new Dictionary<string, Color>();
            foreach (var color in typeof(Colors).GetRuntimeProperties())
            {
                ColorNames.Add(color.Name, (Color)color.GetValue(null));
            }

        }

        private Combination Puzzle;
        private List<Combination> Attempts;

        private int CurrentAttempt;
        // FIXME
        //private GameOver GameOverWindow;

        public MainPage()
        {
            InitializeComponent();
            NewGame();
        }

        private void NewGame()
        {
            var height = 55;
            var attemptsCount = 10;

            Puzzle = Combination.CreatePuzzle(2);
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
                    var won = Attempts[c].Evalueate(Puzzle);
                    Evalueate(o);
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
            this.PuzzleView.DoubleTapped += AttemptsView_DoubleTapped;
    
            // FIXME
            //GameOverWindow = new GameOver(new RelayCommand(
            //    (o) => { return true; },
            //    (o) =>
            //    {
            //        NewGame();
            //        GameOverWindow.Hide();
            //        GameOverWindow.Close();
            //        GameOverWindow = null;
            //    }
            //    ));

            //GameOverWindow.Won();
            //GameOverWindow.Show();

            //GameOverWindow.Play();


        }

        private void AttemptsView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            NewGame();
            this.PuzzleView.DoubleTapped -= AttemptsView_DoubleTapped;
        }

        private void GameOver()
        {
            Puzzle.Show();

            // FIXME
            //GameOverWindow = new GameOver(new RelayCommand(
            //    (o) => { return true; },
            //    (o) =>
            //    {
            //        NewGame();
            //        GameOverWindow.Hide();
            //        GameOverWindow.Close();
            //        GameOverWindow = null;
            //    }
            //    ));

            //GameOverWindow.Lose();
            //GameOverWindow.Show();

            //GameOverWindow.Play();

        }

        private void Evalueate(object o)
        {
            //throw new NotImplementedException();
        }

        // FIXME
        //protected override void OnClosed(EventArgs e)
        //{
        //    if (null != GameOverWindow)
        //    {
        //        GameOverWindow.Close();
        //        GameOverWindow = null;
        //    }

        //    base.OnClosed(e);
        //}

    }
}
