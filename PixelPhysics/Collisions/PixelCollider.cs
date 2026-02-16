using System;
using PixelPhysics.Shapes;
using PixelPhysics.Structs;

namespace PixelPhysics.Collisions
{
    public class PixelCollider
    {
        public Vec2Int Position;
        public IBinaryShape Shape { get; }
        public readonly ColliderType Type;
        public bool IsTrigger = false;
        public event Action<Collision>? OnCollision;
        internal void InvokeCollisionEvent(Collision arg) => OnCollision?.Invoke(arg);

        public PixelCollider(Vec2Int position, IBinaryShape shape, ColliderType type = ColliderType.Static)
        {
            Position = position;
            Shape = shape;
            Type = type;
        }

        public virtual bool CheckPixel(int x, int y) => Shape.CheckValue(x, y);
    }
}