using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace NacXna.Input
{
	public class KeyboardHelper : GameComponent, IInputHelper
	{
		PlayerIndex? playerIndex { get; set; }
		Dictionary<MeanOfKey, int> PressCount { get; set; }
		Dictionary<Keys, MeanOfKey> Config { get; set; }

		public KeyboardHelper( Game game )
			: base( game )
		{
			Config = new Dictionary<Keys, MeanOfKey>();
			PressCount = new Dictionary<MeanOfKey, int>();
			this.playerIndex = null;
		}
		public KeyboardHelper( Game game, PlayerIndex index )
			: base( game )
		{
			PressCount = new Dictionary<MeanOfKey, int>();
			Config = new Dictionary<Keys, MeanOfKey>();
			this.playerIndex = index;
		}

		public override void Update( GameTime gameTime )
		{
			var state = playerIndex.HasValue ? Keyboard.GetState( playerIndex.Value ) : Keyboard.GetState();
			foreach( var button in Config.Keys )
			{
				if( state[button] == KeyState.Down )
					PressCount[Config[button]]++;
				else
					PressCount[Config[button]] = 0;
			}

			base.Update( gameTime );
		}
		public void ConfigWaysDefault()
		{
			Configurate( Keys.Up, MeanOfKey.Up );
			Configurate( Keys.Down, MeanOfKey.Down );
			Configurate( Keys.Left, MeanOfKey.Left);
			Configurate( Keys.Right, MeanOfKey.Right );
		}

		public void Configurate( Keys key, MeanOfKey mean )
		{
			Config[key] = mean;
			PressCount[mean] = 0;
		}
		public MeanOfKey GetConfig( Keys key )
		{
			try
			{
				return Config[key];
			}
			catch( KeyNotFoundException )
			{
				throw new InvalidOperationException( key + "は設定されていません。" );
			}
		}
		public KeyCount GetState()
		{
			return new KeyCount( PressCount );
		}
	}
}
