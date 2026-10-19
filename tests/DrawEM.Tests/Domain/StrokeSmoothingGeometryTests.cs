using DrawEM.App.Domain.Drawing;

namespace DrawEM.Tests.Domain;

public sealed class StrokeSmoothingGeometryTests
{
    [Fact]
    public void CreatePieces_UsesFirstAndLastPointsAndQuadraticInnerControlPoints()
    {
        var points = new[]
        {
            new ScreenPoint(0, 0),
            new ScreenPoint(4, 8),
            new ScreenPoint(10, 4),
            new ScreenPoint(14, 12),
        };

        var pieces = StrokeSmoothingGeometry.CreatePieces(points);

        Assert.Collection(pieces,
            first => Assert.Equal(new StrokePathPiece.Line(new ScreenPoint(0, 0), new ScreenPoint(2, 4)), first),
            second => Assert.Equal(new StrokePathPiece.Quadratic(new ScreenPoint(2, 4), new ScreenPoint(4, 8), new ScreenPoint(7, 6)), second),
            third => Assert.Equal(new StrokePathPiece.Quadratic(new ScreenPoint(7, 6), new ScreenPoint(10, 4), new ScreenPoint(12, 8)), third),
            last => Assert.Equal(new StrokePathPiece.Line(new ScreenPoint(12, 8), new ScreenPoint(14, 12)), last));
    }

    [Fact]
    public void CreatePieces_TwoPointsProducesStraightLinePieces()
    {
        Assert.Collection(StrokeSmoothingGeometry.CreatePieces([new ScreenPoint(1, 2), new ScreenPoint(5, 8)]),
            first => Assert.Equal(
                new StrokePathPiece.Line(new ScreenPoint(1, 2), new ScreenPoint(3, 5)), first),
            last => Assert.Equal(
                new StrokePathPiece.Line(new ScreenPoint(3, 5), new ScreenPoint(5, 8)), last));
    }
}
