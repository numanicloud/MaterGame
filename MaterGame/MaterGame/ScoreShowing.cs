using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nac.Geometory;
using NacXna.Helpers;
using NacXna.Motion;

namespace MaterGame
{
	class ScoreShowing : GameComponent
	{
		public Vector2D Position;
		public int Score { get; set; }
		public Color Color;
		public float Trans { get; set; }
		MotionManager motion { get; set; }

		public ScoreShowing( Game game, Vector2D position, int score, Color color )
			: base( game )
		{
			this.Score = score;
			this.Color = color;
			this.Position = position;
			Trans = 1;
			motion = new MotionManager( game, position );
			motion.Motions.Add( new AccelMotion( new Vector2D( 0, -110 ), new Vector2D( 0, 120 ) ) );
		}
		public override void Update( GameTime gameTime )
		{
			motion.Update( gameTime );
			Position = motion.Position;

			Trans -= 0.01f;
			if( Trans <= 0.01f )
				Enabled = false;

			base.Update( gameTime );
		}
	}

	class ScoreShowingManager : DrawableGameComponent
	{
		List<ScoreShowing> scores { get; set; }
		SpriteBatch batch { get; set; }
		SpriteFont font { get; set; }

		public ScoreShowingManager( Game game )
			: base( game )
		{
			scores = new List<ScoreShowing>();
		}

		protected override void LoadContent()
		{
			batch = new SpriteBatch( GraphicsDevice );
			font = Game.Content.Load<SpriteFont>( "Normal" );
			base.LoadContent();
		}
		public override void Update( GameTime gameTime )
		{
			scores.ForEach( _ => _.Update( gameTime ) );
			base.Update( gameTime );
		}
		public override void Draw( GameTime gameTime )
		{
			batch.Begin();
			foreach( var item in scores )
			{
				batch.DrawString( font, item.Score.ToString(), item.Position.ToXna(), item.Color * item.Trans );
			}
			batch.End();
			base.Draw( gameTime );
		}

		public void Add( ScoreShowing obj )
		{
			scores.Add( obj );
		}

	}
}
