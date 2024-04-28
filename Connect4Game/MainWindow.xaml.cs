using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Connect4Game.DataBase;

namespace Connect4Game
{
    public partial class MainWindow : Window
    {
        public string game_lacation;
        public string opponent;
        public Players Player_1;
        public Players Player_2;
        public Color ColorPlayer1;
        public Color ColorPlayer2;
        public DBOperations operationDB;

        public MainWindow()
        {
            InitializeComponent();

            this.Activated += MainWindow_Activated;
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            game_lacation = "Btn_Russia";
            opponent = "Btn_PC";

            ColorPlayer1 = Colors.Red;
            ColorPlayer2 = Colors.Yellow;

            operationDB = new DBOperations();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            Player_1 = new Players();//текущие игроки, при выходе в гл меню мы их обнуляем
            Player_2 = new Players();
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)//наведение мышки на кнопку
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();

            dropShadowEffect.Color = Colors.WhiteSmoke;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)//мышка вне кнопки меню
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();

            dropShadowEffect.Color = Colors.Transparent;
            dropShadowEffect.Opacity = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_Exit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Btn_Enter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Enter enter = new Enter();

            enter.Owner = this;
            enter.Show();
            this.Hide();
        }

        private void Btn_Registry_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Registration reg = new Registration();

            reg.Owner = this;
            reg.Show();
            this.Hide();
        }

        private void Btn_Edit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Edit edit = new Edit();

            edit.Owner = this;
            edit.Show();
            this.Hide();
        }

        private void Btn_Rating_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rating rating = new Rating();

            rating.Owner = this;
            rating.Show();
            this.Hide();
        }
    }
}
