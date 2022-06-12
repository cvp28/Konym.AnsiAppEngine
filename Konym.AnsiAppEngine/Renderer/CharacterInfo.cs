using System.Runtime.InteropServices;

namespace Konym.AnsiAppEngine;

[StructLayout(LayoutKind.Sequential)]
public struct CharacterInfo
{
	public Span<byte> Foreground
	{
		get => data.AsSpan().Slice(0, 5);

		set
		{
			for (int i = 0; i <= 4; i++)
				data[i] = value[i];
		}
	}

	public Span<byte> Background
	{
		get => data.AsSpan().Slice(5, 5);

		set
		{
			for (int i = 5; i <= 9; i++)
				data[i] = value[i - 5];
		}
	}

	public Span<byte> Control
	{
		get => data.AsSpan().Slice(10, 4);

		set
		{
			for (int i = 10; i <= 13; i++)
				data[i] = value[i - 10];
		}
	}

	public byte Character
	{
		get => data[14];
		set { data[14] = value; }
	}

	public byte[] data { get; private set; }

	public CharacterInfo()
	{
		data = new byte[15];
	}
}
