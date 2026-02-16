using System.Collections.Generic;
using PixelPhysics.Structs;

namespace PixelPhysics.Shapes
{
    public interface IBinaryOutline
    {
        Vec2Int Size { get; }
        IReadOnlyList<Vec2Int> Values { get; }
    }
}