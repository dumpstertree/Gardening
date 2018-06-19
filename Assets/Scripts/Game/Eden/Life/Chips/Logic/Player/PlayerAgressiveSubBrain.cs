using UnityEngine;

public class PlayerAgressiveSubBrain : MonoBehaviour {

	public void Think ( float horizontal, float vertical ){
		
		_horizontal = horizontal;
		_vertical = vertical;

		if ( Input.GetMouseButtonDown( 0 ) ) {
			_startRot = transform.rotation;
			_mouseDown = Input.mousePosition; 
		}

		if ( Input.GetMouseButton( 0 ) ) {
			Rotate();
		}

		if (_horizontal != 0 || _vertical != 0){
			Move();
		}

		Animate();
	}

	[SerializeField] private float _maxRotation;
	[SerializeField] private Eden.Life.BlackBoxes.Player _player;
	[SerializeField] private float _speed;

	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private float xRot;
	private float _horizontal;
	private float _vertical;
	private Vector3 _mouseDown;
	private Quaternion _startRot;

	private Vector3 _mouseDelta {
		get{ return _mouseDown - Input.mousePosition; }
	}


	private void Rotate () {

		var y = (_mouseDelta.x/Screen.width) * _maxRotation * -1;
		_player.transform.rotation = _startRot * Quaternion.AngleAxis( y, Vector3.up );

		var x = (_mouseDelta.y/Screen.height) * _maxRotation;
		xRot = x;
	}
	private void Move () {

		var h = _horizontal * _speed * Time.deltaTime;
		var v = _vertical * _speed * Time.deltaTime;

		_player.Physics.MovePosition( transform.right * h );
		_player.Physics.MovePosition( transform.forward * v );
	}
	private void Animate () {

		_player.Animator.SetFloat( HORIZONTAL_ANIMATION_NAME, _horizontal );
		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, _vertical );
	}
}
