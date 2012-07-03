using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NacXna.Helpers;

namespace NacXna.Draw
{
	using MyMath = Nac.Helpers.MathHelper;
	using NacRectangle = Nac.Geometory.Rectangle;

	public class Wipe : DrawableGameComponent
	{
		class Order : GameComponent
		{
			Wipe owner { get; set; }
			float end { get; set; }
			int time { get; set; }
			int wait { get; set; }
			Action onFinish;

			float diff { get; set; }

			public Order( Wipe owner, float end, int time, int wait, Action onFinish )
				: base( owner.Game )
			{
				this.owner = owner;
				this.end = end;
				this.time = time;
				this.wait = wait;
				this.onFinish = onFinish;
				diff = end - owner.brightness;
			}

			public override void Update( GameTime gameTime )
			{
				if( wait > 0 )
				{
					wait--;
				}
				else
				{
					float d = diff / time;

					if( Math.Abs( end - owner.brightness ) < Math.Abs( d ) + Single.Epsilon )
					{
						owner.brightness = end;
						if( onFinish != null ) onFinish();
						Enabled = false;
					}
					else
					{
						owner.brightness += d;
					}
				}
				base.Update( gameTime );
			}
		}

		#region フィールド
		float brightness_;
		float brightness
		{
			get { return brightness_; }
			set { brightness_ = MyMath.Clamp( value, 0, 1 ); }
		}
		Order currentOrder = null;
		PrimitiveShader primitive;
		public bool IsWiping
		{
			get { return currentOrder != null; }
		}
		#endregion

		public Wipe( Game game, float firstBright )
			: base( game )
		{
			brightness = firstBright;
			DrawOrder = 1;
		}
		protected override void LoadContent()
		{
			primitive = new PrimitiveShader( GraphicsDevice );
			base.LoadContent();
		}

		public override void Update( GameTime gameTime )
		{
			if( currentOrder != null && !currentOrder.Enabled )
			{
				Game.Components.Remove( currentOrder );
				currentOrder = null;
			}
			base.Update( gameTime );
		}
		public override void Draw( GameTime gameTime )
		{
			if( brightness == 1 ) return;

			primitive.Begin();
			primitive.DrawRectangle( GraphicsDevice.Viewport.Bounds.ToNac(), Color.Black * ( 1.0f - brightness ), true );
			primitive.End();
			base.Draw( gameTime );
		}

		public void OrderWipe( float endBright, int time, int wait, Action onFinish )
		{
			Game.Components.Remove( currentOrder );
			currentOrder = new Order( this, endBright, time, wait, onFinish );
			Game.Components.Add( currentOrder );
		}
	}
}
