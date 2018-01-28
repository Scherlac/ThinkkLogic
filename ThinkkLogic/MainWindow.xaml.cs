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


    [Flags]
    public enum States : uint
    {
        Empty = 0x00,
        Red = 0x01, 
        Yellow = 0x02,
        White = 0x03,
        Blue = 0x04,
        Green = 0x05,
        Black = 0x06,

        ColorMod =  0x07,

        ColorMask = 0x0F,

        Hidden = 0x10,
        RightColor = 0x20,
        RigthPlace = 0x40,
        Unknown = 0x80,
    }

    public class RelayCommand : ICommand
    {
        public Func<object, bool> Function { get; private set; }
        public Action<object> Action { get; private set; }

         public event EventHandler CanExecuteChanged;

        public RelayCommand(Func<object, bool> func, Action<object> act)
        {
            Function = func;
            Action = act;
        }

        //public void Disable()
        //{
        //    Function = (o) => { return false; };
        //    Action = null;
        //    CanExecuteChanged.Invoke(this, new EventArgs());
        //}

        public bool CanExecute(object parameter)
        {
            return Function.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            Action?.Invoke(parameter);
        }
    }

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

        private void Evalueate(object o)
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


    public static class LocalExtensions
    {
        public static T RemoveFlag<T>(this Enum value, T f)
        {
            if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                var vv = (uint)(object)value;
                var ff = (uint)(object)f;

                return (T)(object)(vv & ~ff);
            }

            throw new ArgumentException(string.Format("Invalid argument: Cannot convert type {0} to {1}", value.GetType(), typeof(T)));
        }

        public static T AddFlag<T>(this Enum value, T f)
        {
            if (typeof(T).IsAssignableFrom(value.GetType()))
            {
                var vv = (uint)(object)value;
                var ff = (uint)(object)f;

                return (T)(object)(vv | ff);
            }

            throw new ArgumentException(string.Format("Invalid argument: Cannot convert type {0} to {1}", value.GetType(), typeof(T)));
        }


    }
}

