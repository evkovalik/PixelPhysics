using System.Collections.Generic;
using PixelPhysics.Structs;

namespace PixelPhysics.Collisions
{
    public interface IPixelSpace
    {
        void Register(PixelCollider collider, uint layer);
        void Remove(PixelCollider collider);
        List<Collision>? CheckCollisions(DynamicPixelCollider collider, Vec2Int offset = default);
        void MoveCollider(DynamicPixelCollider collider, Vec2Int distance);
    }
}