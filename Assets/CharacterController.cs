using UnityEngine;
using System.Linq;

public class CharacterController : MonoBehaviour {


	[Header( "References" )]
	[SerializeField] private Eden.Life.BlackBoxes.Player _player;
	[SerializeField] Dumpster.Animation.Animator _animator;
	[SerializeField] private Transform _root;

	[Header( "Physics" )]	
	[SerializeField] private float _drag 			 = 20f;
	[SerializeField] private float _terminalVelocity = 20f;
	[SerializeField] private float _balance 		 = 10f;

	[Header( "Strides" )]
	[SerializeField] private float _stridesPerMeterWalk = 0.5f;
	[SerializeField] private float _stridesPerMeterRun  = 0.2f;


	private float _distanceCovered;
	private float _stride;
	private Vector3 _velocity;


	private Vector3 _localVelocity {
		get { return _root.InverseTransformVector( _velocity ); }
	}
	private float _strideWeight {
		get { return _localVelocity.z/_terminalVelocity; }
	}
	private float _stridesPerMeter {
		get { return Mathf.Lerp( _stridesPerMeterWalk, _stridesPerMeterRun, _strideWeight ); }
	}
	private float _distanceFromGround {
		get{ 
			var hit = new RaycastHit();
			if ( Physics.Raycast( _root.transform.position, -_root.transform.up, out hit, Mathf.Infinity) ){
				return hit.distance;
			}

			return Mathf.Infinity;
		}
	}

	private void Awake () {
		
		_player.OnRecieveInput += RecieveInput;
	}
	private void RecieveInput ( Eden.Input.Package package ) {

		_horizontal = package.LeftAnalog.Horizontal;
		_vertical = package.LeftAnalog.Vertical;
	}

	private float _horizontal;
	private float _vertical;

	[SerializeField] private AnimationCurve _curve;
	[SerializeField] private AnimationCurve _fallCurve;
	
	private void Update () {
		
		ApplyVelocity( _horizontal, _vertical );
		ApplyTerminalVelocity ();
	
		Rotate ();
		Move ();
		Balance ();

		ApplyDrag ();


		_distanceCovered += _localVelocity.z * Time.deltaTime;
		_stride += (_localVelocity.z * Time.deltaTime) * _stridesPerMeter;
	}
	private float _worldAnimationProgress {
		get { return Time.time - Mathf.Floor( Time.time ); }
	}
	private void LateUpdate () {

		_animator.ChangeProgress( "Run", Mathf.Repeat( _stride, 1.0f ));
		_animator.SetLayerProgress( "Run", _strideWeight );

		if ( Mathf.Abs(_localVelocity.z) > 0.0f ) {
			_animator.SetAnimationPlaying( "Run", true );
		} else {
			_animator.SetAnimationPlaying( "Run", false );
		}
		

		if ( _isJumping ) {
			_animator.SetWeight( "Jump Up", _curve.Evaluate( 1 - Mathf.Clamp01( GetComponent<Rigidbody>().velocity.y / 10f ) ) );
		} else {
			_animator.SetWeight( "Jump Up", 0f );
		}

		if ( _isFalling ) {
			_animator.SetWeight( "Jump Down", 1f );
		} else {
			_animator.SetWeight( "Jump Down", .01f );
		}

		if ( !_isFalling && _wasFallingLastFrame ) {
			_animator.SetWeight( "Crouch", 0.99f );
		}


		if ( Input.GetKeyDown( KeyCode.J ) ) {

			_animator.SetAnimationPlaying( "Crouch", true );
			EdensGarden.Instance.Async.WaitForSeconds( 0.1f, () => {

				_animator.SetAnimationPlaying( "Crouch", false );
				GetComponent<Rigidbody>().velocity += (Vector3.up * 10);

			});
		}

		_animator.ChangeProgress( "Jump Up", _worldAnimationProgress );
		_animator.ChangeProgress( "Jump Down", _worldAnimationProgress );

		_wasJumpinglastFrame = _isJumping;
		_wasFallingLastFrame = _isFalling;
	}

