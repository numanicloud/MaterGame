using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nac.Geometory;
using Nac;

namespace NacXna.Component
{
	public class TowardMotion : IMotion
	{
		public float ExclusionLevel { get { return 1; } }
		public bool Enabled { get; set; }
		public float paramater { get; set; }

		BezierValue v;
		Vector2D from;
		Vector2D to;
		float diff { get; set; }
		int restTime { get; set; }

		public TowardMotion( Vector2D from, Vector2D to, int time, float beginWeight = 1, float endWeight = 1 )
		{
			v = new BezierValue( beginWeight, endWeight );
			this.from = from;
			this.to = to;
			this.paramater = 0;
			this.diff = 1.0f / time;
			this.restTime = time;
			Enabled = true;
		}

		public void Move( ref Vector2D position )
		{
			position = Move( position );
		}
		public Vector2D Move( Vector2D position )
		{
			if( restTime <= 0 )
			{
				Enabled = false;
				paramater = 1;
				return to;
			}

			paramater = MathHelper.Clamp( paramater + diff, 0, 1 );
			restTime--;
			return from + v[paramater] * ( to - from );
		}
	}
}
