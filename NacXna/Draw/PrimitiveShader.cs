using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nac.Geometory;
using Nac.Helpers;
using NacXna.Helpers;

namespace NacXna.Draw
{
	using NacGeo = Nac.Geometory;
	using MyMath = Nac.Helpers.MathHelper;

	public class PrimitiveShader
	{
		static Texture2D pixel;
		SpriteBatch spriteBatch;

		public PrimitiveShader( GraphicsDevice graphicDevice )
		{
			this.spriteBatch = new SpriteBatch( graphicDevice );
			if( pixel == null )
			{
				pixel = new Texture2D( graphicDevice, 1, 1, false, SurfaceFormat.Color );
				pixel.SetData<uint>( new uint[] { 0xffffffffu } );
			}
		}
		public static void Dispose()
		{
			pixel.Dispose();
		}

		public void Begin()
		{
			spriteBatch.Begin();
		}
		public void End()
		{
			spriteBatch.End();
		}

		public void DrawPixel( float x, float y, Color color )
		{
			DrawPixel( new Vector2D( x, y ), color );
		}
		public void DrawPixel( Vector2D position, Color color, float layerDepth = 0.0f )
		{
			spriteBatch.Draw( pixel, position, color: color, layerDepth: layerDepth );
		}

		public void DrawLine( float beginX, float beginY, float endX, float endY, Color color )
		{
			DrawLine( new Vector2D( beginX, beginY ), new Vector2D( endX, endY ), color );
		}
		public void DrawLine( Vector2D begin, Vector2D end, Color color )
		{
			Vector2D draw = new Vector2D( begin.X, begin.Y );
			Vector2D distance = new Vector2D( ( end - begin ).X, ( end - begin ).Y );
			Vector2D diff = new Vector2D();

			diff = distance / Math.Max( Math.Abs( distance.X ), Math.Abs( distance.Y ) );

			for( ; ( draw - begin ).SquaredLength <= distance.SquaredLength; draw += diff )
				spriteBatch.Draw( pixel, draw.ToXna(), color );
		}
		public void DrawLine( Segment segment, Color color )
		{
			DrawLine( segment.Begin, segment.End, color );
		}

		public void DrawHorizon( float height, float left, float right, Color color )
		{
			for( int i = (int)left; i <= right; i++ )
				DrawPixel( i, height, color );
		}

		public void DrawBezier( Bezier bezier, float diff, Color color )
		{
			float f = 0.0f;
			for( ; f + diff < 1.0f; f += diff )
				DrawLine( bezier[f], bezier[f+diff], color );
			DrawLine( bezier[f], bezier[1.0f], color );
		}

		public void DrawRectangle( float left, float top, float right, float bottom, Color color, bool fill )
		{
			DrawRectangle( new NacGeo::Rectangle( left, top, right - left, bottom - top ), color, fill );
		}
		public void DrawRectangle( Vector2D leftTop, Vector2D size, Color color, bool fill )
		{
			DrawRectangle( new NacGeo::Rectangle( leftTop, size ), color, fill );
		}
		public void DrawRectangle( NacGeo::Rectangle rectangle, Color color, bool fill )
		{
			if( fill )
			{
				spriteBatch.Draw( pixel, rectangle.ToXna(), color );
			}
			else
			{
				DrawLine( rectangle.LeftTop, new Vector2D( rectangle.Right, rectangle.Top ), color );
				DrawLine( rectangle.RightBottom, new Vector2D( rectangle.Left, rectangle.Bottom ), color );
				DrawLine( new Vector2D( rectangle.Right, rectangle.Top ), rectangle.RightBottom, color );
				DrawLine( new Vector2D( rectangle.Left, rectangle.Bottom ), rectangle.LeftTop, color );
			}
		}

		public void DrawCircle( float x, float y, float radius, Color color, bool fill )
		{
			DrawCircle( new Vector2D( x, y ), radius, color, fill );
		}
		public void DrawCircle( Vector2D center, float radius, Color color, bool fill )
		{
			Func<float, float, Vector2D> f = ( x, y ) => center + new Vector2D( x, y );
			Vector2 draw = new Vector2( radius, 0 );
			Vector2 prev = draw;
			while( draw.X >= draw.Y )
			{
				if( fill )
				{
					Action<float, float> d = ( x, y ) => DrawHorizon( center.Y + y, center.X - x, center.X + x, color );

					d( draw.X, draw.Y );
					
					if( draw.Y != 0 )
						d( draw.X, -draw.Y );
					
					if( draw.X != prev.X )
					{
						d( draw.Y, draw.X );
						d( draw.Y, -draw.X );
					}
				}
				else
				{
					spriteBatch.Draw( pixel, f( draw.X, draw.Y ).ToXna(), color );
					spriteBatch.Draw( pixel, f( draw.X, -draw.Y ).ToXna(), color );
					spriteBatch.Draw( pixel, f( -draw.X, draw.Y ).ToXna(), color );
					spriteBatch.Draw( pixel, f( -draw.X, -draw.Y ).ToXna(), color );
					spriteBatch.Draw( pixel, f( draw.Y, draw.X ).ToXna(), color );
					spriteBatch.Draw( pixel, f( draw.Y, -draw.X ).ToXna(), color );
					spriteBatch.Draw( pixel, f( -draw.Y, draw.X ).ToXna(), color );
					spriteBatch.Draw( pixel, f( -draw.Y, -draw.X ).ToXna(), color );
				}

				prev = draw;
				Vector2 a = new Vector2( draw.X, draw.Y + 1 );
				Vector2 b = new Vector2( draw.X - 1, draw.Y + 1 );
				float ad = Math.Abs( a.LengthSquared() - radius*radius );
				float bd = Math.Abs( b.LengthSquared() - radius*radius );
				draw = ad < bd ? a : b;
			}
		}
		public void DrawCircle( Circle circle, Color color, bool fill )
		{
			DrawCircle( circle.Center, circle.Radius, color, fill );
		}

		public void DrawPolygon( Color color, params Vector2D[] vertexes )
		{
			DrawPolygon( new Polygon( vertexes ), color );
		}
		public void DrawPolygon( Polygon polygon, Color color )
		{
			int length = polygon.Vertexes.Count;
			for( int i = 0; i < length; i++ )
				DrawLine( polygon.Vertexes[i], polygon.Vertexes[( i+1 )%length], color );
		}
	}
}
