using Microsoft.Xna.Framework;
using System.IO;

namespace MaterGame
{
	/// <summary>
	/// 基底 Game クラスから派生した、ゲームのメイン クラスです。
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		public static Nac.Geometory.Vector2D Size = new Nac.Geometory.Vector2D( 720, 180 );
		GraphicsDeviceManager graphics;

		ChickenRace race { get; set; }

		public Game1()
		{
			graphics = new GraphicsDeviceManager( this );
			graphics.PreferredBackBufferWidth = (int)Size.X;
			graphics.PreferredBackBufferHeight = (int)Size.Y;
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			race = new ChickenRace( this );
			Components.Add( race );
			base.Initialize();
		}
	}
}
