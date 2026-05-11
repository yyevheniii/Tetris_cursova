using Raylib_cs;
using System.Numerics;
using Tetris.Core;

namespace Tetris.Rendering;

public class Renderer
{
    private const int CellSize   = 34;
    private const int BoardLeft  = 20;
    private const int BoardTop   = 50;
    private const int PanelLeft  = BoardLeft + Board.Columns * CellSize + 24;
    private const int PanelWidth = 200;
    private const int WinW       = 760;
    private const int WinH       = 740;

    private readonly Board       _board;
    private readonly ScoreSystem _score;
    private readonly LinePopup   _popup;
    private Font _font;
    private bool _fontLoaded;

    public Renderer(Board board, ScoreSystem score, LinePopup popup)
    {
        _board = board;
        _score = score;
        _popup = popup;
    }

    public void Draw(Piece current, Piece next, bool gameOver, bool paused, GameMode mode)
    {
        EnsureFont();
        DrawGrid();
        DrawLockedCells();
        DrawGhost(current);
        DrawPiece(current, 255);
        DrawPanel(next, mode);
        DrawLinePopup();
        if (paused)   DrawOverlay("PAUSED",    "P continue   M menu   Esc exit");
        if (gameOver) DrawOverlay("GAME OVER", "R restart   M menu   Esc exit");
    }

    private void DrawGrid()
    {
        Raylib.DrawRectangle(BoardLeft, BoardTop,
            Board.Columns * CellSize, Board.Rows * CellSize,
            new Color(20, 20, 35, 255));
        for (int r = 0; r <= Board.Rows; r++)
            Raylib.DrawLine(BoardLeft, BoardTop + r * CellSize,
                            BoardLeft + Board.Columns * CellSize, BoardTop + r * CellSize,
                            new Color(40, 40, 60, 255));
        for (int c = 0; c <= Board.Columns; c++)
            Raylib.DrawLine(BoardLeft + c * CellSize, BoardTop,
                            BoardLeft + c * CellSize, BoardTop + Board.Rows * CellSize,
                            new Color(40, 40, 60, 255));
        Raylib.DrawRectangleLines(BoardLeft - 1, BoardTop - 1,
            Board.Columns * CellSize + 2, Board.Rows * CellSize + 2,
            new Color(100, 100, 160, 255));
    }

    private void DrawLockedCells()
    {
        for (int r = 0; r < Board.Rows; r++)
            for (int c = 0; c < Board.Columns; c++)
            {
                var col = _board[r, c];
                if (col.HasValue) DrawCell(r, c, col.Value, 255);
            }
    }

    private void DrawPiece(Piece piece, byte alpha)
    {
        foreach (var (r, c) in piece.AbsoluteCells())
            if (r >= 0) DrawCell(r, c, piece.Color, alpha);
    }

    private void DrawGhost(Piece piece)
    {
        var ghost = piece.Clone();
        var test  = ghost.Clone();
        test.Row++;
        while (_board.IsValid(test))
        {
            ghost.Row++;
            test.Row++;
        }
        if (ghost.Row > piece.Row)
            DrawPiece(ghost, 55);
    }

    private void DrawCell(int row, int col, Color color, byte alpha)
    {
        int x = BoardLeft + col * CellSize;
        int y = BoardTop  + row * CellSize;
        Raylib.DrawRectangle(x + 1, y + 1, CellSize - 2, CellSize - 2,
            new Color(color.R, color.G, color.B, alpha));
        Raylib.DrawLine(x + 1, y + 1, x + CellSize - 2, y + 1,
            new Color((byte)255, (byte)255, (byte)255, (byte)(alpha / 3)));
        Raylib.DrawLine(x + 1, y + 1, x + 1, y + CellSize - 2,
            new Color((byte)255, (byte)255, (byte)255, (byte)(alpha / 3)));
    }

