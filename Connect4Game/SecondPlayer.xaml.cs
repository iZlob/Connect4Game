using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Connect4Game
{
    public partial class SecondPlayer : Window
    {
        private MainWindow _main;

        public SecondPlayer()
        {
            InitializeComponent();

            this.Loaded += SecondPlayer_Loaded;
        }

        private void SecondPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            _main = this.Owner as MainWindow;            
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

        private void Btn_Enter_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Enter enter = new Enter();

            enter.Owner = _main;
            enter.Show();
            this.Close();
        }

        private void Btn_Registry_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Registration reg = new Registration();

            reg.Owner = _main;
            reg.Show();
            this.Close();
        }

        private void Btn_Close_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _main.Show();
            this.Close();
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
    }
}
