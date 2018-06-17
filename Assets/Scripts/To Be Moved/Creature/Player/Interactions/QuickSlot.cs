using System.Collections.Generic;
using UnityEngine;

namespace Eden.Life.Chip {
	
	public class QuickSlot: MonoBehaviour {

		[SerializeField] private Player _player;

		// ***************** PUBLIC *******************

		public delegate void OnInputChangedEvent( QuickSlotInventory.ID newId );
		public OnInputChangedEvent OnInputChanged;

		
		// ***************** PRIVATE *******************

		private const KeyCode _topKey    = KeyCode.I;
		private const KeyCode _rightKey  = KeyCode.L;
		private const KeyCode _bottomKey = KeyCode.K;
		private const KeyCode _leftKey   = KeyCode.J;

		private List<KeyCode> _inputStack;
		private QuickSlotInventory.ID _id;


		// ********************** Private  ************************

		private void Awake () {
			
			_inputStack = new List<KeyCode>();
			_player.OnRecieveInput += RecieveInput;
		}
		private void RecieveInput( Input.Package package ) {

			if ( package.Dpad.Up_Down )    { AddInput( _topKey ); }
			if ( package.Dpad.Right_Down ) { AddInput( _rightKey );}
			if ( package.Dpad.Down_Down )  { AddInput( _bottomKey ); }
			if ( package.Dpad.Left_Down )  { AddInput( _leftKey ); }

			if ( package.Dpad.Up_Up )    { RemoveInput( _topKey ); }
			if ( package.Dpad.Right_Up ) { RemoveInput( _rightKey ); }
			if ( package.Dpad.Down_Up )  { RemoveInput( _bottomKey ); }
			if ( package.Dpad.Left_Up )  { RemoveInput( _leftKey ); }

			var newID = GetID();
			if ( _id != newID ){

				_id = newID;
				FireOnInputChange( _id );
			}
		}
		private void AddInput ( KeyCode key ) {

			if ( !_inputStack.Contains( key ) ){
				_inputStack.Add( key );
			}
		}
		private void RemoveInput ( KeyCode key ) {

			if ( _inputStack.Contains( key ) ){
				_inputStack.Remove( key );
			}
		}
		private void FireOnInputChange ( QuickSlotInventory.ID id  ) {

			if (OnInputChanged != null){
				OnInputChanged( id );  
			}
		}
		private QuickSlotInventory.ID GetID () {

			if ( _inputStack.Count != 0 ){
				
				var input = _inputStack[ _inputStack.Count-1 ];

				if ( input == _topKey ){
					return QuickSlotInventory.ID.Top;
				}
				if ( input == _rightKey ){
					return QuickSlotInventory.ID.Right;
				}
				if ( input == _bottomKey ){
					return QuickSlotInventory.ID.Bottom;
				}
				if ( input == _leftKey ){
					return QuickSlotInventory.ID.Left;
				}
			}

			return QuickSlotInventory.ID.Center;
		}
	}
}