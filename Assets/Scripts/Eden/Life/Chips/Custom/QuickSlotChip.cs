using UnityEngine;

namespace Eden.Life.Chip {
	
	public class QuickSlotChip: MonoBehaviour {

		[SerializeField] private Eden.Life.BlackBoxes.Player _player;

		// ***************** PUBLIC *******************

		public delegate void InputChangedEvent( int index );
		public InputChangedEvent OnInputChanged;

		public delegate void IndexChangedEvent( int index );
		public IndexChangedEvent OnIndexChanged;

		public int Index {
			get{ return _index; }
		}
		public bool ItemIsEquiped{
			get{ return _equipIsDown; }
		}

		
		// ***************** PRIVATE *******************

		private int _index = 1;
		private int _numOfItems;
		private bool _equipIsDown;

		private const int _idleIndex = 0;


		// ********************** Private  ************************
		
		private void Awake () {
			
			_player.OnRecieveInput += RecieveInput;
			_numOfItems = _player.EquipedItems.InventoryCount;
		}
		private void RecieveInput( Input.Package package ) {

			if ( package.Dpad.Left_Down ) {
				
				ShiftLeft();
			}
			if ( package.Dpad.Right_Down ) {
				
				ShiftRight();
			}
			
			if ( package.BackLeft.Bumper_Down ) {
				
				_equipIsDown = true;
				FireOnInputChange( _index );
			}
			
			if ( package.BackLeft.Bumper_Up ) {
				
				_equipIsDown = false;
				FireOnInputChange( _idleIndex );
			}
		} 
		private void ShiftLeft () {

			_index -= 1;
			WrapIndex();

			FireIndexChangedEvent ( _index );

			if( _equipIsDown ) {
				FireOnInputChange( _index );
			}
		}
		private void ShiftRight () {

			_index += 1;
			WrapIndex();

			FireIndexChangedEvent ( _index );

			if( _equipIsDown ) {
				FireOnInputChange( _index );
			}
		}
		private void WrapIndex () {

			_index = Mathf.Clamp( _index, 1, _numOfItems - 1 );

			// eventauly this should wrap probably

			// if( _index <= 0) {
			// 	_index = _numOfItems - 1;
			// }
			// if( _index >= _numOfItems) {
			// 	_index = 1;
			// }
		}
		private void FireOnInputChange ( int index  ) {

			if (OnInputChanged != null){
				OnInputChanged( index );  
			}
		}
		private void FireIndexChangedEvent ( int index  ) {

			if (OnIndexChanged != null){
				OnIndexChanged( index );  
			}
		}
	}
}