using PixelPhysics.Shapes;

namespace PixelPhysicsTests.Unit.Shapes;

public class BinaryOutlineTests
{
    [Fact]
    public void CreateFromShape_Shape4x3True_EqualResult()
    {
        var shape = new BinaryShapeRect(new int[,]{
            {1,1,1,1},
            {1,1,1,1},
            {1,1,1,1}
        });
        var outline = new BinaryOutline([
            new(0,0), new(0,1), new(0,2),   // up
            new(1,2), new(2,2), new(3,2),   // right
            new(3,1), new(3,0),             // down
            new(2,0), new(1,0)              // left
        ]);

        // Act
        var sut = BinaryOutline.CreateFromShape(shape);

        for (int i = 0; i < outline.Values.Count; i++)
            Assert.Equal(outline.Values[i], sut.Values[i]);
    }

    [Fact]
    public void CreateFromShape_Shape4x3_EqualResult()
    {
        var shape = new BinaryShapeRect(new int[,]{
            {0,1,1,0},
            {1,0,0,1},
            {0,1,1,1}
        });
        var outline = new BinaryOutline([
            new(1,0), new(0,1), new(1,2), new(2,2),
            new(3,1), new(3,0), new(2,0)
        ]);

        // Act
        var sut = BinaryOutline.CreateFromShape(shape);

        for (int i = 0; i < outline.Values.Count; i++)
            Assert.Equal(outline.Values[i], sut.Values[i]);
    }
}
