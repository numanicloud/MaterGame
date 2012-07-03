using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nac.Geometory;
using NacXna.Input;

namespace MaterGame
{
	using MyMath = Nac.Helpers.MathHelper;

	class Mater : GameComponent
	{
		float value_;
		public float Value
		{
			get { return value_; }
			set { value_ = MyMath.Clamp( value, -100, 100 ); }
		}
		public float Speed { get; set; }
		public int Score { get; set; }
		KeyboardHelper keyboard { get; set; }
		ScoreShowingManager scoreShows { get; set; }

		public Mater( Game game )
			: base( game )
		{
		}

		public override void Initialize()
		{
			Score = 0;
			Value = 0;
			Speed = 50;

			keyboard = new KeyboardHelper( Game );
			keyboard.Configurate( Keys.Left, MeanOfKey.Mean1 );
			keyboard.Configurate( Keys.Right, MeanOfKey.Mean2 );

			scoreShows = new ScoreShowingManager( Game );

			Game.Components.Add( keyboard );
			Game.Components.Add( scoreShows );
			base.Initialize();
		}
		protected override void Dispose( bool disposing )
		{
			Game.Components.Remove( keyboard );
			Game.Components.Remove( scoreShows );
			base.Dispose( disposing );
		}
		public override void Update( GameTime gameTime )
		{
			if( Value >= 100 || Value <= -100 )
			{
				Enabled = false;
				base.Update( gameTime );
				return;
			}

			Value += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			var count = keyboard.GetState();
			if( count[MeanOfKey.Mean1] == 1 && Speed > 0 ||
				count[MeanOfKey.Mean2] == 1 && Speed < 0 )
			{
				int add = (int)( Speed * Value );
				Score += add;

				var pos = new Vector2D( ChickenRace.materCenter + ChickenRace.box.Size.X/2 * Value / 100.0f - 30, ChickenRace.box.Top - 16 );
				scoreShows.Add( new ScoreShowing( Game, pos, add, Value > 0 ? ChickenRace.Red : ChickenRace.Blue ) );

				Speed += Math.Sign( Speed ) * 3.5f;
				Speed = -Speed;
			}

			base.Update( gameTime );
		}

	}
}
