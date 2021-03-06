
namespace Konym.AnsiAppEngine;

public interface IRenderer
{
	public void Render(List<Widget> Widgets);

	public void AddColorsAt(int X, int Y, ConsoleColor Foreground, ConsoleColor Background);

	public void CopyToBuffer2D(int X, int Y, int LineWidth, CharacterInfo[] Buffer);

	public void WriteStringAt(int X, int Y, string Text);

	public void WriteColorStringAt(int X, int Y, string Text, ConsoleColor Foreground, ConsoleColor Background);

	public void WriteBox(int X, int Y, int Width, int Height);
}