    private void DrawPanel(Piece next, GameMode mode)
    {
        int x = PanelLeft;
        int y = BoardTop;
        Raylib.DrawRectangle(x, y, PanelWidth, Board.Rows * CellSize, new Color(20, 20, 35, 255));
        Raylib.DrawRectangleLines(x - 1, y - 1, PanelWidth + 2, Board.Rows * CellSize + 2,
            new Color(100, 100, 160, 255));
        DrawT("TETRIS", x + 12, y + 10, 26, Color.White);
        string modeLabel = mode switch
        {
            GameMode.Tetromino => "Tetromino",
            GameMode.Pentomino => "Pentomino",
            GameMode.Mixed     => "Mixed",
            _ => ""
        };
        DrawT($"Mode: {modeLabel}", x + 12, y + 44, 14, new Color(160, 160, 220, 255));
        Raylib.DrawLine(x + 10, y + 66, x + PanelWidth - 10, y + 66, new Color(60, 60, 100, 255));
        DrawPanelLabel(x, y + 76,  "SCORE",  _score.Score.ToString());
        DrawPanelLabel(x, y + 124, "BEST",   _score.HighScore.ToString());
        DrawPanelLabel(x, y + 172, "LEVEL",  _score.Level.ToString());
        DrawPanelLabel(x, y + 220, "LINES",  _score.LinesCleared.ToString());
        Raylib.DrawLine(x + 10, y + 265, x + PanelWidth - 10, y + 265, new Color(60, 60, 100, 255));
        DrawT("NEXT", x + 12, y + 273, 14, new Color(160, 160, 220, 255));
        DrawNextPreview(next, x + 12, y + 295);
        int hy = y + Board.Rows * CellSize - 118;
        Raylib.DrawLine(x + 10, hy, x + PanelWidth - 10, hy, new Color(60, 60, 100, 255));
        DrawHint(x, hy + 8,   "<- ->",  "move");
        DrawHint(x, hy + 28,  "Up/W",   "rotate");
        DrawHint(x, hy + 48,  "Dn/S",   "soft drop");
        DrawHint(x, hy + 68,  "Space",  "hard drop");
        DrawHint(x, hy + 88,  "M",      "menu");
        DrawHint(x, hy + 108, "Esc",    "exit");
    }

    private void DrawPanelLabel(int px, int y, string label, string value)
    {
        DrawT(label, px + 12, y,      12, new Color(120, 120, 180, 255));
        DrawT(value, px + 12, y + 18, 22, Color.White);
    }

    private void DrawHint(int px, int y, string key, string action)
    {
        DrawT(key,    px + 12, y, 12, new Color(200, 200, 100, 255));
        DrawT(action, px + 78, y, 12, new Color(180, 180, 180, 255));
    }

    private void DrawNextPreview(Piece next, int px, int py)
    {
        const int pc = 24;
        int rows = next.Cells.GetLength(0);
        int cols = next.Cells.GetLength(1);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (next.Cells[r, c] == 1)
                {
                    int x = px + c * pc;
                    int y = py + r * pc;
                    Raylib.DrawRectangle(x + 1, y + 1, pc - 2, pc - 2, next.Color);
                    Raylib.DrawLine(x + 1, y + 1, x + pc - 2, y + 1,
                        new Color((byte)255, (byte)255, (byte)255, (byte)80));
                }
    }

    private void DrawLinePopup()
    {
        if (!_popup.Active) return;
        int boardCenterX = BoardLeft + Board.Columns * CellSize / 2;
        int boardCenterY = BoardTop  + Board.Rows    * CellSize / 2;
        float size  = 36f;
        byte  alpha = (byte)(_popup.Alpha * 255);
        float yPos  = boardCenterY + _popup.OffsetY;
        var vec = Raylib.MeasureTextEx(_font, _popup.Text, size, 3);
        float tx = boardCenterX - vec.X / 2;
        float ty = yPos - vec.Y / 2;
        Raylib.DrawRectangle((int)(tx - 14), (int)(ty - 8), (int)(vec.X + 28), (int)(vec.Y + 16),
            new Color((byte)0, (byte)0, (byte)0, (byte)(alpha / 2)));
        Raylib.DrawTextEx(_font, _popup.Text, new Vector2(tx, ty), size, 3,
            new Color((byte)80, (byte)255, (byte)120, alpha));
    }

    private void DrawOverlay(string title, string sub)
    {
        Raylib.DrawRectangle(0, 0, WinW, WinH, new Color(0, 0, 0, 170));
        float ts = 44f;
        var tv = Raylib.MeasureTextEx(_font, title, ts, 3);
        Raylib.DrawTextEx(_font, title, new Vector2(WinW / 2f - tv.X / 2, 295), ts, 3, Color.White);
        float ss = 18f;
        var sv = Raylib.MeasureTextEx(_font, sub, ss, 3);
        Raylib.DrawTextEx(_font, sub, new Vector2(WinW / 2f - sv.X / 2, 355), ss, 3,
            new Color(200, 200, 100, 255));
    }

    private void DrawT(string text, int x, int y, float size, Color color)
    {
        Raylib.DrawTextEx(_font, text, new Vector2(x, y), size, 3, color);
    }

    private void EnsureFont()
    {
        if (_fontLoaded) return;
        _font = Raylib.GetFontDefault();
        _fontLoaded = true;
    }
}
