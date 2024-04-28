using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Connect4Game
{
    public partial class Game : Window
    {
        private MainWindow _main;
        private Ellipse _element;
        private Ellipse _animationElement;
        private Point _offset;
        private Ellipse[,] _matrixField;
        private bool _isWinPlayer1;
        private bool _isWinPlayer2;
        private bool _isPlayGame;
        private List<Ellipse> _chips;
        private DispatcherTimer _timerPlayer1;
        private DispatcherTimer _timerPlayer2;
        private Stopwatch _stopwatchPlayer1;
        private Stopwatch _stopwatchPlayer2;

        public Game()
        {
            InitializeComponent();

            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            _main = this.Owner as MainWindow;
            _main.Player_1.Score = 1000;
            _main.Player_2.Score = 1000;
            _animationElement = new Ellipse();
            _element = new Ellipse();
            _offset = new Point();
            _matrixField = new Ellipse[6, 7];
            _isWinPlayer1 = false;
            _isWinPlayer2 = false;
            _isPlayGame = true;
            _chips = new List<Ellipse>();
            _timerPlayer1 = new DispatcherTimer();
            _timerPlayer2 = new DispatcherTimer();
            _stopwatchPlayer1 = new Stopwatch();
            _stopwatchPlayer2 = new Stopwatch();

            _timerPlayer1.Interval = TimeSpan.FromSeconds(1);
            _timerPlayer1.Tick += TimerPlayer1_Tick;
            _timerPlayer2.Interval = TimeSpan.FromSeconds(1);
            _timerPlayer2.Tick += TimerPlayer2_Tick;
            _timerPlayer1.Start();
            _stopwatchPlayer1.Start();

            PanelPlayer2.Opacity = 0.3;
            Canvas.SetLeft(Field, SystemParameters.PrimaryScreenWidth / 2 - 300);
            Canvas.SetBottom(Field, SystemParameters.PrimaryScreenHeight / 2 - 200);

            double xL = 5;
            double xR = SystemParameters.PrimaryScreenWidth - 85;
            double y = 250;

            for (int i = 0; i < 21; i++)
            {
                CreateChipsPlayer(_main.ColorPlayer1, xL, y);
                CreateChipsPlayer(_main.ColorPlayer2, xR, y);

                y += 85;

                if (y > SystemParameters.PrimaryScreenHeight - 230)
                {
                    y = 250;
                    xL += 85;
                    xR -= 85;
                }
            }

            Player_1_Name.Text = _main.Player_1.Name;
            Player_2_Name.Text = _main.Player_2.Name;
            Player_1_Score.Text = "Набранных очков: " + _main.Player_1.Score;
            Player_2_Score.Text = "Набранных очков: " + _main.Player_2.Score;

            switch (_main.game_lacation)
            {
                case "Btn_Russia":
                    {
                        GameImage.ImageSource = new BitmapImage(new Uri("Images/RUSSIA.jpg", UriKind.Relative));
                        break;
                    }
                case "Btn_Usa":
                    {
                        GameImage.ImageSource = new BitmapImage(new Uri("Images/usa.jpg", UriKind.Relative));
                        break;
                    }
                case "Btn_Eu":
                    {
                        GameImage.ImageSource = new BitmapImage(new Uri("Images/eu.jpg", UriKind.Relative));
                        break;
                    }
                default: break;
            }
        }
        
        private void CreateChipsPlayer(Color chipsColor, double x, double y)//для создания 1й фишки
        {
            Ellipse ellipse = new Ellipse();

            ellipse.Width = 80;
            ellipse.Height = 80;
            ellipse.StrokeThickness = 2;
            ellipse.Stroke = new SolidColorBrush(Colors.Black);
            ellipse.Fill = new SolidColorBrush(chipsColor);
            ellipse.Cursor = Cursors.Hand;
            ellipse.PreviewMouseDown += Ellipse_PreviewMouseDown;

            Canvas.SetTop(ellipse, y);
            Canvas.SetLeft(ellipse, x);
            GameField.Children.Add(ellipse);
            _chips.Add(ellipse);
        }

        private void TimerPlayer1_Tick(object sender, EventArgs e)//отнимем очки у 1го игрока
        {
            if (_main.Player_1.Score > 0)
            {
                _main.Player_1.Score -= (int)(_stopwatchPlayer1.Elapsed.Seconds / 2) + 1;
            }
            else
            {
                _main.Player_1.Score = 0;
            }

            Player_1_Score.Text = "Набранных очков: " + _main.Player_1.Score;
        }

        private void TimerPlayer2_Tick(object sender, EventArgs e)//отнимаем очки у 2го игрока
        {
            if (_main.Player_2.Score > 0)
            {
                _main.Player_2.Score -= (int)(_stopwatchPlayer2.Elapsed.Seconds / 2) + 1;
            }
            else
            {
                _main.Player_2.Score = 0;
            }

            Player_2_Score.Text = "Набранных очков: " + _main.Player_2.Score;
        }

        private void Ellipse_PreviewMouseDown(object sender, MouseButtonEventArgs e)//когда захватываем фишку
        {
            if (_isPlayGame)
            {
                _element = sender as Ellipse;

                if (PanelPlayer1.Opacity == 1 && ((SolidColorBrush)_element.Fill).Color == _main.ColorPlayer1)
                {
                    _offset = e.GetPosition(GameField as IInputElement);
                    _offset.Y -= Canvas.GetTop(_element);
                    _offset.X -= Canvas.GetLeft(_element);
                    GameField.CaptureMouse();
                }
                else if (PanelPlayer2.Opacity == 1 && ((SolidColorBrush)_element.Fill).Color == _main.ColorPlayer2)
                {
                    _offset = e.GetPosition(GameField as IInputElement);
                    _offset.Y -= Canvas.GetTop(_element);
                    _offset.X -= Canvas.GetLeft(_element);
                    GameField.CaptureMouse();
                }
                else
                {
                    _element = null;
                }
            }
        }

        private void Game_PreviewMouseMove(object sender, MouseEventArgs e)//когда передвигаем фишку
        {
            if (_element == null)
            {
                return;
            }

            var position = e.GetPosition(sender as IInputElement);
            var newPositionX = position.X - _offset.X;
            var newPositionY = position.Y - _offset.Y;
            var leftLimit = SystemParameters.PrimaryScreenWidth / 2 - 380;
            var rightLimit = SystemParameters.PrimaryScreenWidth / 2 + 300;
            var topLimit = SystemParameters.PrimaryScreenHeight / 2 - 280;

            if ((newPositionX < leftLimit || newPositionX > rightLimit) && newPositionY > topLimit)
            {
                Canvas.SetTop(_element, newPositionY);
                Canvas.SetLeft(_element, newPositionX);
            }
            else if (newPositionY <= topLimit)
            {
                Canvas.SetTop(_element, newPositionY);
                Canvas.SetLeft(_element, newPositionX);
            }
        }

        private void Game_PreviewMouseUp(object sender, MouseButtonEventArgs e)//действия когда отпускаем фишку
        {
            int column = 0;
            int row = 0;
            _animationElement = null;

            if (_element != null)
            {
                _animationElement = _element;
                column = (int)((GetColumnChip(_animationElement) - (SystemParameters.PrimaryScreenWidth / 2 - 280)) / 80);
                row = (int)((GetStopRow(column) - (SystemParameters.PrimaryScreenHeight / 2 - 190)) / 90) + 1;
            }

            if (_animationElement != null && ChekChipsPosition(_animationElement) && _matrixField[0, column] == null)
            {
                DoubleAnimation correction = new DoubleAnimation()//корректировка фишки по центру
                {
                    From = Canvas.GetLeft(_animationElement),
                    To = GetColumnChip(_animationElement),
                    Duration = new Duration(TimeSpan.FromMilliseconds(300))
                };

                _animationElement.BeginAnimation(LeftProperty, correction);

                DoubleAnimation fall = new DoubleAnimation()//фишка падает
                {
                    From = Canvas.GetTop(_animationElement),
                    To = GetStopRow(column),
                    Duration = new Duration(TimeSpan.FromMilliseconds(150 * row))
                };

                fall.Completed += Fall_Completed;
                _animationElement.BeginAnimation(TopProperty, fall);
            }

            _element = null;
            GameField.ReleaseMouseCapture();
        }

        private void Fall_Completed(object sender, EventArgs e)//когда фишка упала (кроме фишки компа)
        {
            var column = (int)((GetColumnChip(_animationElement) - (SystemParameters.PrimaryScreenWidth / 2 - 280)) / 80);
            var row = (int)((GetStopRow(column) - (SystemParameters.PrimaryScreenHeight / 2 - 180)) / 90);

            _matrixField[row, column] = _animationElement;
            CheckWin();

            if (PanelPlayer1.Opacity == 1 && _isPlayGame)
            {
                PanelPlayer1.Opacity = 0.3;
                PanelPlayer2.Opacity = 1;
                _stopwatchPlayer1.Reset();
                _timerPlayer1.Stop();
                _timerPlayer2.Start();
                _stopwatchPlayer2.Start();

                if (_main.opponent == "Btn_PC")
                {
                    ComputerStep();
                }
            }
            else if (PanelPlayer2.Opacity == 1 && _isPlayGame)
            {
                PanelPlayer1.Opacity = 1;
                PanelPlayer2.Opacity = 0.3;
                _stopwatchPlayer2.Reset();
                _timerPlayer2.Stop();
                _timerPlayer1.Start();                
                _stopwatchPlayer1.Start();
            }
        }

        private bool ChekChipsPosition(Ellipse chip)//проверка нахождения фишки над центром колонки поля с точностью +-15 пикселей
        {
            var leftBoard = SystemParameters.PrimaryScreenWidth / 2 - 280;

            for (int i = 0; i < 7; i++)
            {
                if (Canvas.GetLeft(chip) > (leftBoard + i * 80 - 15) && Canvas.GetLeft(chip) < (leftBoard + i * 80 + 15))
                    return true;
            }

            return false;
        }

        private double GetColumnChip(Ellipse chip)//получает координаты колонки по горизонатли
        {
            var leftBoard = SystemParameters.PrimaryScreenWidth / 2 - 280;
            double pos = Canvas.GetLeft(chip);

            for (int i = 0; i < 7; i++)
            {
                if (pos > (leftBoard + i * 80 - 15) && pos < (leftBoard + i * 80 + 15))
                    pos = leftBoard + i * 80;
            }

            return pos;
        }        

        private double GetStopRow(int column)//получает координаты по вертикали для остановки фишки
        {
            var topRow = Canvas.GetTop(Field) + 36;
            double stopRow = 0;

            if (column >= 0 && column < 7)
            {
                for (int i = 5; i >= 0; i--)
                {
                    if (_matrixField[i, column] == null)
                    {
                        stopRow = topRow + i * 90;
                        break;
                    }
                }
            }

            return stopRow;
        }

        async private void ComputerStep()//ход компьютера
        {
            await Task.Run(() => Thread.Sleep(new Random().Next(0, 2000)));
            Ellipse chip = new Ellipse();

            for (int i = 0; i < _chips.Count; i++)
            {
                if (((SolidColorBrush)_chips[i].Fill).Color == _main.ColorPlayer2 && Canvas.GetLeft(_chips[i]) > SystemParameters.PrimaryScreenWidth / 2 + 300)
                {
                    chip = _chips[i];
                    break;
                }
            }

            var leftBoard = SystemParameters.PrimaryScreenWidth / 2 - 280;
            int column = 0;
            int row = -1;

            while (row == -1)
            {
                column = new Random().Next(0, 6);

                if (_matrixField[0, column] == null)
                {
                    row = (int)((GetStopRow(column) - (SystemParameters.PrimaryScreenHeight / 2 - 180)) / 90);
                }
                else
                {
                    row = -1;
                }
            }

            var position = leftBoard + column * 80;

            DoubleAnimation up = new DoubleAnimation()//движемся вверх
            {
                From = Canvas.GetTop(chip),
                To = 100,
                Duration = new Duration(TimeSpan.FromMilliseconds(600))
            };

            up.Completed += (s, e) =>
            {
                DoubleAnimation left = new DoubleAnimation()//движемся влево
                {
                    From = Canvas.GetLeft(chip),
                    To = position,
                    Duration = new Duration(TimeSpan.FromMilliseconds(600))
                };

                left.Completed += (ss, ee) =>
                {
                    DoubleAnimation fall = new DoubleAnimation()//фишка падает
                    {
                        From = Canvas.GetTop(chip),
                        To = Canvas.GetTop(Field) + 36 + row * 90,
                        Duration = new Duration(TimeSpan.FromMilliseconds(150 * row + 50))
                    };

                    fall.Completed += (sss, eee) =>
                    {
                        _matrixField[row, column] = chip;
                        CheckWin();

                        if (!_isWinPlayer2)
                        {
                            PanelPlayer1.Opacity = 1;
                            PanelPlayer2.Opacity = 0.3;
                            _timerPlayer2.Stop();
                            _timerPlayer1.Start();
                            _stopwatchPlayer2.Reset();
                            _stopwatchPlayer1.Start();
                        }
                        else
                        {
                            _timerPlayer2.Stop();
                            _timerPlayer1.Stop();
                            _stopwatchPlayer2.Reset();
                            _stopwatchPlayer1.Start();
                        }

                    };

                    chip.BeginAnimation(TopProperty, fall);
                };

                chip.BeginAnimation(LeftProperty, left);
            };

            chip.BeginAnimation(TopProperty, up);
        }

        private void CheckWin()//проверка победы
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    //по вертикали
                    if (i < 3 && _matrixField[i, j] != null && _matrixField[i + 1, j] != null && _matrixField[i + 2, j] != null && _matrixField[i + 3, j] != null &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 1, j].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 2, j].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 3, j].Fill).Color)
                    {
                        GetLine(GetColumnChip(_matrixField[i, j]) + 40, GetRowLine(_matrixField[i, j]) + 45,
                                GetColumnChip(_matrixField[i, j]) + 40, GetRowLine(_matrixField[i, j]) + 3 * 90 + 45);
                        WhoWin(i, j);
                        break;
                    }//по горизонтали
                    else if (j < 4 && _matrixField[i, j] != null && _matrixField[i, j + 1] != null && _matrixField[i, j + 2] != null && _matrixField[i, j + 3] != null &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i, j + 1].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i, j + 2].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i, j + 3].Fill).Color)
                    {
                        GetLine(GetColumnChip(_matrixField[i, j]) + 40, GetRowLine(_matrixField[i, j]) + 45,
                                GetColumnChip(_matrixField[i, j]) + 3 * 80 + 40, GetRowLine(_matrixField[i, j]) + 45);
                        WhoWin(i, j);
                        break;
                    }//на главной диагонали
                    else if (i < 3 && j < 4 && _matrixField[i, j] != null && _matrixField[i + 1, j + 1] != null && _matrixField[i + 2, j + 2] != null && _matrixField[i + 3, j + 3] != null &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 1, j + 1].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 2, j + 2].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 3, j + 3].Fill).Color)
                    {
                        GetLine(GetColumnChip(_matrixField[i, j]) + 40, GetRowLine(_matrixField[i, j]) + 45,
                                GetColumnChip(_matrixField[i, j]) + 3 * 80 + 40, GetRowLine(_matrixField[i, j]) + 3 * 90 + 45);
                        WhoWin(i, j);
                        break;
                    }//на побочной диагонали
                    else if (i < 3 && j > 2 && _matrixField[i, j] != null && _matrixField[i + 1, j - 1] != null && _matrixField[i + 2, j - 2] != null && _matrixField[i + 3, j - 3] != null &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 1, j - 1].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 2, j - 2].Fill).Color &&
                        ((SolidColorBrush)_matrixField[i, j].Fill).Color == ((SolidColorBrush)_matrixField[i + 3, j - 3].Fill).Color)
                    {
                        GetLine(GetColumnChip(_matrixField[i, j]) + 40, GetRowLine(_matrixField[i, j]) + 45,
                                GetColumnChip(_matrixField[i, j]) - 3 * 80 + 40, GetRowLine(_matrixField[i, j]) + 3 * 90 + 45);
                        WhoWin(i, j);
                        break;
                    }
                }

                if (_isWinPlayer1)
                {
                    _isPlayGame = false;
                    _timerPlayer1.Stop();
                    _stopwatchPlayer1.Reset();
                    GetLine(SystemParameters.PrimaryScreenWidth - 300, 50, SystemParameters.PrimaryScreenWidth - 50, 225);
                    GetLine(SystemParameters.PrimaryScreenWidth - 300, 225, SystemParameters.PrimaryScreenWidth - 50, 50);
                    break;
                }
                else if (_isWinPlayer2)
                {
                    _isPlayGame = false;
                    _timerPlayer2.Stop();
                    _stopwatchPlayer2.Reset();
                    GetLine(50, 50, 300, 175);
                    GetLine(50, 175, 300, 50);
                    break;
                }

                int counterchips = 0;

                for (int ii = 0; ii < 6; ii++)
                {
                    for (int jj = 0; jj < 7; jj++)
                    {
                        if (_matrixField[ii, jj] != null)
                        {
                            counterchips++;
                        }
                    }
                }

                if (counterchips == 42 && !_isWinPlayer1 && !_isWinPlayer2)//ничья
                {
                    _isPlayGame = false;

                    GetLine(SystemParameters.PrimaryScreenWidth - 300, 50, SystemParameters.PrimaryScreenWidth - 50, 225);
                    GetLine(SystemParameters.PrimaryScreenWidth - 300, 225, SystemParameters.PrimaryScreenWidth - 50, 50);
                    GetLine(50, 50, 300, 175);
                    GetLine(50, 175, 300, 50);
                    break;
                }
            }
        }

        private double GetRowLine(Ellipse chip)//получает координаты по вертикали для Y координат линий
        {
            var topRow = Canvas.GetTop(Field) + 36;
            double stopRow = Canvas.GetTop(chip);
            int column = (int)((GetColumnChip(chip) - (SystemParameters.PrimaryScreenWidth / 2 - 280)) / 80);

            if (column >= 0 && column < 7)
            {
                for (int i = 5; i >= 0; i--)
                {
                    if (_matrixField[i, column].Equals(chip))
                    {
                        stopRow = topRow + i * 90;
                        break;
                    }
                }
            }

            return stopRow;
        }

        private void GetLine(double x1, double y1, double x2, double y2)//рисует линию по координатам
        {
            Line line = new Line();

            line.Stroke = new SolidColorBrush(Colors.DarkRed);
            line.StrokeThickness = 3;
            Canvas.SetZIndex(line, 3);
            line.Y1 = y1;
            line.Y2 = y2;
            line.X1 = x1;
            line.X2 = x2;
            GameField.Children.Add(line);
        }

        private void WhoWin(int i, int j)//определяет победителя
        {
            if (((SolidColorBrush)_matrixField[i, j].Fill).Color == _main.ColorPlayer1)
            {
                _isWinPlayer1 = true;
            }
            else if (((SolidColorBrush)_matrixField[i, j].Fill).Color == _main.ColorPlayer2)
            {
                _isWinPlayer2 = true;
            }
        }

        private void Btn_MouseEnter(object sender, MouseEventArgs e)//курсор над кнопками
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();

            dropShadowEffect.Color = Colors.WhiteSmoke;
            dropShadowEffect.BlurRadius = 10;
            dropShadowEffect.ShadowDepth = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_MouseLeave(object sender, MouseEventArgs e)//курсор вышел за пределы кнопки
        {
            var img = sender as Image;
            DropShadowEffect dropShadowEffect = new DropShadowEffect();

            dropShadowEffect.Color = Colors.Transparent;
            dropShadowEffect.Opacity = 0;
            img.Effect = dropShadowEffect;
        }

        private void Btn_NewGame_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FixedChampion();

            Game new_game = new Game();

            new_game.Owner = _main;
            new_game.Show();
            this.Close();
        }

        private void Btn_Exit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FixedChampion();

            _main.Show();
            this.Close();
        }

        private void FixedChampion()//записываем результаты
        {
            if (_isWinPlayer1 && _main.Player_1.Score > _main.operationDB.GetPlayerScore(_main.Player_1.Name))
            {
                _main.operationDB.UpdateScore(_main.Player_1.Name, _main.Player_1.Score);
            }
            else if (_isWinPlayer2 && _main.Player_2.Score > _main.operationDB.GetPlayerScore(_main.Player_2.Name))
            {
                _main.operationDB.UpdateScore(_main.Player_2.Name, _main.Player_2.Score);
            }
        }
    }
}
