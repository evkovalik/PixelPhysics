using PixelPhysics.Collisions;
using PixelPhysics.Shapes;
using PixelPhysics.Structs;

namespace PixelPhysicsTests.Unit.Collisions;

public class MutablePixelColliderTests
{
    private BinaryShapeRect _shape_4x3False = new(new int[,]
    {
        {0,0,0,0},
        {0,0,0,0},
        {0,0,0,0}
    });
    private BinaryShapeRect _shape_4x3Grid = new(new int[,]
    {
        {1,0,1,0},
        {0,1,0,1},
        {1,0,1,0}
    });
    private BinaryShapeRect _shape_4x3True = new(new int[,]
    {
        {1,1,1,1},
        {1,1,1,1},
        {1,1,1,1}
    });
    private BinaryShapeRect _shape_3x2True = new(new int[,]
    {
        {1,1,1},
        {1,1,1}
    });
    private BinaryShapeRect _shape_3x2Grid = new(new int[,]
    {
        {1,0,1},
        {0,1,0}
    });

    [Fact]
    public void AddShape_Collider4x3FalseAndShape3x2True_EqualResult()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3False);
        var result = new BinaryShapeRect(new int[,]
        {
            {0,0,1,1},
            {0,0,1,1},
            {0,0,0,0}
        });

        // Act
        sut.AddShape(worldPos + new Vec2Int(2,1), _shape_3x2True);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
    }

    [Fact]
    public void AddShape_Collider4x3GridAndShape3x2Grid_EqualResult()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3Grid);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,1,1,1},
            {0,1,1,1},
            {1,0,1,0}
        });

        // Act
        sut.AddShape(worldPos + new Vec2Int(1,1), _shape_3x2Grid);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
    }

    [Fact]
    public void AddShape_Collider4x3GridAndShape4x3False_EqualResult()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3Grid);
        var result = _shape_4x3Grid;

        // Act
        sut.AddShape(worldPos, _shape_4x3False);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
    }

    [Fact]
    public void AddShape_Collider3x2GridAndShape3x2GridAndShiftToLeft_EqualResult()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_3x2Grid);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,1,1},
            {1,1,0}
        });

        // Act
        sut.AddShape(worldPos - new Vec2Int(1,0), _shape_3x2Grid);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
    }

    [Fact]
    public void RemoveShape_Collider4x3GridAndShape3x2True_EqualResult()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3Grid);
        var result = new BinaryShapeRect(new int[,]
        {
            {0,0,1,0},
            {0,0,0,1},
            {1,0,1,0}
        });

        // Act
        sut.RemoveShape(worldPos + new Vec2Int(-1,1), _shape_3x2True);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
    }

    [Fact]
    public void RemoveShape_Collider4x3GridAndShape4x3False_NoChanges()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3Grid);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,0,1,0},
            {0,1,0,1},
            {1,0,1,0}
        });

        // Act
        sut.RemoveShape(worldPos, _shape_4x3False);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
    }

    [Fact]
    public void MultipleChanges_EmptyColliderAndTwoChanges_EqualResultAndOldShape()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3False);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,0,1,0},
            {0,0,0,1},
            {0,0,0,0}
        });

        // Act
        sut.AddShape(worldPos, _shape_4x3Grid);
        sut.RemoveShape(worldPos, _shape_3x2True);

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
            {
                Assert.Equal(result.CheckValue(x, y), sut.CheckPixel(x,y));
                Assert.False(sut.Shape.CheckValue(x,y));
            }
    }

    [Fact]
    public void ProcessAll_TwoChanges_ShapeUpdated()
    {
        var worldPos = new Vec2Int(2,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3False);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,0,1,0},
            {0,0,0,1},
            {0,0,0,0}
        });
        sut.AddShape(worldPos, _shape_4x3Grid);
        sut.RemoveShape(worldPos, _shape_3x2True);

        // Act
        sut.ProcessAll();

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.Shape.CheckValue(x,y));
    }

    [Fact]
    public void ProcessPart_OneChangesAndProcessingHalf_EqualResult()
    {
        var worldPos = new Vec2Int(1,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3True);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,1,1,1},
            {1,1,1,1},
            {0,0,0,1}
        });
        sut.ProcessingPartSize = 3;
        sut.RemoveShape(worldPos, _shape_3x2True);

        // Act
        sut.ProcessPart();

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.Shape.CheckValue(x,y));

        Assert.True(sut.IsChanges);
    }

    [Fact]
    public void ProcessPart_OneChangesAndProcessingAll_EqualResult()
    {
        var worldPos = new Vec2Int(1,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3True);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,1,1,1},
            {0,0,0,1},
            {0,0,0,1}
        });
        sut.ProcessingPartSize = 12;
        sut.RemoveShape(worldPos, _shape_3x2True);

        // Act
        sut.ProcessPart();

        // Assert
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.Shape.CheckValue(x,y));

        Assert.False(sut.IsChanges);
    }

    [Fact]
    public void ProcessPart_TwoChangesAndProcessingAll_EqualResult()
    {
        var worldPos = new Vec2Int(1,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3Grid);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,1,1,1},
            {0,0,1,0},
            {1,0,0,0}
        });
        sut.ProcessingPartSize = 100;
        sut.AddShape(worldPos + new Vec2Int(1,1), _shape_3x2Grid);
        sut.RemoveShape(worldPos + new Vec2Int(1,0), _shape_3x2Grid);

        // Act
        sut.ProcessPart();

        // Assert
        Assert.False(sut.IsChanges);
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.Shape.CheckValue(x,y));
    }

    [Fact]
    public void ProcessPart_TwoChangesAndTwoProcessing_EqualResult()
    {
        var worldPos = new Vec2Int(1,1);
        var sut = new MutablePixelCollider(worldPos, _shape_4x3Grid);
        var result = new BinaryShapeRect(new int[,]
        {
            {1,1,1,1},
            {0,1,1,1},
            {1,0,0,0}
        });
        sut.ProcessingPartSize = 4;
        sut.AddShape(worldPos + new Vec2Int(1,1), _shape_3x2Grid);
        sut.RemoveShape(worldPos + new Vec2Int(1,0), _shape_3x2True);

        // Act
        sut.ProcessPart();
        sut.ProcessPart();

        // Assert
        Assert.True(sut.IsChanges);
        for (int x = 0; x < result.Size.x; x++)
            for (int y = 0; y < result.Size.y; y++)
                Assert.Equal(result.CheckValue(x, y), sut.Shape.CheckValue(x,y));
    }
}