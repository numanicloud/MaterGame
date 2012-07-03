using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NacXna.Input
{
	public struct ChoiceController
	{
		public IInputHelper InputDevice { get; set; }
		public MeanOfKey NextKey { get; set; }
		public MeanOfKey PreviousKey { get; set; }
		public MeanOfKey DecideKey { get; set; }
		public MeanOfKey? CancelKey { get; set; }

		public ChoiceController( IInputHelper device, MeanOfKey nextKey, MeanOfKey previousKey, MeanOfKey decideKey )
			: this( device, nextKey, previousKey, decideKey, null )
		{
		}

		public ChoiceController( IInputHelper device, MeanOfKey nextKey, MeanOfKey previousKey, MeanOfKey decideKey, MeanOfKey cancelKey )
			: this( device, nextKey, previousKey, decideKey, (MeanOfKey?)cancelKey )
		{
		}

		private ChoiceController( IInputHelper device, MeanOfKey nextKey, MeanOfKey previousKey, MeanOfKey decideKey, MeanOfKey? cancelKey )
			 : this()
		{
			this.InputDevice = device;
			this.NextKey = nextKey;
			this.PreviousKey = previousKey;
			this.DecideKey = decideKey;
			this.CancelKey = cancelKey;
		}
	}

	public class Choice : GameComponent
	{
		#region フィールド
		public IInputHelper input { get; set; }
		public MeanOfKey NextKey { get; set; }
		public MeanOfKey PreviousKey { get; set; }
		public MeanOfKey DecideKey { get; set; }
		public MeanOfKey? CancelKey { get; set; }

		public event Action<int> OnSelect;
		public event Action<int> OnDecide;
		public event Action OnCancel;

		public bool Loop { get; set; }
		public int Length { get; private set; }
		public int Selection { get; private set; }

		public int WaitTime { get; set; }
		public int SpanTime { get; set; }
		#endregion

		public Choice( Game game, int length, bool loop, ChoiceController controller, int waitTime = 0, int spanTime = 0 )
			: base( game )
		{
			this.Length = length;
			this.Loop = loop;
			this.WaitTime = waitTime;
			this.SpanTime = spanTime;
			SetController( controller );
		}

		public void SetController( ChoiceController controller )
		{
			this.input = controller.InputDevice;
			this.NextKey = controller.NextKey;
			this.PreviousKey = controller.PreviousKey;
			this.DecideKey = controller.DecideKey;
			this.CancelKey = controller.CancelKey;
		}
		public override void Update( GameTime gameTime )
		{
			var state = input.GetState();
			int next = state[NextKey];
			int prev = state[PreviousKey];

			if( state[NextKey] == 1 || SpanTime != 0 && ( state[NextKey] - WaitTime ) % SpanTime == 0 )
			{
				if( OnSelect != null ) OnSelect( Selection );

				if( Selection < Length - 1 ) ++Selection;
				else if( Loop ) Selection = 0;
			}
			if( state[PreviousKey] == 1 || SpanTime != 0 && ( state[PreviousKey] - WaitTime ) % SpanTime == 0 )
			{
				if( OnSelect != null ) OnSelect( Selection );

				if( Selection > 0 ) --Selection;
				else if( Loop ) Selection = Length - 1;
			}

			if( state[DecideKey] == 1 && OnDecide != null )
			{
				OnDecide( Selection );
			}
			if( CancelKey.HasValue && state[CancelKey.Value] == 1 && OnCancel != null )
			{
				OnCancel();
			}

			base.Update( gameTime );
		}
	}
}
