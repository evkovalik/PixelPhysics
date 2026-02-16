using PixelPhysics.Structs;
using PixelPhysics.Tools;

namespace PixelPhysicsTests.Unit;

public class GeometryTests
{
    [Fact]
    public void IsOverlap_TwoRectangles_True()
    {
        var rect1 = new RectInt(new(1,1), new(5,4));
        var rect2 = new RectInt(new(4,3), new(7,5));

        var result = Geometry.IsOverlap(rect1, rect2);

        Assert.True(result);
    }

    [Fact]
    public void IsOverlap_TwoRectangles_False()
    {
        var rect1 = new RectInt(new(1,1), new(5,4));
        var rect2 = new RectInt(new(6,1), new(9,3));

        var result = Geometry.IsOverlap(rect1, rect2);

        Assert.False(result);
    }

    [Fact]
    public void CalculateOverlap_TwoRectangles_Success()
    {
        var rect1 = new RectInt(new(1,1), new(5,4));
        var rect2 = new RectInt(new(4,3), new(7,5));
        var overlap = new RectInt(new(4,3), new(5,4));

        var result = Geometry.CalculateOverlap(rect1, rect2);

        Assert.Equal(overlap.min.x, result.min.x);
        Assert.Equal(overlap.min.y, result.min.y);
        Assert.Equal(overlap.max.x, result.max.x);
        Assert.Equal(overlap.max.y, result.max.y);
    }

    [Fact]
    public void PixelRay_DowntoLeft_EqualResult()
    {
        List<Vec2Int> correctResult = [
            new(1,5), new(1,4), new(2,3), new(2,2), new(2,1)
        ];
        var result = new List<Vec2Int>();

        Geometry.PixelRay(new Vec2Int(1,6), new Vec2Int(2,1), point =>
        {
            result.Add(point);
            return true;
        });

        Assert.Equal(correctResult.Count, result.Count);
        foreach (var point in correctResult)
            Assert.Contains(point, result);
    }
}
