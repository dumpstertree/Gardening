using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class CameraController : MonoBehaviour, IInputReciever<Eden.Input.Package> {
	
	
	// **************** Public ******************
	
	public void RecieveInput ( Eden.Input.Package package ) {

		_horizontal = package.RightAnalog.Horizontal;
		_vertical = package.RightAnalog.Vertical;

		if ( package.BackLeft.Bumper_Down ) {
			_strafing= true;
		}
		if ( package.BackLeft.Bumper_Up ) {
			_strafing= false;
		}
	}
	public void EnteredInputFocus () {
	}
	public void ExitInputFocus () {
	}

	
	// **************** Private ******************
	
	private float _distanceToTarget {
		get{ return Vector3.Distance( transform.position, _target.position ); }
	}
	private bool _targetOutOfRange {
		get{ return !(_distanceToTarget > _targetDistance + OUT_OF_RANGE && _distanceToTarget < _targetDistance - OUT_OF_RANGE); }
	}
	private float _targetDistance {
		get{ return _strafing ? _strafeDistance : _followDistance; }
	}
	private float _lerpSpeed {
		get { return _strafing ? _strafeLerpSpeed : _followLerpSpeed; }
	}

	
	[Header( "Target" )]
	[SerializeField] private Transform _target;

	[Header( "Limits" )]
	[SerializeField] private float _minVerticalRotation = -80f;
	[SerializeField] private float _maxVerticalRotation =  80f;

	[SerializeField] private float _followDistance = 6f;
	[SerializeField] private float _strafeDistance = 3f;

	[Header( "Sensitivity" )]
	[SerializeField] private float _horizontalSensitivity = 180f;
	[SerializeField] private float _verticalSensitivity = 100f;

	[Header( "Lerping" )]
	[SerializeField] private float _followLerpSpeed = 0.3f;
	[SerializeField] private float _strafeLerpSpeed = 0.6f;
	
	[Header( "Raycast" )]
	[SerializeField] private float _minDistanceToCollider = 1f;
	
	private const float OUT_OF_RANGE = 0.01f;

	
	private float _horizontal;
	private float _vertical;
	private float _verticalRot;
	private bool _strafing;

	private void Start () {

		EdensGarden.Instance.Input.RegisterToInputLayer( "Testing", this );
		EdensGarden.Instance.Input.RequestInput( "Testing" );
	}
	private void FixedUpdate () {

		RotateHorizontal( _horizontal );
		RotateVertical( _vertical );

		if ( _targetOutOfRange) {

			var right = Vector3.Cross( transform.forward, Vector3.up ).normalized; 
			var forward = Vector3.Cross( right, Vector3.up ).normalized;
			var direction = Quaternion.AngleAxis( _verticalRot, right ) * forward;
			var targetPos =  _target.position + (direction * _targetDistance);

			var collisionTargetPos = AccountForCollision( _minDistanceToCollider, _targetDistance, _target.position, targetPos );
			
			transform.position = Vector3.Lerp( transform.position, collisionTargetPos, _lerpSpeed );
			transform.rotation = Quaternion.Slerp( transform.rotation, Quaternion.LookRotation( -direction ), _lerpSpeed );
		}
	}
	private Vector3 AccountForCollision( float minDistanceToCollider, float distanceFromCamera, Vector3 startPos, Vector3 targetPosition ) {

		var dir = targetPosition - startPos;

		RaycastHit hit;
		if ( Physics.Raycast( startPos, dir, out hit, distanceFromCamera ) ) {

			return hit.point + (-dir * minDistanceToCollider);
		}

		return targetPosition;
	} 

	private void RotateHorizontal ( float horizontalInput ) {

		// rotate around the target
		transform.RotateAround( _target.position, Vector3.up, horizontalInput * (_horizontalSensitivity * Time.fixedDeltaTime) );
	}
	private void RotateVertical ( float verticalInput ) {
		
		// project what the next angle will be
		var projectedRot = _verticalRot + (verticalInput * (_verticalSensitivity * Time.fixedDeltaTime) );
		
		// as long as the projected angle fits in the rules do it
		if ( projectedRot < _maxVerticalRotation && projectedRot > _minVerticalRotation ) {

			// save new rotation
			_verticalRot = projectedRot;
		} 
	}
}
