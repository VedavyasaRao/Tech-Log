using System.Runtime.InteropServices;

//TICS -3@102  -- not relevant here
//TICS -7@101  -- not relevant here
//TICS -3@105  -- not relevant here
//TICS -10@301  -- not relevant here


namespace UITesting.Automated.MouseKeyboardActivityMonitor.WinApi
{
    /// <summary>
    /// The Point structure defines the X- and Y- coordinates of a point. 
    /// </summary>
    /// <remarks>
    /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/gdi/rectangl_0tiq.asp
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    internal struct Point {
        /// <summary>
        /// Specifies the X-coordinate of the point. 
        /// </summary>
        public int X;
        /// <summary>
        /// Specifies the Y-coordinate of the point. 
        /// </summary>
        public int Y;

        /// <summary>Point</summary>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>operator ==</summary>
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>operator !=</summary>
        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }

        /// <summary>Equals</summary>
        public bool Equals(Point other)
        {
            return other.X == X && other.Y == Y;
        }

        /// <summary>Equals</summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Point)) return false;
            return Equals((Point) obj);
        }

        /// <summary>GetHashCode</summary>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }
    }
}
//TICS +10@301
//TICS +3@105
//TICS +7@101
//TICS +3@102


