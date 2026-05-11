using System;
using System.Linq;
using Tetris.Core.Interfaces;
using Tetris.Pieces;

namespace Tetris.Core;

public class PieceFactory : IPieceFactory
{
    private readonly Random  _rng;
    private readonly Piece[] _pool;

    public PieceFactory(GameMode mode)
    {
        _rng = new Random();

        IPieceSet tetrominoes = new TetrominoSet();
        IPieceSet pentominoes = new PentominoSet();

        _pool = mode switch
        {
            GameMode.Tetromino => tetrominoes.All,
            GameMode.Pentomino => pentominoes.All,
            GameMode.Mixed     => tetrominoes.All.Concat(pentominoes.All).ToArray(),
            _                  => tetrominoes.All
        };
    }

    public Piece Next()
    {
        var piece = _pool[_rng.Next(_pool.Length)].Clone();
        piece.Row = 0;
        piece.Col = (Board.Columns - piece.Cells.GetLength(1)) / 2;
        return piece;
    }
}
