using Raylib_cs;
namespace Tetris.Core;
public class Board
{
    public const int Rows    = 20;
    public const int Columns = 10;
    private readonly Color?[,] _grid = new Color?[Rows, Columns];
    public Color? this[int r, int c] => _grid[r, c];
    public bool IsValid(Piece piece)
    {
        foreach (var (r, c) in piece.AbsoluteCells())
        {
            if (c < 0 || c >= Columns) return false;
            if (r >= Rows)             return false;
            if (r >= 0 && _grid[r, c].HasValue) return false;
        }
        return true;
    }
    public int Lock(Piece piece)
    {
        foreach (var (r, c) in piece.AbsoluteCells())
            if (r >= 0 && r < Rows && c >= 0 && c < Columns)
                _grid[r, c] = piece.Color;
        return ClearFullLines();
    }
    private int ClearFullLines()
    {
        int cleared = 0;
        for (int r = Rows - 1; r >= 0; r--)
        {
            if (IsLineFull(r))
            {
                RemoveLine(r);
                r++;        
                cleared++;
            }
        }
        return cleared;
    }
    private bool IsLineFull(int row)
    {
        for (int c = 0; c < Columns; c++)
            if (!_grid[row, c].HasValue) return false;
        return true;
    }
    private void RemoveLine(int row)
    {
        for (int r = row; r > 0; r--)
            for (int c = 0; c < Columns; c++)
                _grid[r, c] = _grid[r - 1, c];
        for (int c = 0; c < Columns; c++)
            _grid[0, c] = null;
    }
    public void Clear()
    {
        for (int r = 0; r < Rows; r++)
            for (int c = 0; c < Columns; c++)
                _grid[r, c] = null;
    }
    public bool HasOverflow()
    {
        for (int c = 0; c < Columns; c++)
            if (_grid[0, c].HasValue) return true;
        return false;
    }
}
