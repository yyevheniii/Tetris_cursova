using Raylib_cs;
using System.Numerics;
using Tetris.Core;
namespace Tetris.UI;
public class MenuScreen
{
    private int _selected = 0;
    private static readonly (string Label, string Desc, GameMode Mode)[] Options =
    {
        ("Tetromino", "Classic mode  -  4-cell pieces",  GameMode.Tetromino),
        ("Pentomino", "Extended mode -  5-cell pieces",  GameMode.Pentomino),
        ("Mixed",     "Combined - both types",           GameMode.Mixed),
    };
    public GameMode? Update()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Up))
            _selected = (_selected - 1 + Options.Length) % Options.Length;
        if (Raylib.IsKeyPressed(KeyboardKey.Down))
            _selected = (_selected + 1) % Options.Length;
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Space))
            return Options[_selected].Mode;
        for (int i = 0; i < Options.Length; i++)
            if (Raylib.IsKeyPressed(KeyboardKey.One + i))
                return Options[i].Mode;
        return null;
    }
    public void Draw(int highScore)
    {
        var font = Raylib.GetFontDefault();
        int cx = 380;
        DrawC(font, "TETRIS", cx, 95, 54, 4, Color.White);
        DrawC(font, "Select game mode", cx, 172, 18, 3, new Color(160, 160, 220, 255));
        for (int i = 0; i < Options.Length; i++)
        {
            bool active = i == _selected;
            int  y      = 232 + i * 86;
            Color box    = active ? new Color(55, 55, 115, 255) : new Color(22, 22, 45, 255);
            Color border = active ? new Color(110, 110, 230, 255) : new Color(55, 55, 95, 255);
            Raylib.DrawRectangle(cx - 210, y, 420, 68, box);
            Raylib.DrawRectangleLines(cx - 210, y, 420, 68, border);
            Raylib.DrawRectangle(cx - 195, y + 16, 28, 28,
                active ? new Color(110, 110, 230, 255) : new Color(55, 55, 95, 255));
            DrawC(font, $"{i + 1}", cx - 181, y + 19, 18, 2, Color.White);
            if (active)
                DrawTriangle(cx - 210 - 18, y + 34);
            Color lc = active ? Color.White : new Color(175, 175, 195, 255);
            DrawC(font, Options[i].Label, cx + 18, y + 10, 19, 3, lc);
            DrawC(font, Options[i].Desc,  cx + 18, y + 36, 13, 2,
                active ? new Color(195, 195, 250, 255) : new Color(110, 110, 155, 255));
        }
        if (highScore > 0)
            DrawC(font, $"Session best: {highScore}", cx, 500, 16, 3, new Color(255, 215, 80, 255));
        DrawC(font, "Up/Down to select   Enter to start", cx, 528, 14, 3, new Color(95, 95, 135, 255));
    }
    private static void DrawTriangle(int x, int cy)
    {
        Raylib.DrawTriangle(
            new Vector2(x,      cy - 10),
            new Vector2(x,      cy + 10),
            new Vector2(x + 14, cy),
            new Color(110, 110, 230, 255));
    }
    private static void DrawC(Font font, string text, int cx, int y, float size, float spacing, Color color)
    {
        var v = Raylib.MeasureTextEx(font, text, size, spacing);
        Raylib.DrawTextEx(font, text, new Vector2(cx - v.X / 2, y), size, spacing, color);
    }
}
