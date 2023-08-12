using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Snek
{
    public static class SnekControls
    {
        private static Direction direction = Direction.None;
        private static Direction incDirection = Direction.None;
        private static int delay = 75;

        private static Coordinates snakeHead = new() { X = -1, Y = -1 };
        private static List<BodySegment> snakeTail = new();
        private static Coordinates food = new() { X = -2, Y = -2 };

        private static bool readyToPlay = false;

        private static int score = 0;

        private static CancellationTokenSource source;
        private static CancellationToken token;

        public static int RightBorder = 1;
        public static int BottomBorder = 1;

        public static Direction Direction { get => direction; }

        public static Coordinates SnakeHead { get => snakeHead; }

        public static List<BodySegment> SnakeTail { get => snakeTail; }

        public static Coordinates Food { get => food; }

        public static int Score { get => score; }

        public static void PlaceTheHead()
        {
            readyToPlay = false;

            score = 0;

            snakeHead.X = RightBorder/2;
            snakeHead.Y = BottomBorder/2;
            direction = Direction.None;
            incDirection = Direction.None;

            snakeTail = new();

            GenerateTail(5);
            SpawnFood();

            source = new CancellationTokenSource();
            token = source.Token;

            Task.Run(() => Cycle(token), token);
        }

        public static void Reset()
        {
            source.Cancel();
            PlaceTheHead();
        }

        private static void Cycle(CancellationToken token)
        {
            readyToPlay = true;

            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(delay);

                if (direction == Direction.None && incDirection == Direction.None)
                    continue;

                direction = incDirection;

                MoveHead();
            }
        }

        public static void ChangeDirection(Direction dir)
        {
            if (!readyToPlay || IsSameOrOpposite(dir)) return;

            incDirection = dir;
        }

        private static bool IsSameOrOpposite(Direction dir)
        {
            if(direction == Direction.None) return false;

            if(dir == direction) return true;

            if(dir == Direction.Up && direction == Direction.Down) return true;

            if(dir == Direction.Down && direction == Direction.Up) return true;

            if(dir == Direction.Left && direction == Direction.Right) return true;

            if(dir == Direction.Right && direction == Direction.Left) return true;

            return false;
        }

        private static void MoveHead()
        {
            var coords = new Coordinates(snakeHead);

            switch (direction)
            {
                case Direction.Left:
                    snakeHead.X = snakeHead.X == 0 ? RightBorder - 1 : snakeHead.X - 1;
                    break;
                case Direction.Right:
                    snakeHead.X = snakeHead.X == RightBorder - 1 ? 0 : snakeHead.X + 1;
                    break;
                case Direction.Up:
                    snakeHead.Y = snakeHead.Y == 0 ? BottomBorder - 1 : snakeHead.Y - 1;
                    break;
                case Direction.Down:
                    snakeHead.Y = snakeHead.Y == BottomBorder - 1 ? 0 : snakeHead.Y + 1;
                    break;
                default: 
                    break;
            }

            if (snakeTail.Any(st => st.Coordinates.Equals(snakeHead)))
            {
                Reset();
                return;
            }   

            if (snakeHead.Equals(food))
            {
                Chomp(coords);
                return;
            }   

            var first = FindFirst();
            var last = FindLast();
            var secondToLast = last.PreviousSegment;

            last.Coordinates = coords;
            last.PreviousSegment = null;
            last.NextSegment = first;

            first.PreviousSegment = last;

            secondToLast.NextSegment = null;
        }

        private static void Chomp(Coordinates coords)
        {
            var first = FindFirst();
            var newSegment = new BodySegment(coords, first, true);
            snakeTail.Add(newSegment);
            first.PreviousSegment = newSegment;
            score += 1;
            SpawnFood();
        }

        private static void GenerateTail(int count = 5)
        {
            var coords = new Coordinates() { X = snakeHead.X - 1, Y = snakeHead.Y };
            BodySegment? lastSegment = null;

            for (int i = 0; i < count; i++)
            {
                Thread.Sleep(200);

                var bodySegment = new BodySegment(coords, lastSegment, false);
                snakeTail.Add(bodySegment);
                coords = new Coordinates(coords);
                coords.X -= 1;

                if (lastSegment != null)
                {
                    bodySegment.PreviousSegment = lastSegment;
                    lastSegment.NextSegment = bodySegment;
                }   

                lastSegment = bodySegment;
            }
        }

        private static void SpawnFood()
        {
            Coordinates coords;

            do
            {
                Random rand = new Random();
                coords = new Coordinates(rand.Next(RightBorder), rand.Next(BottomBorder));
            }
            while (coords.Equals(snakeHead) || snakeTail.Any(st => st.Coordinates.Equals(coords)));

            food = coords;
        }

        private static BodySegment? FindFirst() => snakeTail.Where(bs => bs.PreviousSegment == null).First();

        private static BodySegment? FindLast() => snakeTail.Where(bs => bs.NextSegment == null).First();
    }
}
