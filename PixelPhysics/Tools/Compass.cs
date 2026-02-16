using PixelPhysics.Structs;

namespace PixelPhysics.Tools
{
    public static class Compass
    {
        public static Vec2Int Rotate45CW(Vec2Int point)
        {
            var x = point.x + point.y;
            var y = -point.x + point.y;
            return new(SignRound(x), SignRound(y));
        }

        public static Vec2Int Rotate45CCW(Vec2Int point)
        {
            var x = point.x - point.y;
            var y = point.x + point.y;
            return new(SignRound(x), SignRound(y));
        }

        private static int SignRound(int value)
        {
            if (value >= 0) return (value + 1) / 2;
            else return (value - 1) / 2;
        }
    }
}