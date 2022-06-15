
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

	public Textbox(int X, int Y, int Width, int Height)
	{
		this.X = X;
		this.Y = Y;

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

	public override void Draw(Screen s)
	{
		//	int CursorCellPos = IX(CursorX, CursorY);
		//	
		//	ModifyForegroundAt(CursorCellPos, Sequences.FgBlack);
		//	ModifyBackgroundAt(CursorCellPos, Sequences.BgWhite);
		//	AddClearAt(CursorCellPos);

		// Draw window frame
		s.WriteBox(s.IX(X, Y), Width + 2, Height + 2);

		// Copy textbox buffer to screen
		s.CopyToBuffer2D(s.IX(X + 1, Y + 1), Width, Buffer);
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
		if (CursorX + 1 == Width)
		{
			if (CursorY + 1 != Height)
				CursorY++;
			else
				ScrollDown();

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

	// Does the calculation to write the specified character at the exact cursor position
	private void ModifyChar(char Character)
	{
		int BufferIndex = ((CursorY * Width + CursorX) * 14) + 10;
		Buffer[BufferIndex] = (byte) Character;
	}

	// User API to write text in buffer at current cursor position AND move to next line automatically
	public void WriteLine(string Text)
	{
		Write(Text);
		NextLine();
	}

	public void WriteLine(char Character)
	{
		WriteCharInternal(Character);
		NextLine();
	}

	// User API to write text in buffer at current cursor position
	public void Write(string Text)
	{
		foreach (char c in Text)
			Write(c);
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
		if (CursorY + 1 == Height)
		{
			ScrollDown();
			CursorX = 0;
		}
		else
		{
			CursorY++;
			CursorX = 0;
		}
	}

	public void ScrollDown()
	{
		// Shift entire buffer up by one line
		Array.Copy(Buffer, Width * 14, Buffer, 0, Buffer.Length - (Width * 14));

		int LineWidthBytes = Width * 14;

		// Clear last line
		for (int i = LineWidthBytes * (Height - 1); i < TotalBytes; i++)
		{
			Buffer[i] = ClearBuffer[i];
		}

	}

	public int IX(int X, int Y)
	{
		int Index = Y * Width + X;

		if (Index > TotalCells)
			return 0;
		else
			return Index;
	}

	private void ModifyForegroundAt(int CellIndex, byte[] ColorSequence)
	{
		int BufferIndex = CellIndex * 14;

		for (int i = 0; i < ColorSequence.Length; i++)
			Buffer[BufferIndex + i] = ColorSequence[i];
	}

	private void ModifyBackgroundAt(int CellIndex, byte[] ColorSequence)
	{
		int BufferIndex = CellIndex * 14 + 5;

		for (int i = 0; i < ColorSequence.Length; i++)
			Buffer[BufferIndex + i] = ColorSequence[i];
	}

	private void AddClearAt(int CellIndex)
	{
		int BufferIndex = CellIndex * 14 + 11;

		for (int i = 0; i < 3; i++)
			Buffer[BufferIndex + i] = Sequences.Clear[i];
	}
}
