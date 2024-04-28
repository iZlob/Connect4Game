using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Connect4Game
{
    public partial class Edit : Window
    {
        private string _game_location;
        private string _opponent;
        private Color _player1;
        private Color _player2;
        private bool _isFirstLoad;
        private MainWindow _main;

        public Edit()
        {
            InitializeComponent();

            _isFirstLoad = true;
            this.Activated += Edit_Activated;
        }

        private void Edit_Activated(object sender, EventArgs e)
        {
            _main = this.Owner as MainWindow;

            if (_isFirstLoad)
            {
                _game_location = _main.game_lacation;
                _opponent = _main.opponent;
                _player1 = _main.ColorPlayer1;
                _player2 = _main.ColorPlayer2;
                Color_Player1.SelectedBrush = new SolidColorBrush(_player1);
                Color_Player2.SelectedBrush = new SolidColorBrush(_player2);
                ColorPanel.Opacity = 0;
                _isFirstLoad = false;
            }
        }

        //наведение мышки на кнопку
        private void Btn_MouseEnter(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = GetEffect(Colors.WhiteSmoke, 10, 0);

            if (img.Name != _game_location && img.Name != _opponent)
                img.Effect = dropShadowEffect;
        }

        private DropShadowEffect GetEffect(Color color, double blur, double depth)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();
            dropShadowEffect.Color = color;
            dropShadowEffect.BlurRadius = blur;
            dropShadowEffect.ShadowDepth = depth;

            return dropShadowEffect;
        }

        //мышка вне кнопки меню
        private void Btn_MouseLeave(object sender, MouseEventArgs e)
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = DeleteEffect(Colors.Transparent, 0);

            if (img.Name != _game_location && img.Name != _opponent)
                img.Effect = dropShadowEffect;
        }

        private DropShadowEffect DeleteEffect(Color color, double opacity)
        {
            DropShadowEffect dropShadowEffect = new DropShadowEffect();

            dropShadowEffect.Color = color;
            dropShadowEffect.Opacity = opacity;

            return dropShadowEffect;
        }

        private void Btn_Exit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Owner.Show();
            this.Close();
        }

        private void Btn_Color_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            double opacityFrom = 0;
            double opacityTo = 1;
            Duration duration = new Duration(TimeSpan.FromSeconds(1));
            DoubleAnimation animation;

            if (ColorPanel.Opacity == 0)
            {
                animation = new DoubleAnimation(opacityFrom, opacityTo, duration);
            }
            else
            {
                animation = new DoubleAnimation(opacityTo, opacityFrom, duration);
            }

            ColorPanel.BeginAnimation(OpacityProperty, animation);
        }

        private void Btn_Location_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var width = SystemParameters.PrimaryScreenWidth;
            double xEu = 3 * width / 4;
            double xUsa = 2 * width / 4;
            double xRussia = width / 4;
            double leftInit = 20;
            Thickness thicknessFrom = new Thickness(leftInit, 0, 0, 10);
            Thickness thicknessToRussia = new Thickness(xRussia, 0, 0, 10);
            Thickness thicknessToUsa = new Thickness(xUsa, 0, 0, 10);
            Thickness thicknessToEu = new Thickness(xEu, 0, 0, 10);
            Duration duration = new Duration(TimeSpan.FromSeconds(1));
            ThicknessAnimation AnimationRussia;
            ThicknessAnimation AnimationUsa;
            ThicknessAnimation AnimationEu;

            if (Btn_Russia.Margin.Left == leftInit)
            {
                AnimationRussia = new ThicknessAnimation(thicknessFrom, thicknessToRussia, duration);
                AnimationUsa = new ThicknessAnimation(thicknessFrom, thicknessToUsa, duration);
                AnimationEu = new ThicknessAnimation(thicknessFrom, thicknessToEu, duration);

                AnimationRussia.Completed += Button_RideOutLocation;
            }
            else
            {
                AnimationRussia = new ThicknessAnimation(thicknessToRussia, thicknessFrom, duration);
                AnimationUsa = new ThicknessAnimation(thicknessToUsa, thicknessFrom, duration);
                AnimationEu = new ThicknessAnimation(thicknessToEu, thicknessFrom, duration);

                AnimationRussia.CurrentStateInvalidated += Button_ComeLocation;
            }

            Btn_Russia.BeginAnimation(MarginProperty, AnimationRussia);
            Btn_Usa.BeginAnimation(MarginProperty, AnimationUsa);
            Btn_Eu.BeginAnimation(MarginProperty, AnimationEu);
        }

        private void Btn_Opponent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var width = SystemParameters.PrimaryScreenWidth;
            double xPC = 2 * width / 4;
            double xFriend = width / 4;
            double leftInit = 20;
            Thickness thicknessFrom = new Thickness(leftInit, 0, 0, 10);
            Thickness thicknessToPC = new Thickness(xPC, 0, 0, 10);
            Thickness thicknessToFriend = new Thickness(xFriend, 0, 0, 10);
            Duration duration = new Duration(TimeSpan.FromSeconds(1));
            ThicknessAnimation AnimationPC;
            ThicknessAnimation AnimationFriend;

            if (Btn_PC.Margin.Left == leftInit)
            {
                AnimationPC = new ThicknessAnimation(thicknessFrom, thicknessToPC, duration);
                AnimationFriend = new ThicknessAnimation(thicknessFrom, thicknessToFriend, duration);

                AnimationPC.Completed += Button_RideOutOpponent;
            }
            else
            {
                AnimationPC = new ThicknessAnimation(thicknessToPC, thicknessFrom, duration);
                AnimationFriend = new ThicknessAnimation(thicknessToFriend, thicknessFrom, duration);

                AnimationPC.CurrentStateInvalidated += Button_ComeOpponent;
            }

            Btn_PC.BeginAnimation(MarginProperty, AnimationPC);
            Btn_Friend.BeginAnimation(MarginProperty, AnimationFriend);
        }

        private void Button_ComeLocation(object sender, EventArgs e)
        {
            DropShadowEffect dropShadowEffect = DeleteEffect(Colors.Transparent, 0);

            SetButtonEffectLocation(dropShadowEffect);
        }

        private void Button_RideOutLocation(object sender, EventArgs e)
        {
            DropShadowEffect dropShadowEffect = GetEffect(Colors.Red, 50, 0);

            SetButtonEffectLocation(dropShadowEffect);
        }

        private void Button_ComeOpponent(object sender, EventArgs e)
        {
            DropShadowEffect dropShadowEffect = DeleteEffect(Colors.Transparent, 0);

            SetButtonEffectOpponent(dropShadowEffect);
        }

        private void Button_RideOutOpponent(object sender, EventArgs e)
        {
            DropShadowEffect dropShadowEffect = GetEffect(Colors.Red, 50, 0);

            SetButtonEffectOpponent(dropShadowEffect);
        }

        private void SetButtonEffectLocation(DropShadowEffect effect)
        {
            switch (_game_location)
            {
                case "Btn_Russia":
                    {
                        Btn_Russia.Effect = effect;
                        break;
                    }
                case "Btn_Usa":
                    {
                        Btn_Usa.Effect = effect;
                        break;
                    }
                case "Btn_Eu":
                    {
                        Btn_Eu.Effect = effect;
                        break;
                    }
                default: break;
            }
        }

        private void SetButtonEffectOpponent(DropShadowEffect effect)
        {
            switch (_opponent)
            {
                case "Btn_PC":
                    {
                        Btn_PC.Effect = effect;
                        break;
                    }
                case "Btn_Friend":
                    {
                        Btn_Friend.Effect = effect;
                        break;
                    }
                default: break;
            }
        }

        private void Choise_Location_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Image;
            var effect = DeleteEffect(Colors.Transparent, 0);

            SetButtonEffectLocation(effect);

            _game_location = btn.Name;
            effect = GetEffect(Colors.Red, 50, 0);

            SetButtonEffectLocation(effect);
        }

        private void Choise_Opponent_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Image;
            var effect = DeleteEffect(Colors.Transparent, 0);

            SetButtonEffectOpponent(effect);

            _opponent = btn.Name;
            effect = GetEffect(Colors.Red, 50, 0);

            SetButtonEffectOpponent(effect);
        }

        private void Btn_Save_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _main.game_lacation = _game_location;
            _main.opponent = _opponent;
            _main.ColorPlayer1 = Color_Player1.SelectedBrush.Color;
            _main.ColorPlayer2 = Color_Player2.SelectedBrush.Color;
        }
    }
}
