
namespace Konym.AnsiAppEngine;

public class Textbox : Widget
{
	public int X { get; set; }
	public int Y { get; set; }

	public int CursorX { get; set; }
	public int CursorY { get; set; }

	public bool CursorVisible { get; set; }

	public int Width { get; set; }
	public int Height { get; set; }

	private byte[] Buffer;
	private byte[] ClearBuffer;
	private int TotalBytes;
	private int TotalCells;

	private bool DrawCursor;

	public Textbox(int X, int Y, int Width, int Height) : base()
	{

		this.X = X;
		this.Y = Y;

		DrawCursor = true;
		Task.Run(() =>
		{
			while (true)
			{
				Thread.Sleep(250);
				DrawCursor = !DrawCursor;
			}
		}); ;

		CursorX = 0;
		CursorY = 0;
		CursorVisible = true;

		this.Width = Width;
		this.Height = Height;

		TotalCells = Width * Height;
		TotalBytes = TotalCells * 14;

		Buffer = new byte[TotalBytes];
		ClearBuffer = new byte[TotalBytes];

		for (int i = 0; i < TotalCells; i++)
			ClearBuffer[i * 14 + 10] = (byte) ' ';

		Clear();
	}

	public override void Draw(IRenderer s)
	{
		//	int CursorCellPos = IX(CursorX, CursorY);
		//	
		//	ModifyForegroundAt(CursorCellPos, Sequences.FgBlack);
		//	ModifyBackgroundAt(CursorCellPos, Sequences.BgWhite);
		//	AddClearAt(CursorCellPos);

		// Draw window frame
		s.WriteBox(X, Y, Width + 2, Height + 2);

		// Copy textbox buffer to screen
		//s.CopyToBuffer2D(X + 1, Y + 1, Width, Buffer);

		// Draw cursor
		if (DrawCursor)
			s.AddColorsAt(X + CursorX + 1, Y + CursorY + 1, ConsoleColor.Black, ConsoleColor.White);
	}

	public override void OnConsoleKey(ConsoleKeyInfo cki)
	{
		
	}

	public void Clear()
	{
		Array.Copy(ClearBuffer, Buffer, TotalBytes);
		CursorX = 0;
		CursorY = 0;
	}

	private void AdvanceCursor()
	{
		if (ReachedWidthLimit())
		{
			if (ReachedHeightLimit())
				ScrollDown();
			else
				CursorY++;

			CursorX = 0;
		}
		else
		{
			CursorX++;
		}
	}

	// Handles writing the character and the logic to move the cursor and scroll the buffer when applicable
	private void WriteCharInternal(char Character)
	{
		ModifyChar(Character);
		AdvanceCursor();
	}

	private void WriteCharInternal(char Character, byte[] Foreground, byte[] Background, bool Clear)
	{
		ModifyChar(Character, Foreground, Background, Clear);
		AdvanceCursor();
	}

	// Does the calculation to write the specified character at the exact cursor position
	private void ModifyChar(char Character)
	{
		int BufferIndex = ((CursorY * Width + CursorX) * 14) + 10;
		Buffer[BufferIndex] = (byte) Character;
	}

	private void ModifyChar(char Character, byte[] Foreground, byte[] Background, bool Clear)
	{
		ModifyChar(Character);
		ModifyForeground(Foreground);
		ModifyBackground(Background);

		if (Clear)
			AddClear();
	}

	public void WriteLine(string Text, byte[] Foreground, byte[] Background)
	{
		Write(Text, Foreground, Background);
		NextLine();
	}

	// User API to write text in buffer at current cursor position AND move to next line automatically
	public void WriteLine(string Text)
	{
		Write(Text);
		NextLine();
	}

	public void WriteLine(char Character)
	{
		Write(Character);
		NextLine();
	}

	public void Write(string Text, byte[] Foreground, byte[] Background)
	{
		for (int i = 0; i < Text.Length; i++)
		{
			bool AddClear = ReachedWidthLimit() || i == Text.Length - 1;

			Write(Text[i], Foreground, Background, AddClear);

			//	if (ReachedWidthLimit() || i == Text.Length - 1)
			//		AddClear();
			//	
			//	if (i == 0 || CursorX == 0)
			//		Write(Text[i], Foreground, Background, false);
			//	else
			//		Write(Text[i]);
		}

		//AddClear();
	}

	// User API to write text in buffer at current cursor position
	public void Write(string Text)
	{
		foreach (char c in Text)
			Write(c);
	}

	public void Write(char Character, byte[] Foreground, byte[] Background, bool Clear)
	{
		WriteCharInternal(Character, Foreground, Background, Clear);
	}

	public void Write(char Character)
	{
		WriteCharInternal(Character);
	}

	public void WriteCharInPlace(char Character)
	{
		ModifyChar(Character);
	}

	// User API to manually advance to next line
	public void NextLine()
	{
		if (ReachedHeightLimit())
			ScrollDown();
		else
			CursorY++;

		CursorX = 0;
	}

	private bool ReachedWidthLimit() => CursorX == Width - 1;

	private bool ReachedHeightLimit() => CursorY == Height - 1;

	public void ScrollDown()
	{
		int LineWidthBytes = Width * 14;

		// Shift entire buffer up by one line
		Array.Copy(Buffer, LineWidthBytes, Buffer, 0, Buffer.Length - LineWidthBytes);

		// Clear last line
		for (int i = LineWidthBytes * (Height - 1); i < TotalBytes; i++)
			Buffer[i] = ClearBuffer[i];

	}

	public int IX(int X, int Y)
	{
		int Index = Y * Width + X;

		if (Index > TotalCells)
			return 0;
		else
			return Index;
	}

	private void ModifyForeground(byte[] ColorSequence)
	{
		int BufferIndex = (CursorY * Width + CursorX) * 14;

		for (int i = 0; i < ColorSequence.Length; i++)
			Buffer[BufferIndex + i] = ColorSequence[i];
	}

	private void ModifyBackground(byte[] ColorSequence)
	{
		int BufferIndex = ((CursorY * Width + CursorX) * 14) + 5;

		for (int i = 0; i < ColorSequence.Length; i++)
			Buffer[BufferIndex + i] = ColorSequence[i];
	}

	private void GetColorDataAtCursor(out byte[] Foreground, out byte[] Background)
	{
		Foreground = new byte[5];
		Background = new byte[5];

		int CellForegroundBufferIndex = (CursorY * Width + CursorX) * 14;
		int CellBackgroundBufferIndex = (CursorY * Width + CursorX) * 14 + 5;

		for (int i = 0; i < 5; i++)
		{
			Foreground[i] = Buffer[CellForegroundBufferIndex + i];
			Background[i] = Buffer[CellBackgroundBufferIndex + i];
		}
	}

	private void AddClear()
	{
		int BufferIndex = ((CursorY * Width + CursorX) * 14) + 11;

		for (int i = 0; i < 3; i++)
			Buffer[BufferIndex + i] = Sequences.Clear[i];
	}
}
