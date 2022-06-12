
namespace Konym.AnsiAppEngine;

public class Label : Widget
{
	public int X { get; set; }
	public int Y { get; set; }

	public string Text { get; set; }

	public byte[] ForegroundColor;
	public byte[] BackgroundColor;

	public Label(int X, int Y)
	{
		this.X = X;
		this.Y = Y;
		Text = string.Empty;
		ForegroundColor = Sequences.Null;
		BackgroundColor = Sequences.Null;
	}

	public Label(int X, int Y, string Text)
	{
		this.X = X;
		this.Y = Y;
		this.Text = Text;
		ForegroundColor = Sequences.Null;
		BackgroundColor = Sequences.Null;
	}

	public Label(int X, int Y, string Text, byte[] Foreground, byte[] Background)
	{
		this.X = X;
		this.Y = Y;
		this.Text = Text;
		ForegroundColor = Foreground;
		BackgroundColor = Background;
	}

	public override void Draw(Screen s)
	{
		if (Text.Length == 0) { return; }
		
		s.WriteColorStringAt(s.IX(X, Y), Text, ForegroundColor, BackgroundColor);
	}

	public override void OnConsoleKey(ConsoleKeyInfo cki)
	{
		
	}
}
