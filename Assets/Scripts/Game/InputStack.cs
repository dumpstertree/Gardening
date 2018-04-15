using System.Collections.Generic;
using UnityEngine;

public class InputStack : MonoBehaviour {

	//  ************* Public ******************
	
	public void Init () {
		
		_recievers = new List<InputRecieverLayer>();
		_inputLayout = new DefaultLayout();
	}
	public void AddReciever ( InputRecieverLayer receiver ) {

		if ( !_recievers.Contains( receiver ) ){
			_recievers.Add( receiver );
		}
	}
	public void RemoveReciever ( InputRecieverLayer receiver ) {

		if ( _recievers.Contains( receiver ) ){
			_recievers.Remove( receiver );
		}
	}
	
	//  ************* Private ******************

	private Layout _inputLayout;
	private List<InputRecieverLayer> _recievers;
	
	private bool _confirm;
	private bool _cancel;
	private bool _start;
	private bool _select;
	private float _horizontal;
	private float _vertical;

	private void Update () {


		if ( _recievers.Count == 0 ){
			Debug.LogWarning( "No Input Receivers!" );
			return;
		}

		var reciever = _recievers[ _recievers.Count-1 ];
		if ( reciever != null ) {
		
			// Confirm
			if ( _inputLayout.Confirm != _confirm ){
				if ( _inputLayout.Confirm ) {
					reciever.OnConfirmDown ();
				} else {
					reciever.OnConfirmUp ();
				}
			}

			// Cancel
			if ( _inputLayout.Cancel != _cancel ){
				if ( _inputLayout.Confirm ) {
					reciever.OnCancelDown ();
				} else {
					reciever.OnCancelUp ();
				}
			}

			// Horizontal
			if ( _inputLayout.Horizontal != _horizontal ){
				reciever.HorizontalChanged( _inputLayout.Horizontal );
			}

			// Vertical
			if ( _inputLayout.Vertical != _vertical ){
				reciever.VerticalChanged( _inputLayout.Vertical );
			}

			// save values for next frame
			_confirm = _inputLayout.Confirm;
			_cancel = _inputLayout.Cancel;
			_start = _inputLayout.Start;
			_select = _inputLayout.Select;
			_horizontal = _inputLayout.Horizontal;
			_vertical = _inputLayout.Vertical;
		}
	}

	//  ************* DataTypes ******************
	
	public interface Layout {

		bool Confirm { get; }
		bool Cancel { get; }
		bool Start { get; }
		bool Select { get; }
		float Horizontal { get; }
		float Vertical { get; }
	}

	//  ************* Layouts ******************

	private class DefaultLayout : Layout {
		
		public bool Confirm { 
			get{ return Input.GetKey( KeyCode.RightShift ); }
		}
		
		public bool Cancel { 
			get{ return Input.GetKey( KeyCode.LeftShift ); }
		}
		
		public bool Start { 
			get{ return Input.GetKey( KeyCode.A ); }
		}
		
		public bool Select { 
			get{ return Input.GetKey( KeyCode.A ); }
		}

		public float Horizontal { 
			get{ return Input.GetAxis( "Horizontal" ); }
		}
		
		public float Vertical { 
			get{ return Input.GetAxis( "Vertical" ); }
		}
	}
}

public class InputRecieverLayer {

	public void OnConfirmDown () {
		foreach( IInputReciever r in _receivers ){ r.OnConfirmDown (); }
	}
	public void OnConfirmUp () {
		foreach( IInputReciever r in _receivers ){ r.OnConfirmUp (); }
	}
	public void OnCancelDown () {
		foreach( IInputReciever r in _receivers ){ r.OnCancelDown (); }
	}
	public void OnCancelUp () {
		foreach( IInputReciever r in _receivers ){ r.OnCancelUp (); }
	}
	public void HorizontalChanged ( float horizontal ){
		foreach( IInputReciever r in _receivers ){ r.HorizontalChanged( horizontal ); }
	}
	public void VerticalChanged ( float vertical ){
		foreach( IInputReciever r in _receivers ){ r.VerticalChanged( vertical ); }
	}

	private List<IInputReciever> _receivers;

	public InputRecieverLayer( List<IInputReciever> recievers ) {

		_receivers = recievers;
	}
}

public interface IInputReciever {

	void OnConfirmDown ();
	void OnConfirmUp ();
	void OnCancelDown ();
	void OnCancelUp ();
	void HorizontalChanged ( float horizontal );
	void VerticalChanged ( float vertical );
}


