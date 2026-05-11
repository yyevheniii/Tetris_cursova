using Raylib_cs;
using Tetris.Core;
using Tetris.Core.Interfaces;

namespace Tetris.Pieces;

public class TetrominoSet : IPieceSet
{
    public Piece[] All => new[]
    {
        new Piece("I", new int[][,]
        {
            new int[,] { { 1, 1, 1, 1 } },
            new int[,] { { 1 }, { 1 }, { 1 }, { 1 } },
            new int[,] { { 1, 1, 1, 1 } },
            new int[,] { { 1 }, { 1 }, { 1 }, { 1 } },
        }, new Color(0, 240, 240, 255)),

        new Piece("O", new int[,]
        {
            { 1, 1 },
            { 1, 1 }
        }, new Color(240, 240, 0, 255)),

        new Piece("T", new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        }, new Color(160, 0, 240, 255)),

        new Piece("S", new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 },
            { 0, 0, 0 }
        }, new Color(0, 240, 0, 255)),

        new Piece("Z", new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 0, 0, 0 }
        }, new Color(240, 0, 0, 255)),

        new Piece("J", new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        }, new Color(0, 0, 240, 255)),

        new Piece("L", new int[,]
        {
            { 0, 0, 1 },
            { 1, 1, 1 },
            { 0, 0, 0 }
        }, new Color(240, 160, 0, 255)),
    };
}
