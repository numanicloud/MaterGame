﻿using System;
using System.IO;

namespace MaterGame
{
#if WINDOWS || XBOX
	static class Program
	{
		/// <summary>
		/// アプリケーションのメイン エントリー ポイントです。
		/// </summary>
		static void Main( string[] args )
		{
			using( Game1 game = new Game1() )
			{
				game.Run();
			}
		}
	}
#endif
}

