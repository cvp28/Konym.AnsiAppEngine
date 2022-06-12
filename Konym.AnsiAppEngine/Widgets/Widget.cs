
namespace Konym.AnsiAppEngine;

public abstract class Widget
{
	public abstract void Draw(Screen s);

	public abstract void OnConsoleKey(ConsoleKeyInfo cki);
}
