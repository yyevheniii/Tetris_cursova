using Raylib_cs;
using Tetris.Core.Interfaces;
using Tetris.Rendering;

namespace Tetris.Core;

public class GameState
{
    private readonly Board             _board    = new();
    private readonly ScoreSystem       _score    = new();
    private readonly LinePopup         _popup    = new();
    private readonly InputHandler      _input    = new();
    private readonly IPieceFactory     _factory;
    private readonly IWallKickProvider _wallKick;
    private readonly Renderer          _renderer;
    private readonly GameMode          _mode;

    private Piece  _current = null!;
    private Piece  _next    = null!;
    private double _dropTimer;
    private double _softDropTimer;
    private bool   _gameOver;
    private bool   _paused;
    private bool   _returnToMenu;
    private bool   _exitGame;

    private const double SoftDropDelay = 0.05;

    public bool ReturnToMenu     => _returnToMenu;
    public bool ExitGame         => _exitGame;
    public int  CurrentHighScore => _score.HighScore;

    public GameState(GameMode mode, int inheritedHighScore = 0)
    {
        _mode     = mode;
        _factory  = new PieceFactory(mode);
        _wallKick = new WallKickProvider();
        _renderer = new Renderer(_board, _score, _popup);

        if (inheritedHighScore > 0)
            _score.ForceHighScore(inheritedHighScore);

        SpawnNext();
        _current = _next;
        SpawnNext();
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose() && !_returnToMenu && !_exitGame)
        {
            float dt = Raylib.GetFrameTime();

            _input.Update(dt);

            if (!_gameOver && !_paused)
                UpdatePhysics(dt);

            _popup.Update(dt);
            HandleInput(dt);

            Raylib.BeginDrawing();
            Raylib.ClearBackground(new Color(15, 15, 25, 255));
            _renderer.Draw(_current, _next, _gameOver, _paused, _mode);
            Raylib.EndDrawing();

            if (_gameOver && _input.RestartPressed)
                Restart();
        }
    }

    private void UpdatePhysics(float dt)
    {

        _dropTimer += dt;
        double interval = _score.DropInterval / 1000.0;
        if (_dropTimer >= interval)
        {
            _dropTimer = 0;
            if (!CanMoveDown(_current)) LockCurrent();
            else _current.Row++;
        }

        for (int i = 0; i < _input.DasRepeatCount; i++)
            TryMoveHorizontal(_input.DasHorizontalDir);
    }

    private void HandleInput(float dt)
    {
        if (_input.PauseToggled)
            _paused = !_paused;

        if (_gameOver || _paused)
        {
            if (_input.MenuRequested) _returnToMenu = true;
            if (_input.ExitRequested) _exitGame     = true;
            return;
        }

        if (_input.MenuRequested) { _returnToMenu = true; return; }
        if (_input.ExitRequested) { _exitGame     = true; return; }

        if (_input.RotatePressed)   TryRotate();
        if (_input.HardDropPressed) HardDrop();

        if (_input.InitialHorizontalDir != 0)
            TryMoveHorizontal(_input.InitialHorizontalDir);


        if (_input.SoftDownHeld)
        {
            _softDropTimer += dt;
            while (_softDropTimer >= SoftDropDelay)
            {
                if (!CanMoveDown(_current)) LockCurrent();
                else _current.Row++;
                _softDropTimer -= SoftDropDelay;
                if (_gameOver || _returnToMenu) return;
            }
        }
        else
        {
            _softDropTimer = 0;
        }
    }

    private void TryMoveHorizontal(int dir)
    {
        var moved = _current.Clone();
        moved.Col += dir;
        if (_board.IsValid(moved))
            _current = moved;
    }

    private bool CanMoveDown(Piece p)
    {
        var test = p.Clone();
        test.Row++;
        return _board.IsValid(test);
    }

    private void TryRotate()
    {
        if (!CanMoveDown(_current))
            return;
        var rotated = _current.Rotated();
        var kicks   = _wallKick.GetColOffsets(_current.Name);
        foreach (int dc in kicks)
        {
            var test = rotated.Clone();
            test.Col += dc;
            if (_board.IsValid(test))
            {
                _current = test;
                return;
            }
        }
    }

    private void HardDrop()
    {
        while (CanMoveDown(_current))
            _current.Row++;
        LockCurrent();
    }

    private void LockCurrent()
    {
        int lines = _board.Lock(_current);
        _score.AddLines(lines);
        if (lines > 0) _popup.Trigger(lines);
        _dropTimer     = 0;
        _softDropTimer = 0;
        _current = _next;
        SpawnNext();
        if (!_board.IsValid(_current))
            _gameOver = true;
    }

    private void SpawnNext()
    {
        _next = _factory.Next();
    }

    private void Restart()
    {
        _board.Clear();
        _score.Reset();
        _input.Reset();
        _gameOver      = false;
        _paused        = false;
        _dropTimer     = 0;
        _softDropTimer = 0;
        SpawnNext();
        _current = _next;
        SpawnNext();
    }
}
