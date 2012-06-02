using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nac.Helpers;
using NacXna.Draw;
using NacXna.Helpers;

namespace MaterGame
{
	using NacRectangle = Nac.Geometory.Rectangle;

	[Serializable]
	public struct ScoreData
	{
		public int Score { get; set; }
		public float Speed { get; set; }
		public DateTime Time { get; set; }

		public override string ToString()
		{
			return string.Format( "{0:00000000} speed:{1:000} {2:yy/MM/dd HH:mm}", Score, Speed, Time );
		}
		public override bool Equals( object obj )
		{
			if( obj is ScoreData )
			{
				var s = (ScoreData)obj;
				return s.Score == Score &&
					s.Speed == Speed &&
					s.Time == Time;
			}
			return false;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public class ChickenRace : DrawableGameComponent
	{
		#region フィールド
		static readonly int scoreToBeSaved = 5;
		static readonly string directory = "ChickenRace";
		static readonly string saveFileName = "savedata.sav";
		public static readonly NacRectangle box = new NacRectangle( 30, 80, Game1.Size.X-60, Game1.Size.Y-90 );
		public static readonly float materCenter = ( box.Left + box.Right ) / 2;
		public static readonly Color Text = Color.White;
		public static readonly Color Red = new Color( 255, 70, 90 );
		public static readonly Color Blue = new Color( 40, 160, 255 );

		PrimitiveShader shader { get; set; }
		SpriteBatch batch { get; set; }
		SpriteFont font { get; set; }
		Wipe wipe { get; set; }
		State state { get; set; }
		#endregion

		#region ステートマシン
		abstract class State : DrawableGameComponent
		{
			protected ChickenRace owner { get; set; }
			public State( Game game, ChickenRace owner )
				: base( game )
			{
				this.owner = owner;
			}
		}
		class PrepareState : State
		{
			public PrepareState( Game game, ChickenRace owner )
				: base( game, owner )
			{
			}

			public override void Update( GameTime gameTime )
			{
				var state = Keyboard.GetState();
				if( state[Keys.Enter] == KeyState.Down )
				{
					owner.wipe.OrderWipe( 0, 0.3f, 0, () =>
					{
						owner.state = new PlayingState( Game, owner );
						Game.Components.Remove( this );
						Game.Components.Add( owner.state );
					} );
				}

				base.Update( gameTime );
			}
			public override void Draw( GameTime gameTime )
			{
				owner.batch.Begin();
				owner.batch.DrawString( owner.font, "Press Enter To Start", box.GetCenter().ToXna() - new Vector2( 10*12, 30 ), Text );
				owner.batch.End();
				base.Draw( gameTime );
			}
		}
		class PlayingState : State
		{
			Mater mater { get; set; }

			public PlayingState( Game game, ChickenRace owner )
				: base( game, owner )
			{
			}

			public override void Initialize()
			{
				mater = new Mater( Game );
				owner.wipe.OrderWipe( 1, 0.8f, 0, () =>
				{
					Game.Components.Add( mater );
				} );
				base.Initialize();
			}
			protected override void Dispose( bool disposing )
			{
				Game.Components.Remove( mater );
				base.Dispose( disposing );
			}
			public override void Update( GameTime gameTime )
			{
				if( !mater.Enabled )
				{
					owner.wipe.OrderWipe( 0, 0.25f, 0.75f, () =>
					{
						owner.state = new GameOverState( Game, owner, mater.Score, mater.Speed );
						Game.Components.Remove( this );
						Game.Components.Add( owner.state );
					} );
					Enabled = false;
				}
				base.Update( gameTime );
			}
			public override void Draw( GameTime gameTime )
			{
				owner.shader.Begin();
				var right = materCenter + box.Size.X/2 * mater.Value / 100.0f;
				owner.shader.DrawRectangle( materCenter, box.Top, right, box.Bottom, mater.Value > 0 ? Red : Blue, true );
				owner.shader.DrawRectangle( box, Text, false );
				owner.shader.End();

				owner.batch.Begin();
				owner.batch.DrawString( owner.font, string.Format( "Score:{0}", mater.Score ), Vector2.Zero, Text );
				owner.batch.DrawString( owner.font, string.Format( "Speed:{0}", mater.Speed ), new Vector2( 200, 0 ), Text );
				owner.batch.End();

				base.Draw( gameTime );
			}
		}
		class GameOverState : State
		{
			ScoreData result;
			List<ScoreData> scores;

			public GameOverState( Game game, ChickenRace owner, int score, float speed )
				: base( game, owner )
			{
				result = new ScoreData { Score = score, Speed = speed, Time = DateTime.Now };
			}

			public override void Initialize()
			{
				scores = new List<ScoreData>();
				var load = StorageHelper.LoadData<ScoreData[]>( directory, saveFileName );
				
				if( load != null )
					scores.AddRange( load );
				scores.Add( result );

				scores = scores.OrderByDescending( x => x.Score ).ToList();

				StorageHelper.SaveData<ScoreData[]>( directory, saveFileName, scores.Take( scoreToBeSaved ).ToArray() );

				Enabled = false;
				owner.wipe.OrderWipe( 1, 0.25f, 0, () => Enabled = true );
				base.Initialize();
			}
			public override void Update( GameTime gameTime )
			{
				var state = Keyboard.GetState();
				if( state[Keys.Enter] == KeyState.Down )
				{
					owner.state = new PlayingState( Game, owner );
					Game.Components.Remove( this );
					Game.Components.Add( owner.state );
				}

				base.Update( gameTime );
			}
			public override void Draw( GameTime gameTime )
			{
				owner.batch.Begin();
				owner.batch.DrawString( owner.font, "Game Over", new Vector2( 20, 15 ), Color.Red );
				owner.batch.DrawString( owner.font, string.Format( "Score:{0}", result.Score ), new Vector2( 20, 60 ), Color.Red );
				owner.batch.DrawString( owner.font, "Press Enter To Continue", new Vector2( 20, 120 ), Text );

				for( int i = 0; i < scores.Count && i < scoreToBeSaved; i++ )
				{
					owner.batch.DrawString( owner.font, scores[i].ToString(), new Vector2( 320, 15 + i*30 ), result.Equals( scores[i] ) ? Color.Red : Text );
				}

				owner.batch.End();
				base.Draw( gameTime );
			}
		}
		#endregion

		public ChickenRace( Game game )
			: base( game )
		{
			DrawOrder = 0;
		}

		public override void Initialize()
		{
			state = new PrepareState( Game, this );
			Game.Components.Add( state );
			base.Initialize();
		}
		protected override void LoadContent()
		{
			shader = new PrimitiveShader( GraphicsDevice );
			batch = new SpriteBatch( GraphicsDevice );
			font = Game.Content.Load<SpriteFont>( "Normal" );
			wipe = new Wipe( Game, 1 );
			Game.Components.Add( wipe );
			base.LoadContent();
		}

		public override void Draw( GameTime gameTime )
		{
			GraphicsDevice.Clear( Color.Black );
			base.Draw( gameTime );
		}
	}
}
