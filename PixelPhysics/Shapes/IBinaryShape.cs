using PixelPhysics.Structs;

namespace PixelPhysics.Shapes
{
    public interface IBinaryShape
    {
        Vec2Int Size { get; }
        bool CheckValue(int x, int y);
    }
}