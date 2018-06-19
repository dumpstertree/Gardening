using System.Collections.Generic;
using UnityEngine;

namespace Eden.Life.Chip {
	
	public class QuickSlotChip: MonoBehaviour {

		[SerializeField] private Eden.Life.BlackBoxes.Player _player;

		// ***************** PUBLIC *******************

		public delegate void OnInputChangedEvent( int index );
		public OnInputChangedEvent OnInputChanged;

		
		// ***************** PRIVATE *******************

		private const int CENTER_INDEX = 0;
		private const int TOP_INDEX    = 1;
		private const int RIGHT_INDEX  = 2;
		private const int BOTTOM_INDEX = 3;
		private const int LEFT_INDEX   = 4;

		private List<int> _inputStack;
		private int _index;


		// ********************** Private  ************************

		private void Awake () {
			
			_inputStack = new List<int>();
			_player.OnRecieveInput += RecieveInput;
		}
		private void RecieveInput( Input.Package package ) {

			if ( package.Dpad.Up_Down )    { AddInput( TOP_INDEX ); }
			if ( package.Dpad.Right_Down ) { AddInput( RIGHT_INDEX );}
			if ( package.Dpad.Down_Down )  { AddInput( BOTTOM_INDEX ); }
			if ( package.Dpad.Left_Down )  { AddInput( LEFT_INDEX ); }

			if ( package.Dpad.Up_Up )    { RemoveInput( TOP_INDEX ); }
			if ( package.Dpad.Right_Up ) { RemoveInput( RIGHT_INDEX ); }
			if ( package.Dpad.Down_Up )  { RemoveInput( BOTTOM_INDEX ); }
			if ( package.Dpad.Left_Up )  { RemoveInput( LEFT_INDEX ); }

			var newIndex = GetID();
			if ( _index != newIndex ){

				_index = newIndex;
				FireOnInputChange( newIndex );
			}
		}
		private void AddInput ( int index ) {

			if ( !_inputStack.Contains( index ) ){
				_inputStack.Add( index );
			}
		}
		private void RemoveInput ( int index ) {

			if ( _inputStack.Contains( index ) ){
				_inputStack.Remove( index );
			}
		}
		private void FireOnInputChange ( int index  ) {

			if (OnInputChanged != null){
				OnInputChanged( index );  
			}
		}
		private int GetID () {

			return ( _inputStack.Count != 0 ) ? _inputStack[ _inputStack.Count-1 ] : CENTER_INDEX;
		}
	}
}