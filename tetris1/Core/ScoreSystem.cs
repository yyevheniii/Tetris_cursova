using System;
using System.IO;

namespace Tetris.Core;

public class ScoreSystem
{
    private static readonly int[] LinePoints = { 0, 100, 300, 700, 1500, 2500 };
    private static readonly string SaveFile = "highscore.dat";

    public int Score        { get; private set; }
    public int Level        { get; private set; } = 1;
    public int LinesCleared { get; private set; }
    public int HighScore    { get; private set; }

    public double DropInterval => Math.Max(100, 800 - (Level - 1) * 50);

    public ScoreSystem()
    {
        HighScore = LoadHighScore();
    }

    public void ForceHighScore(int value)
    {
        if (value > HighScore) HighScore = value;
    }

    public void AddLines(int lines)
    {
        if (lines <= 0) return;
        int pts = lines < LinePoints.Length ? LinePoints[lines] : LinePoints[^1];
        Score += pts;
        LinesCleared += lines;
        Level = Score / 500 + 1;
        if (Score > HighScore)
        {
            HighScore = Score;
            SaveHighScore(HighScore);
        }
    }

    public void Reset()
    {
        Score = 0;
        Level = 1;
        LinesCleared = 0;
    }

    private static int LoadHighScore()
    {
        try
        {
            if (File.Exists(SaveFile))
                return int.Parse(File.ReadAllText(SaveFile).Trim());
        }
        catch { }
        return 0;
    }

    private static void SaveHighScore(int value)
    {
        try { File.WriteAllText(SaveFile, value.ToString()); }
        catch { }
    }
}
