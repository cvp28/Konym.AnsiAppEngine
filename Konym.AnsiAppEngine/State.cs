using System;

namespace Konym.AnsiAppEngine;

public struct State
{
	public bool KeyPressed;
	public bool DimensionsChanged;

	public ConsoleKeyInfo KeyInfo;
	public Dimensions Dimensions;

	public long FPS;
	public long LastFrameTime;
}