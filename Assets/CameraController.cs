using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class CameraController : MonoBehaviour, IInputReciever<Eden.Input.Package> {
	
	
	// **************** Public ******************
	
	public void RecieveInput ( Eden.Input.Package package ) {

		_horizontal = package.RightAnalog.Horizontal;
		_vertical = package.RightAnalog.Vertical;
	}
	public void EnteredInputFocus () {
	}
	public void ExitInputFocus () {
	}

	
	// **************** Private ******************
	
	[SerializeField] private Transform _target;
	[SerializeField] private float _minDistanceToTarget;
	[SerializeField] private float _maxDistanceToTarget;
	[SerializeField] private float _lerpPos;

	[SerializeField] private float _cameraRotateSpeed;
	[SerializeField] private float _maxVerticalRotation;
	[SerializeField] private float _minVerticalRotation;

	private bool _moving;
	private float _horizontal;
	private float _vertical;

	
	// mono
	private void Start () {

		EdensGarden.Instance.Input.RegisterToInputLayer( "Testing", this );
		EdensGarden.Instance.Input.RequestInput( "Testing" );
	}
	private void LateUpdate () {

		var distanceToTarget = Vector3.Distance( transform.position, _target.position );

		if ( !(distanceToTarget > _minDistanceToTarget && distanceToTarget < _maxDistanceToTarget) || _moving ) {
			Move ();
		}

		RotateHorizontal( _horizontal );
		RotateVertical( _vertical );

		LookAt ();
	}
	private void OnDrawGizmos () {

		var toTarget = ( _target.position - transform.position ).normalized;
		var right = Vector3.Cross( toTarget, Vector3.up ).normalized; 
		var forward = Vector3.Cross( right, Vector3.up ).normalized;
		
		Gizmos.color = Color.white;
		Gizmos.DrawRay( transform.position, toTarget );
		
		Gizmos.color = Color.red;
		Gizmos.DrawRay( transform.position, Vector3.up );
		
		Gizmos.color = Color.blue;
		Gizmos.DrawRay( transform.position, forward );
		
		Gizmos.color = Color.green;
		Gizmos.DrawRay( transform.position, right );
	}
		

	private void Move () {

		// start moving
		_moving = true;
			
		
		// find direction to target
		var toTarget =  ( _target.position - transform.position ).normalized;

		
		// find local directions
		var right   = Vector3.Cross( toTarget, Vector3.up ).normalized;
		var forward = Vector3.Cross( right,    Vector3.up ).normalized;
			
		
		// find new camera target position
		var targetPos = _target.position + (forward * _lerpPos);

		
		// move camera
		transform.position = Vector3.Lerp( transform.position, targetPos, 0.2f );


		// if close enough to the target's position stop moving
		if ( Vector3.Distance( transform.position, _target.position ) - _lerpPos < 0.1f ) {
			_moving = false;
		}
	}
	private void RotateHorizontal ( float horizontalInput ) {

		// rotate around the target
		transform.RotateAround( _target.position, Vector3.up, horizontalInput * _cameraRotateSpeed );
	}
	private void RotateVertical ( float verticalInput ) {
	
		// get angle to target
		var toTarget = (_target.position - transform.position).normalized;
		var angle = Vector3.Angle( Vector3.up, toTarget );
		
		
		// project what the next angle will be
		var projectedAngle = angle + (verticalInput * _cameraRotateSpeed);
			

		// as long as the projected angle fits in the rules do it
		if ( projectedAngle < _maxVerticalRotation && projectedAngle > _minVerticalRotation ) {

			transform.RotateAround( _target.position, transform.right, verticalInput * _cameraRotateSpeed );
		} 
	}
	private void LookAt () {

		// find direction to target
		var toTarget = ( _target.position - transform.position ).normalized;


		// lerp to face target
		transform.rotation = Quaternion.Slerp( 
			transform.rotation, 
			Quaternion.LookRotation( toTarget ), 
			0.5f
		);
	}
}
