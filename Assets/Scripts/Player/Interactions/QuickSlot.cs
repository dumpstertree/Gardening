using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlot: MonoBehaviour {

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

	// *********************************************

	private void Awake () {
		
		_inputStack = new List<KeyCode>();
	}
	private void Update () {

		if ( Input.GetKeyDown( _topKey ) )    { AddInput( _topKey ); }
		if ( Input.GetKeyDown( _rightKey ) )  { AddInput( _rightKey );}
		if ( Input.GetKeyDown( _bottomKey ) ) { AddInput( _bottomKey ); }
		if ( Input.GetKeyDown( _leftKey ) )   { AddInput( _leftKey ); }

		if ( Input.GetKeyUp( _topKey ) )    { RemoveInput( _topKey ); }
		if ( Input.GetKeyUp( _rightKey ) )  { RemoveInput( _rightKey ); }
		if ( Input.GetKeyUp( _bottomKey ) ) { RemoveInput( _bottomKey ); }
		if ( Input.GetKeyUp( _leftKey ) )   { RemoveInput( _leftKey ); }

		var newID = GetID();
		if ( _id != newID ){

			_id = newID;
			FireOnInputChange( _id );
		}
	}
	private void AddInput ( KeyCode key ){

		if ( !_inputStack.Contains( key ) ){
			_inputStack.Add( key );
		}
	}
	private void RemoveInput ( KeyCode key ){

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

