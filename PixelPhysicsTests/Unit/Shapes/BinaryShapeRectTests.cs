using PixelPhysics.Shapes;

namespace PixelPhysicsTests.Unit.Shapes;

public class BinaryShapeRectTests
{
    [Fact]
    public void CheckValue()
    {
        var sut = new BinaryShapeRect(new int[,]{
            {0,1,1},
            {1,1,1},
            {1,0,1}}
        );

        // Act
        var value1 = sut.CheckValue(1,0);
        var value2 = sut.CheckValue(0,2);
        var value3 = sut.CheckValue(2,2);
        var value4 = sut.CheckValue(2,0);

        Assert.False(value1);
        Assert.False(value2);
        Assert.True(value3);
        Assert.True(value4);
    }
}