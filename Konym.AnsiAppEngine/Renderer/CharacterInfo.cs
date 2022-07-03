
namespace Konym.AnsiAppEngine;

public struct CharacterInfo
{
	public char Character;
	public ConsoleColor Foreground;
	public ConsoleColor Background;
	public bool SignalAnsiClear;

	public short ColorsToShort()
	{
		// Foreground in bottom 4 bits
		// Background in top 4 bits
		int Attributes = (byte) Foreground | ((byte) Background << 4);

		return (short) Attributes;
	}
}
