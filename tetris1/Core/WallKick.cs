using Tetris.Core.Interfaces;

namespace Tetris.Core;

public class WallKickProvider : IWallKickProvider
{
    public int[] GetColOffsets(string pieceName)
    {
        if (pieceName == "O")
            return new[] { 0 };
        if (pieceName == "I" || pieceName == "I5")
            return new[] { 0, -1, -2, -3, 1, 2, 3 };
        return new[] { 0, -1, -2, 1, 2 };
    }
}
