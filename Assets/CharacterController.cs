using UnityEngine;

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

	private void Awake () {
		
		_player.OnRecieveInput += RecieveInput;
	}
	private void RecieveInput ( Eden.Input.Package package ) {

		_horizontal = package.LeftAnalog.Horizontal;
		_vertical = package.LeftAnalog.Vertical;
	}

	private float _horizontal;
	private float _vertical;

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
	private void LateUpdate () {

		_animator.ChangeProgress( "Run", Mathf.Repeat( _stride, 1.0f ));
		_animator.SetLayerProgress( "Run", _strideWeight );

		// if( Mathf.Abs(_localVelocity.z) > 0.0f ) {
		// 	_animator.SetAsActiveAnimation( "Run" );
		// } else if ( Input.GetKey( KeyCode.Space ) ) {
		// 	_animator.SetAsActiveAnimation( "Crouch" );
		// } else {
		// 	_animator.SetAsActiveAnimation( "Idle" );
		// }

		if ( Mathf.Abs(_localVelocity.z) > 0.0f ) {
			_animator.SetWeight( "Run", 1f );
		} else {
			_animator.SetWeight( "Run", 0f );
		}
		
		if ( Input.GetKey( KeyCode.Space ) ) {
			_animator.SetWeight( "Crouch", 1f );
		} else {
			_animator.SetWeight( "Crouch", 0f );
		}
	}

	
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

		_root.position += _velocity * Time.deltaTime;
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
