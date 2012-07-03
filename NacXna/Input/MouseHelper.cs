using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nac.Geometory;

namespace NacXna.Input
{
	public struct MouseCount
	{
		public Vector2D Position { get; private set; }
		public Vector2D Velocity { get; private set; }
		public int LeftButton { get; private set; }
		public int RightButton { get; private set; }
		public int MiddleButton { get; private set; }
		public int Wheel { get; private set; }

		internal MouseCount( Vector2D position, Vector2D velocity, int left, int right, int middle, int wheel )
			: this()
		{
			this.Position = position;
			this.Velocity = velocity;
			this.LeftButton = left;
			this.RightButton = right;
			this.MiddleButton = middle;
			this.Wheel = wheel;
		}
	}

	public class MouseHelper : GameComponent
	{
		Vector2D prevPosition { get; set; }
		Vector2D position { get; set; }

		int leftButton { get; set; }
		int rightButton { get; set; }
		int middleButton { get; set; }

		int prevWheel { get; set; }
		int wheel { get; set; }

		public MouseHelper( Game game )
			: base( game )
		{
			var state = Mouse.GetState();
			position = new Vector2D( state.X, state.Y );
			leftButton = rightButton = middleButton = 0;
			wheel = 0;
		}
		public override void Update( GameTime gameTime )
		{
			var state = Mouse.GetState();
			prevPosition = position;
			position = new Vector2D( state.X, state.Y );

			if( state.LeftButton == ButtonState.Pressed ) leftButton++;
			else leftButton = 0;

			if( state.RightButton == ButtonState.Pressed ) rightButton++;
			else rightButton = 0;

			if( state.MiddleButton == ButtonState.Pressed ) middleButton++;
			else middleButton = 0;

			prevWheel = wheel;
			wheel = state.ScrollWheelValue;
		}
		public MouseCount GetState()
		{
			return new MouseCount( position, position - prevPosition, leftButton, rightButton, middleButton, wheel - prevWheel );
		}
	}
}
