using UnityEngine;

public class PlayerPassiveSubBrain : MonoBehaviour {

	public void Think ( float horizontal, float vertical, float cameraHorizontal, float cameraVertical ){

		_horizontal = horizontal;
		_vertical   = vertical;
		_cameraHorizontal = cameraHorizontal;
		_cameraVertical = cameraVertical;

		_followCamera.CameraHorizontal = _cameraHorizontal;
		_followCamera.CameraVertical = _cameraVertical;

		// if ( _player.Animator.GetCurrentAnimatorStateInfo(0).IsTag( RESTRICTED_INPUT_TAG ) ) {
		// 	return;
		// }

		// Animate();

		// if (_horizontal != 0 || _vertical != 0){
		// 	Rotate();
		// 	Move();
		// }
	}


	//***************************

	[SerializeField] private LayerMask _layerMask;
	[SerializeField] private Dumpster.Core.BuiltInModules.FollowCameraController _followCamera;
	[SerializeField] private Eden.Life.BlackBoxes.Player _player;
	[SerializeField] private float _speed;

	private const string RESTRICTED_INPUT_TAG = "InputRestricted";
	private const string HORIZONTAL_ANIMATION_NAME = "Horizontal";
	private const string VERTICAL_ANIMATION_NAME = "Vertical";

	private float _horizontal;
	private float _vertical;
	private float _cameraHorizontal;
	private float _cameraVertical;

	private float _walkTreshold = 0.05f;
	private float _runThreshold = 0.6f;

	private float _walkValue = 0.5f;
	private float _runValue  = 1.0f;

	private float _forwardAnimationValue = 0f;
	private float _lerpSpeed = 0.5f;

	//***************************

	private void Rotate () {

		var rads = Mathf.Atan2( _vertical,_horizontal );
		var degrees = rads * Mathf.Rad2Deg;
		var adjusted = ( degrees - 90 ) * -1;
		var y = Camera.main.transform.eulerAngles.y + adjusted;

		_player.transform.rotation = Quaternion.Euler( new Vector3( 0, y, 0 ) );
	}
	private void Move () {
	
		var h = Mathf.Abs(_horizontal);
		var v = Mathf.Abs(_vertical);
		var speed = ( ( h > v ) ? h : v) * Time.deltaTime * _speed;
		
		_player.Physics.MovePosition ( transform.forward * speed ) ;
	}
	private void Animate () {
		
		var absH = Mathf.Abs( _horizontal );
		var absV = Mathf.Abs( _vertical );
		var greaterValue = absH > absV ? _horizontal : _vertical;

		if ( Mathf.Abs( greaterValue ) > _runThreshold ) {
			_forwardAnimationValue = Mathf.Lerp( _forwardAnimationValue, _runValue, _lerpSpeed);
		} else if ( Mathf.Abs( greaterValue ) > _walkTreshold ) {
			_forwardAnimationValue = Mathf.Lerp( _forwardAnimationValue, _walkValue, _lerpSpeed );
		} else {
			_forwardAnimationValue = Mathf.Lerp( _forwardAnimationValue, 0, _lerpSpeed );
		}


		_player.Animator.SetFloat( VERTICAL_ANIMATION_NAME, _forwardAnimationValue );
	}
}
