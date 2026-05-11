using System;
using System.Collections.Generic;
using Raylib_cs;
namespace Tetris.Core;
public class Piece
{
    public int[,] Cells { get; private set; }
    public Color  Color  { get; }
    public string Name   { get; }
    public int Row { get; set; }
    public int Col { get; set; }
    public int RotationIndex { get; private set; }
    private readonly int[][,] _rotations;
    public Piece(string name, int[][,] allRotations, Color color)
    {
        Name          = name;
        Color         = color;
        _rotations    = allRotations;
        RotationIndex = 0;
        Cells         = _rotations[0];
    }
    public Piece(string name, int[,] shape, Color color)
        : this(name, BuildRotations(shape), color) { }
    private Piece(string name, int[][,] rotations, Color color, int rotIdx, int row, int col)
    {
        Name          = name;
        Color         = color;
        _rotations    = rotations;
        RotationIndex = rotIdx;
        Cells         = _rotations[rotIdx];
        Row           = row;
        Col           = col;
    }
    public Piece Rotated()
    {
        int next    = (RotationIndex + 1) % 4;
        int oldCols = Cells.GetLength(1);
        int newCols = _rotations[next].GetLength(1);
        int colOff  = (oldCols - newCols) / 2;
        int oldRows = Cells.GetLength(0);
        int newRows = _rotations[next].GetLength(0);
        int rowOff  = (oldRows - newRows) / 2;
        return new Piece(Name, _rotations, Color, next, Row + rowOff, Col + colOff);
    }
    public Piece Clone()
    {
        return new Piece(Name, _rotations, Color, RotationIndex, Row, Col);
    }
    public IEnumerable<(int r, int c)> AbsoluteCells()
    {
        int rows = Cells.GetLength(0);
        int cols = Cells.GetLength(1);
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (Cells[r, c] == 1)
                    yield return (Row + r, Col + c);
    }
    private static int[][,] BuildRotations(int[,] shape)
    {
        var r = new int[4][,];
        r[0] = shape;
        for (int i = 1; i < 4; i++)
            r[i] = Rotate90(r[i - 1]);
        return r;
    }
    private static int[,] Rotate90(int[,] m)
    {
        int R = m.GetLength(0), C = m.GetLength(1);
        var n = new int[C, R];
        for (int r = 0; r < R; r++)
            for (int c = 0; c < C; c++)
                n[c, R - 1 - r] = m[r, c];
        return n;
    }
}
