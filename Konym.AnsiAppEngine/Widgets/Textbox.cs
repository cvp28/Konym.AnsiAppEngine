
namespace Konym.AnsiAppEngine;

public class Textbox : Widget
{
	public int X { get; set; }
	public int Y { get; set; }

	public int CursorX { get; set; }
	public int CursorY { get; set; }

	public int Width { get; set; }
	public int Height { get; set; }


	public override void Draw(Screen s)
	{
		s.WriteBox(s.IX(X, Y), Width + 2, Height + 2);
	}

	public override void OnConsoleKey(ConsoleKeyInfo cki)
	{
		
	}
}
