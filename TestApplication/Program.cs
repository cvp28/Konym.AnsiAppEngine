using System.Diagnostics;

using Konym.AnsiAppEngine;

Console.ReadKey(true);

Stopwatch sw = new();

int i = 1;
int LoadingCounter = 0;
Label HelloWorldLabel;
Label CurrentThreadLabel;
Label FPSLabel;
Label KeyPressLabel;
Label DimensionsLabel;
Label TimeLabel;
Textbox TextBox;

void OnUpdate(State s)
{
	FPSLabel.Text = $"Last frame: {s.LastFrameTime} ms :: {s.FPS} FPS";
	CurrentThreadLabel.Text = Thread.CurrentThread.Name;
	KeyPressLabel.ForegroundColor = Sequences.RandomForeground();
	TimeLabel.Text = $"{Math.Floor(sw.Elapsed.TotalSeconds)} seconds since application start";

	var dim = Dimensions.Current;
	DimensionsLabel.Text = $"WW: {dim.WindowWidth} WH: {dim.WindowHeight} BW: {dim.BufferWidth} BH: {dim.BufferHeight}";

	if (s.KeyPressed)
	{
		KeyPressLabel.Text = $"Last Key: {s.KeyInfo.Key}";
		
		char c = s.KeyInfo.KeyChar;
		
		if (char.IsLetterOrDigit(c) || char.IsPunctuation(c) || c == ' ')
			TextBox.Write(c);

		switch (s.KeyInfo.Key)
		{
			case ConsoleKey.F1:
				HelloWorldLabel.Text = $"Hello, World! {i}";
				i++;
				return;

			case ConsoleKey.Escape:
				Engine.SignalExit();
				break;

			case ConsoleKey.F2:
				TextBox.Write("Loading... ");
				break;

			case ConsoleKey.F3:
				switch (LoadingCounter)
				{
					case 0:
						TextBox.WriteCharInPlace('|');
						break;

					case 1:
						TextBox.WriteCharInPlace('/');
						break;

					case 2:
						TextBox.WriteCharInPlace('-');
						break;

					case 3:
						TextBox.WriteCharInPlace('\\');
						break;
				}
				if (LoadingCounter == 3)
					LoadingCounter = 0;
				else
					LoadingCounter++;
				break;

			case ConsoleKey.Backspace:
			case ConsoleKey.UpArrow:
			case ConsoleKey.DownArrow:
			case ConsoleKey.LeftArrow:
			case ConsoleKey.RightArrow:
			case ConsoleKey.W:
			case ConsoleKey.S:
			case ConsoleKey.A:
			case ConsoleKey.D:
				HandleTextboxMovement(s.KeyInfo.Key);
				break;
		}
	}
}

void HandleTextboxMovement(ConsoleKey key)
{
	switch (key)
	{
		case ConsoleKey.Backspace:
			TextBox.Clear();
			break;
		
		case ConsoleKey.UpArrow:
			if (TextBox.Y > 0)
				TextBox.Y -= 1;
			break;

		case ConsoleKey.DownArrow:
			if (TextBox.Y + TextBox.Height + 2 < Console.WindowHeight)
				TextBox.Y += 1;
			break;

		case ConsoleKey.LeftArrow:
			if (TextBox.X > 0)
				TextBox.X -= 1;
			break;

		case ConsoleKey.RightArrow:
			if (TextBox.X + TextBox.Width + 2 < Console.WindowWidth)
				TextBox.X += 1;
			break;

		//	case ConsoleKey.W:
		//		if (TextBox.Height > 0)
		//			TextBox.Height -= 1;
		//		break;
		//	
		//	case ConsoleKey.S:
		//		if (TextBox.Y + TextBox.Height + 2 < Console.WindowHeight)
		//			TextBox.Height += 1;
		//		break;
		//	
		//	case ConsoleKey.A:
		//		if (TextBox.Width > 0)
		//			TextBox.Width -= 1;
		//		break;
		//	
		//	case ConsoleKey.D:
		//		if (TextBox.X + TextBox.Width + 2 < Console.WindowWidth)
		//			TextBox.Width += 1;
		//		break;
	}
}

Engine.Initialize();

FPSLabel = new(0, 0);

DimensionsLabel = new(0, 1);

KeyPressLabel = new(0, 2);
KeyPressLabel.Text = "Last Key: ";

HelloWorldLabel = new(0, 3);
HelloWorldLabel.Text = "Hello, World!";
HelloWorldLabel.ForegroundColor = Sequences.FgBrightBlue;

CurrentThreadLabel = new(0, Console.WindowHeight - 1);
CurrentThreadLabel.ForegroundColor = Sequences.FgBrightGreen;

TimeLabel = new(0, 4);
TimeLabel.ForegroundColor = Sequences.FgBrightYellow;

TextBox = new(20, 10, 50, 18);

Engine.AddWidget(HelloWorldLabel);
Engine.AddWidget(FPSLabel);
Engine.AddWidget(KeyPressLabel);
Engine.AddWidget(CurrentThreadLabel);
Engine.AddWidget(DimensionsLabel);
Engine.AddWidget(TimeLabel);
Engine.AddWidget(TextBox);

Engine.OnUpdate = OnUpdate;

sw.Start();
Engine.Run();