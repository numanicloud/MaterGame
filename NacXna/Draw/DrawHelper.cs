using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Nac.Geometory;
using NacXna.Helpers;

namespace NacXna.Draw
{
	using NacRectangle = Nac.Geometory.Rectangle;
	using XnaRectangle = Microsoft.Xna.Framework.Rectangle;

	public static class DrawHelper
	{
		public static void Draw( this SpriteBatch spriteBatch, Texture2D texture, Vector2D position, NacRectangle? source = null, Color? color = null,
			float rotation = 0, Vector2D? origin = null, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0 )
		{
			Color c = color ?? Color.White;
			XnaRectangle? s = source != null ? source.Value.ToXna() : (XnaRectangle?)null;
			Vector2 o = ( origin ?? Vector2D.Zero ).ToXna();
			spriteBatch.Draw( texture, position.ToXna(), s, c, rotation, o, 1, effect, layerDepth );
		}
		public static void Draw( this SpriteBatch spriteBatch, Texture2D texture, NacRectangle destination, NacRectangle? source = null, Color? color = null,
			float rotation = 0, Vector2D? origin = null, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0 )
		{
			Color c = color ?? Color.White;
			XnaRectangle? s = source != null ? source.Value.ToXna() : (XnaRectangle?)null;
			Vector2 o = ( origin ?? texture.Bounds.Center.ToVector2D() ).ToXna();
			spriteBatch.Draw( texture, destination.ToXna(), s, c, rotation, o, effect, layerDepth );
		}

		public static void DrawExtend( this SpriteBatch spriteBatch, Texture2D texture, Vector2D position, float scale, NacRectangle? source = null,
			Color? color = null, float rotation = 0, Vector2D? origin = null, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0 )
		{
			Color c = color ?? Color.White;
			XnaRectangle? s = source != null ? source.Value.ToXna() : (XnaRectangle?)null;
			Vector2 o = ( origin ?? Vector2D.Zero ).ToXna();
			spriteBatch.Draw( texture, position.ToXna(), s, c, rotation, o, scale, effect, layerDepth );
		}
		public static void DrawExtend( this SpriteBatch spriteBatch, Texture2D texture, Vector2D position, Vector2D scale, NacRectangle? source = null,
			Color? color = null, float rotation = 0, Vector2D? origin = null, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0 )
		{
			Color c = color ?? Color.White;
			XnaRectangle? s = source != null ? source.Value.ToXna() : (XnaRectangle?)null;
			Vector2 o = ( origin ?? Vector2D.Zero ).ToXna();
			spriteBatch.Draw( texture, position.ToXna(), s, c, rotation, o, scale.ToXna(), effect, layerDepth );
		}

		public static void DrawCenter( this SpriteBatch spriteBatch, Texture2D texture, Vector2D position, NacRectangle? source = null, Color? color = null,
			float rotation = 0, float scale = 1, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0 )
		{
			XnaRectangle? so = source != null ? source.Value.ToXna() : (XnaRectangle?)null;
			Color c = color ?? Color.White;
			Vector2 o = source == null ? texture.Bounds.Center.ToVector2() : ( source.Value.Size / 2 ).ToXna();
			spriteBatch.Draw( texture, position.ToXna(), so, c, rotation, o, scale, effect, layerDepth );
		}
		public static void DrawCenter( this SpriteBatch spriteBatch, Texture2D texture, Circle destination, NacRectangle? source = null, Color? color = null,
			float rotation = 0, SpriteEffects effect = SpriteEffects.None, float layerDepth = 0 )
		{
			XnaRectangle so = source != null ? source.Value.ToXna() : texture.Bounds;
			Color c = color ?? Color.White;
			Vector2 o = source == null ? texture.Bounds.Center.ToVector2() : ( source.Value.Size / 2 ).ToXna();
			var sc = 2 * destination.Radius / Math.Min( so.Width, so.Height );
			spriteBatch.Draw( texture, destination.Center.ToXna(), so, c, rotation, o, sc, effect, layerDepth );
		}

		public static void DrawStringCenter( this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2D location, Color color )
		{
			var size = font.MeasureString( text );
			spriteBatch.DrawString( font, text, location.ToXna() - size/2, color );
		}
	}
}
