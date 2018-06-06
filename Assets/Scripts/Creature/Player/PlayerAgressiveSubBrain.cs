using UnityEngine;

public class PlayerAgressiveSubBrain : MonoBehaviour {


	public void Think ( float horizontal, float vertical ){
		
		_horizontal = horizontal;
		_vertical = vertical;

		if ( Input.GetMouseButtonDown( 0 ) ) {
			_startRot = transform.rotation;
			_mouseDown = Input.mousePosition; 
		}

		MoveCameraTarget();
		MoveCameraFocus();

		if ( Input.GetMouseButton( 0 ) ) {
			Rotate();
		}

		if (_horizontal != 0 || _vertical != 0){
			Move();
		}

		Animate();
	}

	[SerializeField] private float _maxRotation;
	[SerializeField] private Player _player;
	[SerializeField] private float _speed;

	[SerializeField] private Vector3 _targetPosLeft;
	[SerializeField] private Vector3 _targetPosRight;

	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private float _horizontal;
	private float _vertical;
	private Vector3 _mouseDown;
	private Quaternion _startRot;
	private float xRot;

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

		var newPos = transform.position;
		var h = _horizontal * _speed * Time.deltaTime;
		var v = _vertical * _speed * Time.deltaTime;

		newPos = newPos + transform.right * h;
		newPos = newPos + transform.forward * v;

		//_player.Rigidbody.MovePosition( newPos );
	}
	private void Animate () {

		_player.Animator.SetFloat( HORIZONTAL_ANIMATION_NAME, _horizontal );
		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, _vertical );
	}
	private void MoveCameraTarget () {
		
		var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (_horizontal+1) /2 );
		var worldPos = transform.TransformPoint( targetPosLocal );

		_player.CameraTarget.position = Vector3.Lerp( _player.CameraTarget.position, worldPos, 0.2f );
		_player.CameraTarget.rotation = Quaternion.Slerp( _player.CameraTarget.rotation, transform.rotation, 0.2f ); 
	}
	private void MoveCameraFocus () {

		var targetPos = transform.position + Quaternion.AngleAxis( xRot, transform.right ) * transform.forward * 100 ;
		_player.CameraFocus.position = Vector3.Lerp( _player.CameraFocus.position, targetPos, 0.2f);
	}
	private void OnDrawGizmos(){
			
		var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (_horizontal+1) /2 );
		var worldPos = transform.TransformPoint( targetPosLocal );

		Gizmos.DrawWireSphere( worldPos, 0.25f );
	}
}
