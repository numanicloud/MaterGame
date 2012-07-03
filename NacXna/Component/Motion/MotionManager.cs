using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nac.Geometory;
using Microsoft.Xna.Framework;
using System.Collections;

namespace NacXna.Component
{
	public class MotionManager : GameComponent, IEnumerable<IMotion>
	{
		public Vector2D Position;
		public Vector2D Previous;
		public Vector2D Diff
		{
			get { return Position - Previous; }
		}
		private List<IMotion> Motions { get; set; }

		public MotionManager( Game game, Vector2D position )
			: base( game )
		{
			Motions = new List<IMotion>();
			this.Position = position;
		}
		public override void Update( GameTime gameTime )
		{
			Previous = Position;

			Motions.RemoveAll( _ => !_.Enabled );

			if( Motions.Count != 0 )
			{
				var level = Motions.Max( _ => _.ExclusionLevel );
				Motions.Where( _ => _.ExclusionLevel >= level ).ToList().ForEach( _ => _.Move( ref Position ) );
			}

			base.Update( gameTime );
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public IEnumerator<IMotion> GetEnumerator()
		{
			foreach( var item in Motions )
				yield return item;
		}

		public void Add( IMotion motion )
		{
			Motions.Add( motion );
		}
		public void AddRange( params IMotion[] motions )
		{
			Motions.AddRange( motions );
		}
		public void Remove( IMotion motion )
		{
			Motions.Remove( motion );
		}
		public void Clear()
		{
			Motions.Clear();
		}
	}
}
