using PixelPhysics.Structs;

namespace PixelPhysics.Shapes
{
    public class BinaryShapeRect : IMutableBinaryShape
    {
        private bool[] _mask;
        private Vec2Int _size;

        public BinaryShapeRect(int[,] pixels)
        {
            _size = new Vec2Int(pixels.GetLength(1), pixels.GetLength(0));
            _mask = new bool[pixels.Length];
            
            for (int y = 0; y < _size.y; y++)
                for (int x = 0; x < _size.x; x++)
                    _mask[x + y * _size.x] = pixels[_size.y - 1 - y, x] > 0;
        }

        public Vec2Int Size => _size;
        public bool CheckValue(int x, int y) => _mask[x + y * _size.x];
        public void ChangeValue(int x, int y, bool value) => _mask[x + y * _size.x] = value;
    }
}