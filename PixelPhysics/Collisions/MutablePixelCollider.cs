using System;
using System.Collections.Generic;
using PixelPhysics.Shapes;
using PixelPhysics.Structs;
using PixelPhysics.Tools;

namespace PixelPhysics.Collisions
{
    public class MutablePixelCollider : PixelCollider
    {
        public int ProcessingPartSize = 1000;
        public bool IsChanges => _isChanges;
        private IMutableBinaryShape _mutableShape;
        private LinkedList<DeferredChange> _changesQueue = new();
        private bool _isChanges = false;
        private Vec2Int _processPoint = 0;

        enum Operation { Add, Remove }

        readonly struct DeferredChange
        {
            public readonly Operation operation;
            public readonly Vec2Int position;
            public readonly IBinaryShape shape;
            public readonly RectInt localOverlap;
            public DeferredChange(Operation operation, Vec2Int position, IBinaryShape shape, RectInt localOverlap)
            {
                this.operation = operation;
                this.position = position;
                this.shape = shape;
                this.localOverlap = localOverlap;
            }
        }

        public MutablePixelCollider(Vec2Int position, IMutableBinaryShape shape)
        : base(position, shape, ColliderType.StaticMutable)
        {
            _mutableShape = shape;
        }

        public override bool CheckPixel(int x, int y)
        {
            if(x >= _mutableShape.Size.x || y >= _mutableShape.Size.y)
                throw new ArgumentOutOfRangeException(
                    $"Arguments must be in the range: x = [0; {_mutableShape.Size.x-1}], y = [0; {_mutableShape.Size.y-1}]"
                );

            var pixel = _mutableShape.CheckValue(x, y);
            if(!_isChanges) return pixel;

            for(var node = _changesQueue.Last; node != null; node = node.Previous)
            {
                if(Geometry.IsOverlap(new Vec2Int(x, y), node.Value.localOverlap))
                {
                    var value =  node.Value.shape.CheckValue(
                        x + Position.x - node.Value.position.x,
                        y + Position.y - node.Value.position.y
                    );

                    if(node.Value.operation == Operation.Add)
                        return value || pixel;
                    else
                        return !value && pixel;
                }
            }
            return pixel;
        }

        public void AddShape(Vec2Int worldPosition, IBinaryShape shape)
        {
            if(CalculateOverlap(worldPosition, shape, out var localOverlap))
            {
                _changesQueue.AddLast(new DeferredChange(Operation.Add, worldPosition, shape, localOverlap!.Value));
                _isChanges = true;
            }
        }

        public void RemoveShape(Vec2Int worldPosition, IBinaryShape shape)
        {
            if(CalculateOverlap(worldPosition, shape, out var localOverlap))
            {
                _changesQueue.AddLast(new DeferredChange(Operation.Remove, worldPosition, shape, localOverlap!.Value));
                _isChanges = true;
            }
        }

        private bool CalculateOverlap(Vec2Int position, IBinaryShape shape, out RectInt? localOverlap)
        {
            localOverlap = null;
            var rect1 = new RectInt(Position, Position + Shape.Size - 1);
            var rect2 = new RectInt(position, position + shape.Size - 1);

            if(!Geometry.IsOverlap(rect1, rect2)) return false;

            var overlap = Geometry.CalculateOverlap(rect1, rect2);
            overlap.min -= Position;
            overlap.max -= Position;
            localOverlap = overlap;
            return true;
        }

        public void ProcessAll()
        {
            if(!_isChanges) return;

            for(var node = _changesQueue.First; node != null; node = node.Next)
            {
                var changes = node.Value;
                var start = changes.localOverlap.min;
                var end = changes.localOverlap.max;

                for (int y = start.y; y <= end.y; y++)
                    for (int x = start.x; x <= end.x; x++)
                    {
                        var pointInShape = new Vec2Int(x, y) + Position - changes.position;
                        var pixelInShape = changes.shape.CheckValue(pointInShape.x, pointInShape.y);

                        bool colliderPixel = _mutableShape.CheckValue(x, y);
                        if(changes.operation == Operation.Add)
                            colliderPixel |= pixelInShape;
                        else
                            colliderPixel &= !pixelInShape;

                        _mutableShape.ChangeValue(x, y, colliderPixel);
                    }
            }
            _changesQueue.Clear();
            _isChanges = false;
        }

        public void ProcessPart()
        {
            if(!_isChanges) return;

            var node = _changesQueue.First;
            int resource = ProcessingPartSize;
            while(resource > 0 && node != null)
            {
                var changes = node.Value;
                var start = changes.localOverlap.min + _processPoint;
                var end = changes.localOverlap.max;

                for (int y = start.y; y <= end.y; y++)
                    for (int x = start.x; x <= end.x; x++)
                    {
                        var pointInShape = new Vec2Int(x, y) + Position - changes.position;
                        var pixelInShape = changes.shape.CheckValue(pointInShape.x, pointInShape.y);

                        bool colliderPixel = _mutableShape.CheckValue(x, y);
                        if(changes.operation == Operation.Add)
                            colliderPixel |= pixelInShape;
                        else
                            colliderPixel &= !pixelInShape;
                        _mutableShape.ChangeValue(x, y, colliderPixel);
                        
                        if(--resource <= 0)
                        {
                            _processPoint = new Vec2Int(x + 1, y) - changes.localOverlap.min; // saving offset
                            return;
                        }
                    }
                _processPoint = new Vec2Int(0, 0);
                node = node.Next;
                _changesQueue.RemoveFirst();
            }
            if(_changesQueue.Count == 0) _isChanges = false;
        }
    }
}