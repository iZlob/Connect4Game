using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Connect4Game
{    
    public partial class Enter : Window
    {
        private MainWindow _main;

        public Enter()
        {
            InitializeComponent();

            this.Loaded += Enter_Loaded;
        }

        private void Enter_Loaded(object sender, RoutedEventArgs e)
        {
            _main = this.Owner as MainWindow;
            mistakeText.Visibility = Visibility.Hidden;          
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.WhiteSmoke;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Transparent;
            dropShadowEffect.Opacity = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_Close_MouseEnter(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Red;
            dropShadowEffect.BlurRadius = 30;
            dropShadowEffect.ShadowDepth = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_Close_MouseLeave(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = Colors.Transparent;
            dropShadowEffect.Opacity = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _main.Show();
            this.Close();
        }

        private void Btn_OK_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_main.operationDB.CheckNameAndPassword(Name.Text, Password.Password))
            {
                mistakeText.Visibility = Visibility.Visible;
                Name.Text = "";
                Password.Password = "";
                return;
            }
            else
            {
                if (_main.Player_1.Name == "")
                {
                    _main.Player_1.Name = Name.Text;
                }
                else if (_main.Player_2.Name == "" && _main.opponent == "Btn_Friend")
                {
                    _main.Player_2.Name = Name.Text;
                }
            }

            if (_main.opponent == "Btn_PC" || (_main.opponent == "Btn_Friend" && _main.Player_2.Name != ""))
            {
                if (_main.opponent == "Btn_PC")
                {
                    _main.Player_2.Name = "Компьютер";
                }

                if (!_main.operationDB.CheckName(_main.Player_2.Name))
                {
                    _main.operationDB.AddPlayer(_main.Player_2);
                }

                Game game = new Game();
                game.Owner = _main;
                game.Show();
                this.Close();
            }
            else if (_main.opponent == "Btn_Friend")
            {
                SecondPlayer player2 = new SecondPlayer();
                player2.Owner = _main;
                player2.Show();
                this.Close();
            }            
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            mistakeText.Visibility = Visibility.Hidden;
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            mistakeText.Visibility = Visibility.Hidden;
        }
    }
}
