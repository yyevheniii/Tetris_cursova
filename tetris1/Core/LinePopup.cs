namespace Tetris.Core;
public class LinePopup
{
    public string Text    { get; private set; } = "";
    public float  Alpha   { get; private set; }
    public float  OffsetY { get; private set; }
    public bool   Active  => Alpha > 0;
    private const float Duration  = 1.4f;
    private const float RiseSpeed = 40f;
    private float _timer;
    public void Trigger(int lines)
    {
        Text   = lines switch { 1 => "+1 LINE", 2 => "+2 LINES", 3 => "+3 LINES", 4 => "+4 LINES", _ => $"+{lines} LINES" };
        _timer  = Duration;
        Alpha   = 1f;
        OffsetY = 0f;
    }
    public void Update(float dt)
    {
        if (_timer <= 0) return;
        _timer  -= dt;
        OffsetY -= RiseSpeed * dt;
        Alpha    = System.Math.Max(0f, _timer / Duration);
    }
}
