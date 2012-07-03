using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nac.Geometory;

namespace NacXna.Component
{
	public class AccelMotion : IMotion
	{
		public Vector2D Velocity;
		public Vector2D Accel;
		public float ExclusionLevel { get { return 0; } }
		public bool Enabled { get; set; }

		public AccelMotion( Vector2D velocity, Vector2D accel )
		{
			this.Velocity = velocity;
			this.Accel = accel;
			Enabled = true;
		}
		public AccelMotion( float vx, float vy, float ax, float ay )
			: this( new Vector2D( vx, vy ), new Vector2D( ax, ay ) )
		{
		}

		public void Move( ref Vector2D position )
		{
			position += Velocity;
			Velocity += Accel;
		}
		public Vector2D Move( Vector2D position )
		{
			position += Velocity;
			Velocity += Accel;
			return position;
		}
	}
}
