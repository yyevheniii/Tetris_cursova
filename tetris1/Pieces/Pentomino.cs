using Raylib_cs;
using Tetris.Core;
using Tetris.Core.Interfaces;

namespace Tetris.Pieces;

public class PentominoSet : IPieceSet
{
    public Piece[] All => new[]
    {

        new Piece("F", new int[,]
        {
            { 0, 1, 1 },
            { 1, 1, 0 },
            { 0, 1, 0 }
        }, new Color(255, 100, 100, 255)),

        new Piece("I5", new int[,]
        {
            { 1, 1, 1, 1, 1 }
        }, new Color(100, 220, 255, 255)),

        new Piece("L5", new int[,]
        {
            { 1, 0 },
            { 1, 0 },
            { 1, 0 },
            { 1, 1 }
        }, new Color(255, 180, 50, 255)),

        new Piece("N", new int[,]
        {
            { 0, 1 },
            { 1, 1 },
            { 1, 0 },
            { 1, 0 }
        }, new Color(180, 255, 100, 255)),

        new Piece("P", new int[,]
        {
            { 1, 1 },
            { 1, 1 },
            { 1, 0 }
        }, new Color(255, 80, 200, 255)),

        new Piece("T5", new int[,]
        {
            { 1, 1, 1 },
            { 0, 1, 0 },
            { 0, 1, 0 }
        }, new Color(200, 100, 255, 255)),

        new Piece("U", new int[,]
        {
            { 1, 0, 1 },
            { 1, 1, 1 }
        }, new Color(100, 255, 200, 255)),

        new Piece("V", new int[,]
        {
            { 1, 0, 0 },
            { 1, 0, 0 },
            { 1, 1, 1 }
        }, new Color(255, 220, 100, 255)),

        new Piece("W", new int[,]
        {
            { 1, 0, 0 },
            { 1, 1, 0 },
            { 0, 1, 1 }
        }, new Color(100, 150, 255, 255)),

        new Piece("X", new int[,]
        {
            { 0, 1, 0 },
            { 1, 1, 1 },
            { 0, 1, 0 }
        }, new Color(255, 255, 100, 255)),

        new Piece("Y", new int[,]
        {
            { 0, 1 },
            { 1, 1 },
            { 0, 1 },
            { 0, 1 }
        }, new Color(150, 255, 150, 255)),

        new Piece("Z5", new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 0 },
            { 0, 1, 1 }
        }, new Color(255, 150, 50, 255)),

        new Piece("F'", new int[,]
        {
            { 1, 1, 0 },
            { 0, 1, 1 },
            { 0, 1, 0 }
        }, new Color(255, 160, 160, 255)),

        new Piece("J5", new int[,]
        {
            { 0, 1 },
            { 0, 1 },
            { 0, 1 },
            { 1, 1 }
        }, new Color(255, 210, 120, 255)),

        new Piece("N'", new int[,]
        {
            { 1, 0 },
            { 1, 1 },
            { 0, 1 },
            { 0, 1 }
        }, new Color(210, 255, 120, 255)),

        new Piece("P'", new int[,]
        {
            { 1, 1 },
            { 1, 1 },
            { 0, 1 }
        }, new Color(255, 120, 220, 255)),

        new Piece("Y'", new int[,]
        {
            { 1, 0 },
            { 1, 1 },
            { 1, 0 },
            { 1, 0 }
        }, new Color(180, 255, 180, 255)),

        new Piece("S5", new int[,]
        {
            { 0, 1, 1 },
            { 0, 1, 0 },
            { 1, 1, 0 }
        }, new Color(255, 190, 80, 255)),
    };
}
