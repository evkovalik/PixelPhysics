
namespace PixelPhysics.Shapes
{
    public interface IMutableBinaryShape : IBinaryShape
    {
        void ChangeValue(int x, int y, bool value);
    }
}