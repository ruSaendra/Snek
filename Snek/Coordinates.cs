namespace Snek
{
    public class Coordinates
    {
        public int X;

        public int Y;

        public Coordinates() { }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinates(Coordinates other) : this(other.X, other.Y) { }

        public bool Equals(Coordinates other) => X == other.X && Y == other.Y;
    }
}
