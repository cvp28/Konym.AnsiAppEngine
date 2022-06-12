using System.Diagnostics;

using Konym.AnsiAppEngine;

Console.ReadKey(true);

Stopwatch sw = new();

int i = 1;
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

		switch (s.KeyInfo.Key)
		{
			case ConsoleKey.F1:
				HelloWorldLabel.Text = $"Hello, World! {i}";
				i++;
				return;

			case ConsoleKey.Escape:
				Engine.SignalExit();
				break;

			case ConsoleKey.UpArrow:
				if (TextBox.Y > 0)
					TextBox.Y -= 1;
				break;

			case ConsoleKey.DownArrow:
				if (TextBox.Y < Console.WindowHeight)
					TextBox.Y += 1;
				break;

			case ConsoleKey.LeftArrow:
				if (TextBox.X > 0)
					TextBox.X -= 1;
				break;

			case ConsoleKey.RightArrow:
				if (TextBox.X < Console.WindowWidth)
					TextBox.X += 1;
				break;
		}
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

TextBox = new()
{
	X = 20,
	Y = 10,
	Width = 25,
	Height = 9
};

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