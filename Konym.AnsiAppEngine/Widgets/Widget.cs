﻿
namespace Konym.AnsiAppEngine;

public abstract class Widget
{
	public Dictionary<ConsoleKey, Action> KeyActions { get; set; }

	public Widget()
	{
		KeyActions = new();
	}

	public abstract void Draw(IRenderer s);

	public abstract void OnConsoleKey(ConsoleKeyInfo cki);

	public void AddKeyAction(ConsoleKey Key, Action Action)
	{
		if (KeyActions.ContainsKey(Key))
			return;

		KeyActions.Add(Key, Action);
	}

	public void OverrideKeyAction(ConsoleKey Key, Action Action)
	{
		if (!KeyActions.ContainsKey(Key))
			return;

		KeyActions[Key] = Action;
	}

	public void RemoveKeyAction(ConsoleKey Key)
	{
		if (!KeyActions.ContainsKey(Key))
			return;

		KeyActions.Remove(Key);
	}
}
