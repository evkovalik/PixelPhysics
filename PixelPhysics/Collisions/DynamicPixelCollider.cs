using PixelPhysics.Shapes;
using PixelPhysics.Structs;

namespace PixelPhysics.Collisions
{
    public class DynamicPixelCollider : PixelCollider
    {
        public uint CollisionMask = uint.MaxValue;
        public readonly IBinaryOutline Outline;

        public DynamicPixelCollider(Vec2Int position, IBinaryShape shape, IBinaryOutline outline)
        : base(position, shape, ColliderType.Dynamic)
        {
            Outline = outline;
        }
    }
}