using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nac.Geometory;

namespace NacXna.Component
{
	/// <summary>
	/// 内部の処理によって、動く物体の現在の位置を操作するクラスのインターフェース。
	/// </summary>
	public interface IMotion
	{
		float ExclusionLevel { get; }
		bool Enabled { get; set; }
		void Move( ref Vector2D position );
		Vector2D Move( Vector2D position );
	}
}
