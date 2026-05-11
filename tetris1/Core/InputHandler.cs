using Raylib_cs;

namespace Tetris.Core;

public class InputHandler
{
    private double _dasTimer;
    private double _arrTimer;
    private int    _dasDir;

    private const double DasDelay = 0.17;
    private const double ArrDelay = 0.05;

    public bool PauseToggled         { get; private set; }
    public bool MenuRequested        { get; private set; }
    public bool ExitRequested        { get; private set; }
    public bool RotatePressed        { get; private set; }
    public bool HardDropPressed      { get; private set; }
    public bool SoftDownHeld         { get; private set; }
    public bool RestartPressed       { get; private set; }

    public int InitialHorizontalDir  { get; private set; }

    public int DasHorizontalDir      { get; private set; }
    public int DasRepeatCount        { get; private set; }

    public void Update(float dt)
    {
        PauseToggled    = Raylib.IsKeyPressed(KeyboardKey.P);
        MenuRequested   = Raylib.IsKeyPressed(KeyboardKey.M);
        ExitRequested   = Raylib.IsKeyPressed(KeyboardKey.Escape);
        RotatePressed   = Raylib.IsKeyPressed(KeyboardKey.Up)    || Raylib.IsKeyPressed(KeyboardKey.W);
        HardDropPressed = Raylib.IsKeyPressed(KeyboardKey.Space);
        SoftDownHeld    = Raylib.IsKeyDown(KeyboardKey.Down)     || Raylib.IsKeyDown(KeyboardKey.S);
        RestartPressed  = Raylib.IsKeyPressed(KeyboardKey.R);

        bool leftPressed   = Raylib.IsKeyPressed(KeyboardKey.Left)  || Raylib.IsKeyPressed(KeyboardKey.A);
        bool rightPressed  = Raylib.IsKeyPressed(KeyboardKey.Right) || Raylib.IsKeyPressed(KeyboardKey.D);
        bool leftReleased  = Raylib.IsKeyReleased(KeyboardKey.Left) || Raylib.IsKeyReleased(KeyboardKey.A);
        bool rightReleased = Raylib.IsKeyReleased(KeyboardKey.Right)|| Raylib.IsKeyReleased(KeyboardKey.D);

        bool justChanged = false;
        if (leftPressed)
        {
            InitialHorizontalDir = -1;
            _dasDir = -1; _dasTimer = 0; _arrTimer = 0;
            justChanged = true;
        }
        else if (rightPressed)
        {
            InitialHorizontalDir = 1;
            _dasDir = 1; _dasTimer = 0; _arrTimer = 0;
            justChanged = true;
        }
        else
        {
            InitialHorizontalDir = 0;
        }

        if (leftReleased  && _dasDir == -1) _dasDir = 0;
        if (rightReleased && _dasDir ==  1) _dasDir = 0;

        DasRepeatCount   = 0;
        DasHorizontalDir = 0;
        if (_dasDir != 0 && !justChanged)
        {
            _dasTimer += dt;
            if (_dasTimer >= DasDelay)
            {
                _arrTimer += dt;
                while (_arrTimer >= ArrDelay)
                {
                    DasRepeatCount++;
                    _arrTimer -= ArrDelay;
                }
                if (DasRepeatCount > 0)
                    DasHorizontalDir = _dasDir;
            }
        }
    }

    public void Reset()
    {
        _dasDir   = 0;
        _dasTimer = 0;
        _arrTimer = 0;
    }
}
