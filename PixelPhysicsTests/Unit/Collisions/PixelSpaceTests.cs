using PixelPhysics.Collisions;
using PixelPhysics.Shapes;
using PixelPhysics.Structs;

namespace PixelPhysicsTests.Unit.Collisions;

public class PixelSpaceTests
{
    private BinaryShapeRect _shape_4x3True = new(new int[,]
    {
        {1,1,1,1},
        {1,1,1,1},
        {1,1,1,1}
    });
    private BinaryShapeRect _shape_3x3True = new(new int[,]
    {
        {1,1,1},
        {1,1,1},
        {1,1,1}
    });

    [Fact]
    public void CheckCollisions_OneIntersection_OneCollision()
    {
        var worldPos = new Vec2Int(2,1);
        var outline = BinaryOutline.CreateFromShape(_shape_3x3True);
        var collider = new PixelCollider(worldPos, _shape_4x3True);
        var dynamicCollider = new DynamicPixelCollider(worldPos + 1, _shape_3x3True, outline);
        uint layer = 1;

        var sut = new PixelSpace();
        sut.Register(collider, layer);
        sut.Register(dynamicCollider, layer);

        // Act
        var collisions = sut.CheckCollisions(dynamicCollider);

        Assert.NotNull(collisions);
        Assert.Single(collisions);
        Assert.Equal(collider, collisions[0].other);
        Assert.Equal(5, collisions[0].contacts.Count);
    }

    [Fact]
    public void CheckCollisions_NoIntersection_NoCollisions()
    {
        var worldPos = new Vec2Int(2,1);
        var outline = BinaryOutline.CreateFromShape(_shape_3x3True);
        var shape = new BinaryShapeRect(new int[,]
        {
            {1,0,0,0},
            {1,0,0,0},
            {1,1,1,1}
        });
        var collider = new PixelCollider(worldPos, shape);
        var dynamicCollider = new DynamicPixelCollider(worldPos + 1, _shape_3x3True, outline);
        uint layer = 1;

        var sut = new PixelSpace();
        sut.Register(collider, layer);
        sut.Register(dynamicCollider, layer);

        // Act
        var collisions = sut.CheckCollisions(dynamicCollider);

        Assert.Null(collisions);
    }

    [Theory]
    [InlineData(-3,0)]
    [InlineData(-4,0)]
    [InlineData(-10,0)]
    [InlineData(-10,1)]
    public void MoveCollider_DifferentDistances_StoppingMovementAfterFirstContact(int distanceX, int distanceY)
    {
        var worldPos = new Vec2Int(2,1);
        var outline = BinaryOutline.CreateFromShape(_shape_3x3True);
        var shape = new BinaryShapeRect(new int[,]
        {
            {1,0,0,0},
            {1,1,0,0},
            {1,1,0,0}
        });
        var collider = new PixelCollider(worldPos, shape);
        var dynamicCollider = new DynamicPixelCollider(worldPos + new Vec2Int(4,0), _shape_3x3True, outline);
        uint layer = 1;

        var sut = new PixelSpace();
        sut.Register(collider, layer);
        sut.Register(dynamicCollider, layer);

        Collision collision = default;
        dynamicCollider.OnCollision += c => collision = c;

        // Act
        sut.MoveCollider(dynamicCollider, new Vec2Int(distanceX, distanceY));

        Assert.NotNull(collision.other);
        Assert.NotNull(collision.contacts);
        Assert.Equal(2, collision.contacts.Count);
    }
}