	private bool _isJumping {
		get { return System.Math.Round( GetComponent<Rigidbody>().velocity.y, 2 ) > 0; }
	}
	private bool _isFalling {
		get { return System.Math.Round( GetComponent<Rigidbody>().velocity.y, 2 ) < -0.1f; }
	}
	private bool _wasJumpinglastFrame;
	private bool _wasFallingLastFrame;
	
	// physics
	private void ApplyVelocity ( float horizontalInput, float verticalInput ) {
		
		// get input
		var largerInput = new Vector2( horizontalInput, verticalInput ).magnitude;

		// get angle of analog
		var rads = Mathf.Atan2( horizontalInput, verticalInput );
		var degrees = rads * Mathf.Rad2Deg;
		var angle = Camera.main.transform.eulerAngles.y + degrees;

		// get forward vector
		var forwardRotation = Quaternion.Euler( new Vector3( 0, angle, 0 ) );
		var forwardVector = forwardRotation * Vector3.forward;

		// move in forward vector
		_velocity = Vector3.Lerp( _velocity, ( forwardVector * ( _terminalVelocity * largerInput ) ), 0.2f );
	}
	private void ApplyDrag () {	

		var x = Mathf.Clamp( Mathf.Abs( _velocity.x ) - _drag * Time.deltaTime, 0f, _terminalVelocity) * ( _velocity.x > 0 ? 1 : -1);
		var y = 0f;
		var z = Mathf.Clamp( Mathf.Abs( _velocity.z ) - _drag * Time.deltaTime, 0f, _terminalVelocity) * ( _velocity.z > 0 ? 1 : -1);

		_velocity = new Vector3( x, y, z );
	}
	private void ApplyTerminalVelocity () {
		
		_velocity = new Vector3( 

			Mathf.Clamp( _velocity.x, -_terminalVelocity, _terminalVelocity ),
			Mathf.Clamp( _velocity.y, -_terminalVelocity, _terminalVelocity ),
			Mathf.Clamp( _velocity.z, -_terminalVelocity, _terminalVelocity )
		);	
	}


	// use physics
	private void Rotate () {
			
		if ( _velocity.magnitude > 0.01f ) {
			_root.localRotation = Quaternion.LookRotation( _velocity );
		}
	}
	private void Move () {

		var v = new Vector3( _velocity.x, GetComponent<Rigidbody>().velocity.y, _velocity.z );
		GetComponent<Rigidbody>().velocity = v;
	}
	private void Balance () {

		_root.localRotation = _root.rotation *
							  Quaternion.AngleAxis(  (_localVelocity.z/_terminalVelocity) * _balance, Vector3.right ) * 
							  Quaternion.AngleAxis( (-_localVelocity.x/_terminalVelocity) * _balance, Vector3.forward );
	}

	// debug
	private void OnDrawGizmos () {

		var leftLegRay1 = new Ray( transform.position, Quaternion.AngleAxis( _stride * 360f/_stridesPerMeter , _root.right ) *  _root.forward );
		var leftLegRay2 = new Ray( transform.position, Quaternion.AngleAxis( _stride * 360f/_stridesPerMeter , _root.right ) * -_root.forward );
		
		var rightLegRay1 = new Ray( transform.position, Quaternion.AngleAxis( _stride * 360f/_stridesPerMeter , _root.right ) *  _root.up );
		var rightLegRay2 = new Ray( transform.position, Quaternion.AngleAxis( _stride * 360f/_stridesPerMeter , _root.right ) * -_root.up );


		Gizmos.color = Color.blue;
		Gizmos.DrawRay( leftLegRay1 );
		Gizmos.DrawRay( leftLegRay2 );

		Gizmos.color = Color.red;
		Gizmos.DrawRay( rightLegRay1 );
		Gizmos.DrawRay( rightLegRay2 );
	}
}
