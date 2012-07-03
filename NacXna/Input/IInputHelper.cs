using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nac.Geometory;

namespace NacXna.Input
{
	public interface IInputHelper
	{
		KeyCount GetState();
	}

	public enum MeanOfKey
	{
		Up, Down, Left, Right,
		Mean1, Mean2, Mean3, Mean4,
		Mean5, Mean6, Mean7, Mean8,
		Mean9, Mean10, Mean11, Mean12,
	}

	public struct KeyCount
	{
		Dictionary<MeanOfKey, int> pressCount { get; set; }
		public int this[MeanOfKey mean]
		{
			get { return pressCount[mean]; }
		}
		public Vector2D Way
		{
			get
			{
				if( !pressCount.ContainsKey( MeanOfKey.Up ) ||
						!pressCount.ContainsKey( MeanOfKey.Down ) ||
						!pressCount.ContainsKey( MeanOfKey.Left ) || 
						!pressCount.ContainsKey( MeanOfKey.Right ) )
					throw new InvalidOperationException( "方向キーの状態を取得しようとしましたが、方向キーがコンフィグされていません。" );

				Vector2D result = Vector2D.Zero;
				if( pressCount[MeanOfKey.Up] > 0 )
					result.Y--;
				if( pressCount[MeanOfKey.Down] > 0 )
					result.Y++;
				if( pressCount[MeanOfKey.Left] > 0 )
					result.X--;
				if( pressCount[MeanOfKey.Right] > 0 )
					result.X++;
				return result;
			}
		}

		public KeyCount( Dictionary<MeanOfKey, int> pressCount )
			: this()
		{
			this.pressCount = pressCount;
		}
	}

}
