using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections;

namespace NacXna.Component
{
	public class DrawableManager<T> : DrawableGameComponent, IEnumerable<T> where T : DrawableGameComponent
	{
		public bool AllowAdd { get; set; }
		public bool UsePool { get; set; }
		protected SpriteBatch spriteBatch { get; set; }
		private List<T> list { get; set; }
		private List<T> pool { get; set; }

		public int Count
		{
			get { return list.Count; }
		}
		public int PoolCount
		{
			get { return pool.Count; }
		}

		public DrawableManager( Game game )
			: base( game )
		{
			AllowAdd = true;
			UsePool = true;
			list = new List<T>();
			pool = new List<T>();
		}
		public override void Initialize()
		{
			base.Initialize();
			spriteBatch = new SpriteBatch( GraphicsDevice );
		}
		public override void Update( GameTime gameTime )
		{
			var find = list.FindAll( obj => !obj.Enabled );
			foreach( var item in find )
			{
				list.Remove( item );
				if( UsePool ) pool.Add( item );
			}
			list.ForEach( x => x.Update( gameTime ) );

			base.Update( gameTime );
		}
		public override void Draw( GameTime gameTime )
		{
			list.Where( x => x.Visible ).ToList().ForEach( x => x.Draw( gameTime ) );
			base.Draw( gameTime );
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public IEnumerator<T> GetEnumerator()
		{
			foreach( var item in list )
				yield return item;
		}
		public void Add( T actor )
		{
			if( AllowAdd && actor != null )
			{
				actor.Initialize();
				list.Add( actor );
			}
		}
		public void AddRange( T[] actors )
		{
			if( !AllowAdd ) return;
			foreach( var item in actors )
				Add( item );
		}
		public void Clear()
		{
			if( UsePool )
				foreach( var item in list )
					pool.Add( item );
			list.Clear();
		}
		public void ClearPool()
		{
			if( UsePool )
				pool.Clear();
		}
		/// <summary>
		/// プールからオブジェクトを引き出します。引き出したオブジェクトはプールから削除されます。
		/// </summary>
		/// <typeparam name="S">引き出すオブジェクトの型。</typeparam>
		/// <returns>引き出したオブジェクト。見つからなかった場合はnull。</returns>
		public S Pull<S>() where S : T
		{
			if( !UsePool ) return null;

			T result = pool.Find( obj => obj is S );
			if( result != null )
			{
				pool.Remove( result );
				return result as S;
			}
			return null;
		}
		/// <summary>
		/// プールからオブジェクトを引き出します。引き出したオブジェクトはプールから削除されます。
		/// </summary>
		/// <returns>引き出したオブジェクト。プールが空だった場合はnull。</returns>
		public T Pull()
		{
			if( !UsePool ) return null;

			T result = null;
			if( pool.Count != 0 )
			{
				result = pool[0];
				pool.Remove( result );
			}
			return result;
		}
	}
}
