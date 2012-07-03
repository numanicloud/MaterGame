using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nac.Geometory;
using Nac.Helpers;

namespace NacXna.Helpers
{
	using MyMath = Nac.Helpers.MathHelper;
	using NacRectangle = Nac.Geometory.Rectangle;
	using XnaRectangle = Microsoft.Xna.Framework.Rectangle;
	using System.Collections;

	public static class Helper
	{
		#region キャスト
		public static Vector2 ToXna( this Vector2D obj )
		{
			return new Vector2( (float)obj.X, (float)obj.Y );
		}
		public static Vector2D ToNac( this Vector2 obj )
		{
			return new Vector2D( obj.X, obj.Y );
		}

		public static Vector2 ToVector2( this Point point )
		{
			return new Vector2( point.X, point.Y );
		}
		public static Vector2D ToVector2D( this Point point )
		{
			return new Vector2D( point.X, point.Y );
		}
		public static Point ToPoint( this Vector2 vector )
		{
			return new Point( (int)vector.X, (int)vector.Y );
		}
		public static Point ToPoint( this Vector2D vector )
		{
			return new Point( (int)vector.X, (int)vector.Y );
		}

		public static XnaRectangle ToXna( this NacRectangle obj )
		{
			return new XnaRectangle( (int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.X, (int)obj.Size.Y );
		}
		public static NacRectangle ToNac( this XnaRectangle obj )
		{
			return new NacRectangle( obj.X, obj.Y, obj.Width, obj.Height );
		}
		#endregion

		public static Vector2 GetSize( this XnaRectangle obj )
		{
			return new Vector2( obj.Width, obj.Height );
		}

		public static XnaRectangle Bind( XnaRectangle shape, XnaRectangle Binding )
		{
			var min = new Vector2();
			var max = new Vector2();

			min.X = MyMath.Floor( shape.Location.X, Binding.Left );
			min.Y = MyMath.Floor( shape.Location.Y, Binding.Top );
			shape.Location = min.ToPoint();

			max.X = MyMath.Ceiling( shape.Location.X + shape.Width, Binding.Right ) - shape.Width;
			max.Y = MyMath.Ceiling( shape.Location.Y + shape.Height, Binding.Bottom ) - shape.Height;
			shape.Location = max.ToPoint();

			return shape;
		}

		public static T GetService<T>( this GameServiceContainer services )
		{
			return (T)services.GetService( typeof( T ) );
		}
	}
}
