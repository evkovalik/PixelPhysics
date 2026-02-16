using System.Collections.Generic;
using PixelPhysics.Structs;

namespace PixelPhysics.Collisions
{
    public readonly struct Collision
    {
        public readonly PixelCollider other;
        public readonly List<Vec2Int> contacts;

        public Collision(PixelCollider other, List<Vec2Int> contacts)
        {
            this.other = other;
            this.contacts = contacts;
        }
    }
}