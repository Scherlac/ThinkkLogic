using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using ThinkkCommon;

namespace ThinkkLogic
{
    /// <summary>
    /// Interaction logic for GameOver.xaml
    /// </summary>
    public partial class GameOver : Window
    {
        private Random Rnd;
        private bool Started;
        private bool NewGameTriggered;
        private ICommand RelayNewGame;

        public GameOver(ICommand newGame)
        {
            NewGameTriggered = false;
            InitializeComponent();
            Rnd = new Random();
            RelayNewGame = newGame;

            this.NewGameCommand.Command = new RelayCommand(
                (o) => { return RelayNewGame.CanExecute(o); },
                (o) =>
                {
                    NewGameTriggered = true;
                    RelayNewGame.Execute(o);
                }
                );

            this.ReplayCommand.Command = new RelayCommand(
                (o) =>
                {
                    return true;
                },
                (o) =>
                {
                    //Lose();
                    Play();
                }
                );

        }

        public void Play()
        {
            this.Media.Stretch = Stretch.Uniform;

            //if (Started)
            //{
            MediaEnded(null, null);
            //}
            //            this.Media
            this.Media.MediaEnded += new RoutedEventHandler(MediaEnded);

            Started = true;
            this.Media.Play();


        }

        protected override void OnClosed(EventArgs e)
        {
            if (!NewGameTriggered)
            {
                NewGameTriggered = true;
                RelayNewGame?.Execute(null);
                return;
            }

            base.OnClosed(e);
        }

        void MediaEnded(object sender, RoutedEventArgs e)
        {
            Started = false;
            this.Media.Stop();
            this.Media.Position = TimeSpan.FromSeconds(0);
        }


        internal void Lose()
        {
            var li = new List<String>() {
                @"Data\GameOver01.gif",
                @"Data\GameOver02.mp4",
                @"Data\GameOver03.gif",
                @"Data\GameOver04.mp4",
                @"Data\GameOver05.gif",
                ""
            };
            var x = Rnd.Next(0, 5);
            var fi = new FileInfo(li[x]);
            var fn = fi.FullName;
            this.Media.Source = new Uri(fn);
            this.Text.Text = "Game Over! Try again!";
            this.Text.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Red);
        }

        internal void Won()
        {
            var li = new List<String>() {
                @"Data\Won01.mp4",
                @"Data\Won02.mp4",
                @"Data\Won03.mp4",
                @"Data\Won04.mp4",
                @"Data\Won05.gif",
                ""
            };
            var x = Rnd.Next(0, 5);
            var fi = new FileInfo(li[x]);
            var fn = fi.FullName;
            this.Media.Source = new Uri(fn);

            this.Text.Text = "Game Over: You won!";
            this.Text.Foreground = new SolidColorBrush(System.Windows.Media.Colors.Yellow);
        }
    }
}
