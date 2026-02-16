using System;
using System.Collections.Generic;
using PixelPhysics.Structs;
using PixelPhysics.Tools;

namespace PixelPhysics.Collisions
{
    public class PixelSpace : IPixelSpace
    {
        private Dictionary<uint, List<PixelCollider>> _colliders = new();

        public void Register(PixelCollider collider, uint layer)
        {
            if(_colliders.TryGetValue(layer, out var colliders))
                colliders.Add(collider);
            else
                _colliders[layer] = new(){ collider };
        }

        public void Remove(PixelCollider collider)
        {
            foreach (var pair in _colliders)
                pair.Value.Remove(collider);
        }

        public void ForEachColliderFromLayers(uint layerMask, Action<PixelCollider> action)
        {
            foreach (var (layer, colliders) in _colliders)
                if((layer & layerMask) > 0)
                    foreach (var collider in colliders)
                        action(collider);
        }

        public List<Collision>? CheckCollisions(DynamicPixelCollider collider, Vec2Int offset = default)
        {
            List<Collision>? collisions = null;
            foreach (var outlinePoint in collider.Outline.Values)
            {
                var pixelInWorld = collider.Position + outlinePoint + offset;

                ForEachColliderFromLayers(collider.CollisionMask, otherCollider =>
                {
                    if(otherCollider != collider)
                    {
                        var otherRect = new RectInt(
                            otherCollider.Position,
                            otherCollider.Position + otherCollider.Shape.Size - 1
                        );
                        if(Geometry.IsOverlap(pixelInWorld, otherRect))
                        {
                            pixelInWorld -= otherCollider.Position; // to local
                            bool isCollision = otherCollider.CheckPixel(pixelInWorld.x, pixelInWorld.y);
                            if(isCollision)
                            {
                                collisions ??= new();
                                var index = collisions.FindIndex(c => c.other == otherCollider);
                                if(index >= 0)
                                {
                                    collisions[index].contacts.Add(pixelInWorld);
                                }
                                else
                                {
                                    collisions.Add(new Collision(otherCollider, new List<Vec2Int>(){pixelInWorld}));
                                }
                            }
                        }
                    }
                });
            }
            return collisions;
        }

        public void MoveCollider(DynamicPixelCollider collider, Vec2Int distance)
        {
            List<Collision>? collisions = null;
            bool ForEachRayStep(Vec2Int rayPoint)
            {
                collisions = CheckCollisions(collider, rayPoint);
                if(collisions == null) return true; // next step
                return false; // stop
            }

            if(distance.x >= -1 && distance.x <= 1 && distance.y >= -1 && distance.y <= 1)
                collisions = CheckCollisions(collider, distance);
            else
                Geometry.PixelRay(0, distance, ForEachRayStep);

            collider.Position += distance;
            if(collisions != null)
            {
                foreach (var collision in collisions)
                {
                    collider.InvokeCollisionEvent(collision);
                    collision.other.InvokeCollisionEvent(new Collision(collider, collision.contacts));
                }
            }
        }
    }
}