using System.Text;

namespace Konym.AnsiAppEngine;

public static class Sequences
{
	private static Random ansi_rand = new();

	public static void Init()
	{
		Null = new byte[] { 0, 0, 0, 0, 0 };
		NullControl = new byte[] { 0, 0, 0, 0 };

		CursorToTopLeft = Encoding.UTF8.GetBytes("\u001b[;H");
		Clear = new byte[3] { 27, 91, 109 };

		Reset = Encoding.UTF8.GetBytes("\u001b[0m");
		BoldText = Encoding.UTF8.GetBytes("\u001b[1m");
		RegularText = Encoding.UTF8.GetBytes("\u001b[2m");
		UnderlineText = Encoding.UTF8.GetBytes("\u001b[4m");

		FgBlack = Encoding.UTF8.GetBytes("\u001b[30m");
		FgRed = Encoding.UTF8.GetBytes("\u001b[31m");
		FgGreen = Encoding.UTF8.GetBytes("\u001b[32m");
		FgYellow = Encoding.UTF8.GetBytes("\u001b[33m");
		FgBlue = Encoding.UTF8.GetBytes("\u001b[34m");
		FgMagenta = Encoding.UTF8.GetBytes("\u001b[35m");
		FgCyan = Encoding.UTF8.GetBytes("\u001b[36m");
		FgWhite = Encoding.UTF8.GetBytes("\u001b[37m");

		FgGray = Encoding.UTF8.GetBytes("\u001b[90m");
		FgBrightRed = Encoding.UTF8.GetBytes("\u001b[91m");
		FgBrightGreen = Encoding.UTF8.GetBytes("\u001b[92m");
		FgBrightYellow = Encoding.UTF8.GetBytes("\u001b[93m");
		FgBrightBlue = Encoding.UTF8.GetBytes("\u001b[94m");
		FgBrightMagenta = Encoding.UTF8.GetBytes("\u001b[95m");
		FgBrightCyan = Encoding.UTF8.GetBytes("\u001b[96m");
		FgBrightWhite = Encoding.UTF8.GetBytes("\u001b[97m");

		BgBlack = Encoding.UTF8.GetBytes("\u001b[40m");
		BgRed = Encoding.UTF8.GetBytes("\u001b[41m");
		BgGreen = Encoding.UTF8.GetBytes("\u001b[42m");
		BgYellow = Encoding.UTF8.GetBytes("\u001b[43m");
		BgBlue = Encoding.UTF8.GetBytes("\u001b[44m");
		BgMagenta = Encoding.UTF8.GetBytes("\u001b[45m");
		BgCyan = Encoding.UTF8.GetBytes("\u001b[46m");
		BgWhite = Encoding.UTF8.GetBytes("\u001b[47m");

		BgGray = Encoding.UTF8.GetBytes("\u001b[100m");
		BgBrightRed = Encoding.UTF8.GetBytes("\u001b[101m");
		BgBrightGreen = Encoding.UTF8.GetBytes("\u001b[102m");
		BgBrightYellow = Encoding.UTF8.GetBytes("\u001b[103m");
		BgBrightBlue = Encoding.UTF8.GetBytes("\u001b[104m");
		BgBrightMagenta = Encoding.UTF8.GetBytes("\u001b[105m");
		BgBrightCyan = Encoding.UTF8.GetBytes("\u001b[106m");
		BgBrightWhite = Encoding.UTF8.GetBytes("\u001b[107m");
	}

	public static byte[] Null;
	public static byte[] NullControl;
	public static byte[] Clear;

	// Cursor controls

	public static byte[] CursorToTopLeft;

	// Text controls

	public static byte[] Reset;
	public static byte[] BoldText;
	public static byte[] RegularText;
	public static byte[] UnderlineText;


	// Foreground colors

	public static byte[] FgBlack;
	public static byte[] FgRed;
	public static byte[] FgGreen;
	public static byte[] FgYellow;
	public static byte[] FgBlue;
	public static byte[] FgMagenta;
	public static byte[] FgCyan;
	public static byte[] FgWhite;

	public static byte[] FgGray;
	public static byte[] FgBrightRed;
	public static byte[] FgBrightGreen;
	public static byte[] FgBrightYellow;
	public static byte[] FgBrightBlue;
	public static byte[] FgBrightMagenta;
	public static byte[] FgBrightCyan;
	public static byte[] FgBrightWhite;

	public static byte[] RandomForeground()
	{
		return ansi_rand.Next(0, 16) switch
		{
			0 => FgBlack,
			1 => FgRed,
			2 => FgGreen,
			3 => FgYellow,
			4 => FgBlue,
			5 => FgMagenta,
			6 => FgCyan,
			7 => FgWhite,

			8 => FgGray,
			9 => FgBrightRed,
			10 => FgBrightGreen,
			11 => FgBrightYellow,
			12 => FgBrightBlue,
			13 => FgBrightMagenta,
			14 => FgBrightCyan,
			15 => FgBrightWhite,
		};
	}

	// Background colors

	public static byte[] BgBlack;
	public static byte[] BgRed;
	public static byte[] BgGreen;
	public static byte[] BgYellow;
	public static byte[] BgBlue;
	public static byte[] BgMagenta;
	public static byte[] BgCyan;
	public static byte[] BgWhite;

	public static byte[] BgGray;
	public static byte[] BgBrightRed;
	public static byte[] BgBrightGreen;
	public static byte[] BgBrightYellow;
	public static byte[] BgBrightBlue;
	public static byte[] BgBrightMagenta;
	public static byte[] BgBrightCyan;
	public static byte[] BgBrightWhite;

	public static byte[] RandomBackground()
	{
		return ansi_rand.Next(0, 16) switch
		{
			0 => BgBlack,
			1 => BgRed,
			2 => BgGreen,
			3 => BgYellow,
			4 => BgBlue,
			5 => BgMagenta,
			6 => BgCyan,
			7 => BgWhite,

			8 => BgGray,
			9 => BgBrightRed,
			10 => BgBrightGreen,
			11 => BgBrightYellow,
			12 => BgBrightBlue,
			13 => BgBrightMagenta,
			14 => BgBrightCyan,
			15 => BgBrightWhite,
		};
	}
}
