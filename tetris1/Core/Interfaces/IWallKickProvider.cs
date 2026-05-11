namespace Tetris.Core.Interfaces;

public interface IWallKickProvider
{
    int[] GetColOffsets(string pieceName);
}
