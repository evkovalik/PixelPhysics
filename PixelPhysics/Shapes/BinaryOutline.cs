using System;
using System.Collections.Generic;
using PixelPhysics.Structs;
using PixelPhysics.Tools;

namespace PixelPhysics.Shapes
{
    public class BinaryOutline : IBinaryOutline
    {
        private Vec2Int _size;
        private List<Vec2Int> _values;

        public Vec2Int Size => _size;
        public IReadOnlyList<Vec2Int> Values => _values;

        public BinaryOutline(List<Vec2Int> points)
        {
            _values = points;
            foreach(var point in points)
            {
                if(point.x > _size.x) _size.x = point.x;
                if(point.y > _size.y) _size.y = point.y;
            }
            _size += 1;
        }

        public BinaryOutline(List<Vec2Int> points, Vec2Int size)
        {
            _values = points;
            _size = size;
        }

        public static BinaryOutline CreateFromShape(IBinaryShape shape)
        {
            for (int y = 0; y < shape.Size.y; y++)
                for (int x = 0; x < shape.Size.x; x++)
                    if(shape.CheckValue(x, y))
                    {
                        var outline = ApplyAlgorithmMNT(new(x, y), new(x - 1, y), shape);
                        return new(outline, shape.Size);
                    }
            throw new ArgumentException("The shape does not contain pixels");
        }

        /// <summary>
        /// Moore's neighborhood tracing algorithm
        /// </summary>
        private static List<Vec2Int> ApplyAlgorithmMNT(Vec2Int start, Vec2Int beforeStart, IBinaryShape shape)
        {
            var outline = new List<Vec2Int>(){start};
            var point = start;
            var arrow = Compass.Rotate45CW(beforeStart - start);
            var next = point + arrow;
            while(next != start)
            {
                if(next.x < 0 || next.x >= shape.Size.x || next.y < 0 || next.y >= shape.Size.y)
                {
                    arrow = Compass.Rotate45CW(arrow);
                    next = point + arrow;
                    continue;
                }

                if(shape.CheckValue(next.x, next.y))
                {
                    outline.Add(next);
                    var previousPoint = Compass.Rotate45CCW(arrow) + point;
                    point = outline[^1];
                    arrow = Compass.Rotate45CW(previousPoint - point);
                }
                else arrow = Compass.Rotate45CW(arrow);

                next = point + arrow;
            }
            return outline;
        }
    }
}