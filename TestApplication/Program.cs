using System.Diagnostics;

using Konym.AnsiAppEngine;

bool UseKernel32;

prompt:

Console.Write("Use Kernel32 Console API? (y/n): ");
var cki = Console.ReadKey(false);
Console.WriteLine();

if (cki.Key == ConsoleKey.Y)
	UseKernel32 = true;
else if (cki.Key == ConsoleKey.N)
	UseKernel32 = false;
else
	goto prompt;

Stopwatch sw = new();

int i = 1;

long LowestFPS = 900000;
long HighestFPS = 0;

Label HelloWorldLabel;
Label KeyBindingsLabel;
Label FPSLabel;
Label KeyPressLabel;
Label DimensionsLabel;
Label TimeLabel;
NewTextBox TextBox;
InputField Input;

bool DirectInput = false;

List<long> FPSList = new(10000);

void OnUpdate(State s)
{
	bool NewMinimum = s.FPS < LowestFPS;
	bool NewMaximum = s.FPS > HighestFPS;

	if (NewMinimum)
		LowestFPS = s.FPS;
	if (NewMaximum)
		HighestFPS = s.FPS;

	if (FPSList.Count == FPSList.Capacity)
		FPSList.RemoveAt(0);

	FPSList.Add(s.FPS);

	long AverageFPS = (long) FPSList.Average();

	FPSLabel.Text = $"Last frame : {s.LastFrameTime} ms :: {s.FPS} FPS (Lowest {LowestFPS} Highest {HighestFPS} Average {AverageFPS})";
	KeyBindingsLabel.Text = "F1: Increment Hello, World Label Counter    F2: Colored Output to Textbox    F3: Reset High/Low FPS trackers    F4: Reset Average FPS tracker    Esc: Exit";
	TimeLabel.Text = $"{Math.Floor(sw.Elapsed.TotalSeconds)} seconds since application start";

	var dim = Dimensions.Current;
	DimensionsLabel.Text = $"WW: {dim.WindowWidth} WH: {dim.WindowHeight} BW: {dim.BufferWidth} BH: {dim.BufferHeight}";


	if (s.KeyPressed)
	{
		KeyPressLabel.Text = $"Last Key: {s.KeyInfo.Key}";
		char c = s.KeyInfo.KeyChar;
		bool ValidChar = char.IsPunctuation(c) || char.IsLetterOrDigit(c) || c == ' ';
		
		if (DirectInput && ValidChar)
			TextBox.Write(c);

		switch (s.KeyInfo.Key)
		{
			case ConsoleKey.F1:
				HelloWorldLabel.Text = $"Hello, World! {i}";
				i++;
				return;

			case ConsoleKey.F2:
				TextBox.Write("Hello, World!", ConsoleColor.White, ConsoleColor.Green);
				break;

			case ConsoleKey.Escape:
				Engine.SignalExit();
				break;

			case ConsoleKey.F3:
				LowestFPS = 900000;
				HighestFPS = 0;
				break;

			case ConsoleKey.F4:
				FPSList.Clear();
				break;

			case ConsoleKey.F5:
				DirectInput = !DirectInput;

				Input.DrawCursor = DirectInput == false;

				if (!DirectInput)
					Engine.FocusedWidget = Input;
				else
					Engine.FocusedWidget = null;

				break;
		}
	}
}

Engine.Initialize(UseKernel32);

FPSLabel = new(0, 0);

DimensionsLabel = new(0, 1);

KeyPressLabel = new(0, 2);
KeyPressLabel.Text = "Last Key: ";

HelloWorldLabel = new(0, 3);
HelloWorldLabel.Text = "Hello, World!";
HelloWorldLabel.ForegroundColor = ConsoleColor.Blue;

KeyBindingsLabel = new(0, Console.WindowHeight - 1);
KeyBindingsLabel.ForegroundColor = ConsoleColor.Green;

TimeLabel = new(0, 4);
TimeLabel.ForegroundColor = ConsoleColor.Yellow;

TextBox = new(10, 10, 80, 25);

Input = new(10, 37, "> ");
Input.CursorForeground = ConsoleColor.White;
Input.CursorBackground = ConsoleColor.Green;
Input.OnInput += delegate (string TextInput)
{
	switch(TextInput.ToUpper())
	{
		case "CLEAR":
			TextBox.Clear();
			break;

		case "EXIT":
			Engine.SignalExit();
			break;
	}
};

Engine.AddWidget(FPSLabel);
Engine.AddWidget(HelloWorldLabel);
Engine.AddWidget(KeyPressLabel);
Engine.AddWidget(DimensionsLabel);
Engine.AddWidget(TimeLabel);
Engine.AddWidget(Input);
Engine.AddWidget(TextBox);

Engine.OnUpdate = OnUpdate;
Engine.FocusedWidget = Input;

sw.Start();
Engine.Run();