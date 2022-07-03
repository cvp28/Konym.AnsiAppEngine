using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konym.AnsiAppEngine;

public class NewTextBox : Widget
{
	public int X { get; set; }
	public int Y { get; set; }

	public int CursorX { get; set; }
	public int CursorY { get; set; }

	public bool CursorVisible { get; set; }

	public int Width { get; set; }
	public int Height { get; set; }

	private CharacterInfo[] Buffer;
	private CharacterInfo[] ClearBuffer;
	private int TotalCells;

	private bool DrawCursor;

	public NewTextBox(int X, int Y, int Width, int Height) : base()
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

		Buffer = new CharacterInfo[TotalCells];
		ClearBuffer = new CharacterInfo[TotalCells];

		for (int i = 0; i < TotalCells; i++)
		{
			ClearBuffer[i].Character = ' ';
			ClearBuffer[i].Foreground = ConsoleColor.White;
			ClearBuffer[i].Background = ConsoleColor.Black;
		}

		Clear();
	}

	public void Clear()
	{
		Array.Copy(ClearBuffer, Buffer, TotalCells);
		CursorX = 0;
		CursorY = 0;
	}

	public override void Draw(IRenderer s)
	{
		s.WriteBox(X, Y, Width + 2, Height + 2);

		s.CopyToBuffer2D(X + 1, Y + 1, Width, Buffer);

		if (DrawCursor)
			s.AddColorsAt(X + CursorX + 1, Y + CursorY + 1, ConsoleColor.Black, ConsoleColor.White);
	}

	public override void OnConsoleKey(ConsoleKeyInfo cki)
	{

	}

	private bool ReachedWidthLimit() => CursorX == Width - 1;

	private bool ReachedHeightLimit() => CursorY == Height - 1;

	public void ScrollDown()
	{
		// Shift entire buffer up by one line
		Array.Copy(Buffer, Width, Buffer, 0, Buffer.Length - Width);

		// Clear last line
		for (int i = Width * (Height - 1); i < TotalCells; i++)
			Buffer[i] = ClearBuffer[i];
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

	public int IX(int X, int Y)
	{
		int Cell = Y * Width + X;

		if (Cell > TotalCells)
			return Cell % TotalCells;
		else
			return Cell;
	}

	private void ModifyChar(char Character)
	{
		int CellIndex = IX(CursorX, CursorY);
		Buffer[CellIndex].Character = Character;
	}

	private void ModifyChar(char Character, ConsoleColor Foreground, ConsoleColor Background)
	{
		int CellIndex = IX(CursorX, CursorY);

		Buffer[CellIndex].Character = Character;
		Buffer[CellIndex].Foreground = Foreground;
		Buffer[CellIndex].Background = Background;
	}

	private void AddClear()
	{
		int CellIndex = IX(CursorX, CursorY);
		Buffer[CellIndex].SignalAnsiClear = true;
	}

	public void NextLine()
	{
		if (ReachedHeightLimit())
			ScrollDown();
		else
			CursorY++;

		CursorX = 0;
	}

	public void WriteLine(string Text)
	{
		Write(Text);
		NextLine();
	}

	public void WriteLine(string Text, ConsoleColor Foreground, ConsoleColor Background)
	{
		Write(Text, Foreground, Background);
		NextLine();
	}

	public void WriteLine(char Character)
	{
		Write(Character);
		NextLine();
	}

	public void WriteLine(char Character, ConsoleColor Foreground, ConsoleColor Background)
	{
		Write(Character, Foreground, Background);
		NextLine();
	}

	public void Write(string Text)
	{
		foreach (char c in Text)
			Write(c);
	}

	public void Write(string Text, ConsoleColor Foreground, ConsoleColor Background)
	{
		for (int i = 0; i <  Text.Length; i++)
		{
			if (ReachedWidthLimit() || i == Text.Length - 1)
				AddClear();

			Write(Text[i], Foreground, Background);
		}
	}

	public void Write(char Character)
	{
		ModifyChar(Character);
		AdvanceCursor();
	}

	public void Write(char Character, ConsoleColor Foreground, ConsoleColor Background)
	{
		ModifyChar(Character, Foreground, Background);
		AdvanceCursor();
	}

	public void WriteCharInPlace(char Character) => ModifyChar(Character);

	public void WriteCharInPlace(char Character, ConsoleColor Foreground, ConsoleColor Background) => ModifyChar(Character, Foreground, Background);
}
