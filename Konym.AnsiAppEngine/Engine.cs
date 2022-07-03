using System.Diagnostics;

namespace Konym.AnsiAppEngine;

public static class Engine
{
	private static Thread ApplicationThread;
	private static Thread RenderThread;

	private static bool AppActive;
	private static bool RenderActive;

	public static IRenderer Screen;
	private static Stopwatch RenderTimer;
	private static long FPS;
	private static long FrameTime;

	private static Dimensions LastDimensions;

	public static Widget FocusedWidget { get; set; } = null;
	private static List<Widget> Widgets;

	public static Action<State> OnUpdate { get; set; } = null;


	public static void Initialize(bool UseKernel32API)
	{
		ApplicationThread = new(ApplicationLoop);
		ApplicationThread.IsBackground = true;
		ApplicationThread.Name = "AnsiAppEngine Application Loop Thread";

		RenderThread = new(RenderLoop);
		RenderThread.IsBackground = false;
		RenderThread.Name = "AnsiAppEngine Screen Renderer Loop Thread";

		RenderTimer = new();

		LastDimensions = Dimensions.Current;

		if (UseKernel32API)
			Screen = new NativeScreen();
		else
			Screen = new Screen();

		Widgets = new();

		AppActive = false;
		RenderActive = false;
	}

	public static void ApplicationLoop()
	{
		State s = new();

		while (AppActive)
		{
			s.FPS = FPS;
			s.LastFrameTime = FrameTime;

			// Check for updated console dimensions and update current state accordingly
			#region Update Dimensions
			Dimensions CurrentDimensions = Dimensions.Current;

			bool WindowWidthChanged = CurrentDimensions.WindowWidth != LastDimensions.WindowWidth;
			bool WindowHeightChanged = CurrentDimensions.WindowHeight != LastDimensions.WindowHeight;
			bool BufferWidthChanged = CurrentDimensions.BufferWidth != LastDimensions.BufferWidth;
			bool BufferHeightChanged = CurrentDimensions.BufferHeight != LastDimensions.BufferHeight;

			// Dimensions were changed if any of those conditions were true
			s.DimensionsChanged = WindowWidthChanged || WindowHeightChanged || BufferWidthChanged || BufferHeightChanged;

			// Set current dimensions
			s.Dimensions = CurrentDimensions;

			// Update last dimensions
			LastDimensions = CurrentDimensions;
			#endregion

			// Check for user input and update current state accordingly
			#region Update User Input
			if (Console.KeyAvailable)
			{
				var cki = Console.ReadKey(true);
				s.KeyPressed = true;
				s.KeyInfo = cki;

				if (FocusedWidget is not null)
				{
					FocusedWidget.OnConsoleKey(cki);
					s.InputAlreadyHandled = true;
				}
				else
				{
					s.InputAlreadyHandled = false;
				}
			}
			else
			{
				s.KeyPressed = false;
				s.KeyInfo = default;
			}
			#endregion

			if (OnUpdate is not null)
				OnUpdate(s);
		}
	}

	private static void RenderLoop()
	{
		while (RenderActive)
		{
			RenderTimer.Restart();
			Screen.Render(Widgets);
			RenderTimer.Stop();

			// Calculate FPS
			FPS = Stopwatch.Frequency / RenderTimer.ElapsedTicks;
			FrameTime = RenderTimer.ElapsedMilliseconds;

			//Console.Title = $"{FPS} FPS";
		}
	}

	public static void Run()
	{
		Console.CursorVisible = false;

		AppActive = true;
		RenderActive = true;

		ApplicationThread.Start();
		RenderThread.Start();

		ApplicationThread.Join();
		RenderThread.Join();

		Console.ResetColor();
		Console.Clear();
	}

	public static void SignalExit()
	{
		AppActive = false;
		RenderActive = false;
	}

	public static void AddWidget(Widget Widget)
	{
		Widgets.Add(Widget);
	}

	public static void AddWidgets(params Widget[] Widgets)
	{
		foreach (Widget Widget in Widgets)
			AddWidget(Widget);
	}

	public static bool RemoveWidget(Widget Widget)
	{
		return Widgets.Remove(Widget);
	}
}