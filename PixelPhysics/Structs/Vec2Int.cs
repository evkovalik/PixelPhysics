using System;

namespace PixelPhysics.Structs
{
    public struct Vec2Int : IEquatable<Vec2Int>
    {
        public int x;
        public int y;

        public Vec2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vec2Int(int value) => new(value, value);

        public static Vec2Int operator +(Vec2Int lhs, Vec2Int rhs) =>
            new(lhs.x + rhs.x, lhs.y + rhs.y);

        public static Vec2Int operator -(Vec2Int lhs, Vec2Int rhs) =>
            new(lhs.x - rhs.x, lhs.y - rhs.y);

        public static bool operator ==(Vec2Int lhs, Vec2Int rhs) =>
            lhs.x == rhs.x && lhs.y == rhs.y;
        
        public static bool operator !=(Vec2Int lhs, Vec2Int rhs) =>
            lhs.x != rhs.x || lhs.y != rhs.y;

        public override readonly bool Equals(object obj) => obj is Vec2Int other && Equals(other);
        public readonly bool Equals(Vec2Int other) => x == other.x && y == other.y;
        public override readonly int GetHashCode() => HashCode.Combine(x, y);

    } 
}