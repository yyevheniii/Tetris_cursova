using Tetris.UI;
using Tetris.Core;
using Raylib_cs;
namespace Tetris;
class Program
{
    static void Main()
    {
        Raylib.InitWindow(760, 740, "Tetris - Kursova robota");
        Raylib.SetTargetFPS(60);
        var menu = new MenuScreen();
        int highScore = 0;
        while (!Raylib.WindowShouldClose())
        {
            GameMode? selectedMode = null;
            while (!Raylib.WindowShouldClose() && selectedMode == null)
            {
                selectedMode = menu.Update();
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.Black);
                menu.Draw(highScore);
                Raylib.EndDrawing();
            }
            if (selectedMode == null) break;
            var game = new GameState(selectedMode.Value, highScore);
            game.Run();
            highScore = game.CurrentHighScore;
            if (game.ExitGame) break;
        }
        Raylib.CloseWindow();
    }
}
