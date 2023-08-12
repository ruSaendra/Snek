using System;

namespace Snek
{
    public class BodySegment
    {
        public Coordinates Coordinates { get; set; }

        public BodySegment? PreviousSegment { get; set; }

        public BodySegment? NextSegment { get; set; }

        public BodySegment(int x, int y, BodySegment? connectedSegment = null, bool putAtHead = true) : this(new Coordinates() { X = x, Y = y }, connectedSegment, putAtHead) { }

        public BodySegment(Coordinates coordinates, BodySegment? connectedSegment = null, bool putAtHead = true)
        {
            if (connectedSegment != null && !connectedSegment.IsConnected(coordinates))
                throw new Exception();

            Coordinates = coordinates;

            if (putAtHead)
                NextSegment = connectedSegment;
            else
                PreviousSegment = connectedSegment;
        }

        public bool IsConnected(Coordinates coordinates)
        {
            if(coordinates.Equals(Coordinates)) return false;

            if (coordinates.X == Coordinates.X 
                && (Math.Abs(coordinates.Y - Coordinates.Y) == 1
                    || (coordinates.Y == 0 && Coordinates.Y == SnekControls.BottomBorder - 1)
                    || (coordinates.Y == SnekControls.BottomBorder - 1 && Coordinates.Y == 0))) 
                return true;

            if (coordinates.Y == Coordinates.Y 
                && (Math.Abs(coordinates.X - Coordinates.X) == 1
                    || (coordinates.X == 0 && Coordinates.Y == SnekControls.RightBorder - 1)
                    || (coordinates.Y == SnekControls.RightBorder - 1 && Coordinates.Y == 0)))
                return true;

            return false;
        }
    }
}
