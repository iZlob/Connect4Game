using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using Connect4Game.DataBase;

namespace Connect4Game
{

    public partial class Registration : Window
    {
        private MainWindow _main;
        private bool _isOkPassword;
        public Registration()
        {
            InitializeComponent();

            this.Loaded += Registration_Loaded;
        }

        private void Registration_Loaded(object sender, RoutedEventArgs e)
        {
            _main = this.Owner as MainWindow;

            HaveYetName.Visibility = Visibility.Hidden;
            _isOkPassword = false;
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();

            dropShadowEffect.Color = Colors.Red;
            dropShadowEffect.BlurRadius = 30;
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

        private void Btn_Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void Btn_Save_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_main.operationDB.CheckName(Name.Text))
            {
                HaveYetName.Visibility = Visibility.Visible;
                Name.Text = "";

                return;
            }
            else
            {
                Players player = new Players();

                player.Name = Name.Text;
                player.Password = Password.Password;
                _main.operationDB.AddPlayer(player);

                if (_main.Player_1.Name == "")
                {
                    _main.Player_1.Name = Name.Text;
                }
                else if (_main.Player_2.Name == "" && _main.opponent == "Btn_Friend")
                {
                    _main.Player_2.Name = Name.Text;
                }
            }

            //если оппонент пк то сразу в игру, если друг то регистрация или вход
            if ((_main.opponent == "Btn_PC" && _isOkPassword) || (_main.opponent == "Btn_Friend" && _isOkPassword && _main.Player_2.Name != ""))
            {
                if (_main.Player_2.Name == "")
                {
                    _main.Player_2.Name = "Компьютер";
                }

                Game game = new Game();

                game.Owner = _main;
                game.Show();
                this.Close();
            }
            else if (_main.opponent == "Btn_Friend" && _isOkPassword)
            {
                SecondPlayer player2 = new SecondPlayer();

                player2.Owner = _main;
                player2.Show();
                this.Close();
            }
        }

        private void Name_GotFocus(object sender, RoutedEventArgs e)
        {
            HaveYetName.Visibility = Visibility.Hidden;
        }

        private void PasswordRepeat_PasswordChanged(object sender, RoutedEventArgs e)
        {
            int ind = 0;

            if (Password.Password.Length == PasswordRepeat.Password.Length)
            {
                foreach (var ch in PasswordRepeat.Password)
                {
                    if (Password.Password[ind] == ch)
                    {
                        PasswordRepeat.Foreground = new SolidColorBrush(Colors.Black);
                        _isOkPassword = true;
                    }
                    else
                    {
                        PasswordRepeat.Foreground = new SolidColorBrush(Colors.Red);
                        _isOkPassword = false;

                        break;
                    }

                    ind++;
                }
            }
            else
            {
                PasswordRepeat.Foreground = new SolidColorBrush(Colors.Red);
                _isOkPassword = false;
            }
        }
    }
}
