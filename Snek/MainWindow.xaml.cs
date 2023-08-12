using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Snek
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string title = "SNEK Score: ";

        private List<string> field = new List<string>()
        {
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        ",
            "                                                                                        "
        };

        private const char snakeRight = '<';
        private const char snakeLeft = '>';
        private const char snakeUp = 'V';
        private const char snakeDown = 'Ʌ';

        private const char bodyChar = '■';

        private const char foodChar = '•';

        public static RoutedCommand PressLeft = new RoutedCommand();
        public static RoutedCommand PressRight = new RoutedCommand();
        public static RoutedCommand PressUp = new RoutedCommand();
        public static RoutedCommand PressDown = new RoutedCommand();
        public static RoutedCommand PressEsc = new RoutedCommand();

        public MainWindow()
        {
            InitializeComponent();

            AssignKeys();

            SnekControls.BottomBorder = field.Count;
            SnekControls.RightBorder = field[0].Length;

            SnekControls.PlaceTheHead();
            Task.Run(Cycle);
        }

        public void TurnLeft(object sender, ExecutedRoutedEventArgs e) => SnekControls.ChangeDirection(Direction.Left);
        public void TurnRight(object sender, ExecutedRoutedEventArgs e) => SnekControls.ChangeDirection(Direction.Right);
        public void TurnUp(object sender, ExecutedRoutedEventArgs e) => SnekControls.ChangeDirection(Direction.Up);
        public void TurnDown(object sender, ExecutedRoutedEventArgs e) => SnekControls.ChangeDirection(Direction.Down);
        public void Reset(object sender, ExecutedRoutedEventArgs e) => SnekControls.Reset();

        private void Cycle() 
        {
            while (true)
            {
                Thread.Sleep(1000 / 60);

                List<string> fieldTemp = new List<string>(field);

                var head = SnekControls.SnakeHead;
                var tail = SnekControls.SnakeTail;
                var food = SnekControls.Food;
                var direction = SnekControls.Direction;
                var score = SnekControls.Score;

                if (head.X < 0 || head.Y < 0) continue;
                if (food.X < 0 || food.Y < 0) continue;

                char headChar = direction switch
                {
                    Direction.None or Direction.Right => snakeRight,
                    Direction.Left => snakeLeft,
                    Direction.Up => snakeUp,
                    Direction.Down => snakeDown,
                    _ => snakeRight
                };

                var row = fieldTemp[head.Y].ToCharArray();
                row[head.X] = headChar;
                fieldTemp[head.Y] = new string(row);

                tail.ForEach(seg =>
                {
                    row = fieldTemp[seg.Coordinates.Y].ToCharArray();
                    row[seg.Coordinates.X] = bodyChar;
                    fieldTemp[seg.Coordinates.Y] = new string(row);
                });

                row = fieldTemp[food.Y].ToCharArray();
                row[food.X] = foodChar;
                fieldTemp[food.Y] = new string(row);

                try
                {
                    Dispatcher.Invoke(() =>
                    {
                        playingFldTbl.Text = string.Join("\n", fieldTemp);
                        Title = title + score.ToString();
                    });
                }
                catch { }
            }
        }

        private void AssignKeys()
        {
            PressLeft.InputGestures.Add(new KeyGesture(Key.Left));
            PressRight.InputGestures.Add(new KeyGesture(Key.Right));
            PressUp.InputGestures.Add(new KeyGesture(Key.Up));
            PressDown.InputGestures.Add(new KeyGesture(Key.Down));
            PressEsc.InputGestures.Add(new KeyGesture(Key.Escape));
        }

        private void Refresh()
        {
            
        }
    }
}
