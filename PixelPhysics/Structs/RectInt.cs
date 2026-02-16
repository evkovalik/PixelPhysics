
namespace PixelPhysics.Structs
{
    public struct RectInt
    {
        public Vec2Int min, max;

        public RectInt(Vec2Int min, Vec2Int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}