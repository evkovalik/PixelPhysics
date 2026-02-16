using System;
using PixelPhysics.Structs;

namespace PixelPhysics.Tools
{
    public static class Geometry
    {
        public static bool IsOverlap(RectInt rect1, RectInt rect2)
        {
            if(rect1.max.x < rect2.min.x || rect1.min.x > rect2.max.x) return false;
            if(rect1.max.y < rect2.min.y || rect1.min.y > rect2.max.y) return false;
            return true;
        }

        public static bool IsOverlap(Vec2Int point, RectInt rect)
        {
            if(point.x < rect.min.x || point.x > rect.max.x) return false;
            if(point.y < rect.min.y || point.y > rect.max.y) return false;
            return true;
        }

        public static RectInt CalculateOverlap(RectInt rect1, RectInt rect2)
        {
            var result = new RectInt();
            result.min.x = Math.Clamp(rect2.min.x, rect1.min.x, rect1.max.x);
            result.max.x = Math.Clamp(rect2.max.x, rect1.min.x, rect1.max.x);
            result.min.y = Math.Clamp(rect2.min.y, rect1.min.y, rect1.max.y);
            result.max.y = Math.Clamp(rect2.max.y, rect1.min.y, rect1.max.y);
            return result;
        }

        /// <summary>
        /// Builds a ray from point 0 to point 1 and calls a function for each point.
        /// If the function returns false, then the ray construction stops.
        /// The action is not called for point0.
        /// </summary>
        public static void PixelRay(Vec2Int point0, Vec2Int point1, Func<Vec2Int, bool> func)
        {
            int dx = Math.Abs(point1.x - point0.x);
            int dy = Math.Abs(point1.y - point0.y);
            
            int sx = (point0.x < point1.x) ? 1 : -1;
            int sy = (point0.y < point1.y) ? 1 : -1;
            
            int err = dx - dy;
            var currentPoint = point0;

            bool next = true;
            while (next && (currentPoint.x != point1.x || currentPoint.y != point1.y))
            {
                int err2 = err * 2;
                if (err2 > -dy)
                {
                    err -= dy;
                    currentPoint.x += sx;
                }
                if (err2 < dx)
                {
                    err += dx;
                    currentPoint.y += sy;
                }
                next = func(currentPoint);
            }
        }
    }
}