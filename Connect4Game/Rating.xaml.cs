using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Connect4Game
{
    public partial class Rating : Window
    {
        private MainWindow _main;
        public Rating()
        {
            InitializeComponent();            
            
            this.Loaded += Rating_Loaded;
        }

        private void Rating_Loaded(object sender, RoutedEventArgs e)
        {
            _main = this.Owner as MainWindow;

            var champions = _main.operationDB.GetChampions();

            Table.ItemsSource = champions;
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
    }
}
