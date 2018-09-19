using System.Collections;
using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class MockCharacterController : MonoBehaviour, IInputReciever<Eden.Input.Package> {

	// ************** Public ****************

	public void RecieveInput ( Eden.Input.Package package ) {

		if ( package.Face.Down_Down && _isOnGround ) {
			_ignoreGravity = true;
			Jump ();
		}

		_horizontal = package.LeftAnalog.Horizontal;
		_vertical = package.LeftAnalog.Vertical;

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


	// ************** Private ****************

	[Header( "References" )]
	[SerializeField] private Animator2 _animator;

	[Header( "Physics" )]	
	[SerializeField] private float _terminalVelocity = 20f;
	[SerializeField] private float _jumpPower = 15f;
	[SerializeField] private float _gravity = 25f;

	[Header( "Movement Settings" )]
	[SerializeField] private float _stridesPerMeterWalk = 0.5f;
	[SerializeField] private float _stridesPerMeterRun  = 0.2f;
	[SerializeField] private float _walkThreshold = 0.15f;
	[SerializeField] private float _runThreshold = 0.95f;
	[SerializeField] private float _maxTurnSpeed = 30f;

	[Header( "Gameplay Settings" )]
	[SerializeField] private bool _strafing = false;

	[SerializeField] private Dumpster.Physics.PhysicsPlane _down;
	[SerializeField] private LayerMask _mask;
	
	private const string WALK_TAG = "Walk";
	private const string RUN_TAG  = "Run";
	private const string MOVE_TAG = "Move";
	private const string IDLE_TAG = "Idle";
	private const string JUMP_TAG = "Jump";
	private const string FALL_TAG = "Fall";
	private const string LAND_TAG = "Land";
		

	private Vector3 _distanceCovered;
	private Vector3 _velocity;
	private bool _ignoreGravity;
	private float _stride;

	private float _horizontal;
	private float _vertical;


	private Vector3 _localVelocity {
		get { return transform.InverseTransformVector( _velocity ); }
	}
	private float _stridesPerMeter {
		get { return Mathf.Lerp( _stridesPerMeterWalk, _stridesPerMeterRun, Vector3.Distance(Vector3.zero, _velocity)/_terminalVelocity ); }
	}
	private bool _isJumping {
		get { return !_isOnGround && GetComponent<Rigidbody>().velocity.y > 0; }
	}
	private bool _isFalling {
		get { return !_isOnGround && GetComponent<Rigidbody>().velocity.y <= 0f; }
	}
	private bool _isOnGround {
		get { return Physics.Raycast( transform.position, -Vector3.up, 0.1f, _mask ); }
	}
	private bool _isMoving {
		get { return _isOnGround && _inputMagnitude > _walkThreshold; }
	}
	private bool _isIdling {
		get{ return _isOnGround && _inputMagnitude < _walkThreshold; }
	}
	private bool _isWalking {
		get{ return _isOnGround && _inputMagnitude > _walkThreshold && _inputMagnitude < _runThreshold; }
	}
	private bool _isRunning {
		get{ return _isOnGround && _inputMagnitude > _runThreshold; }
	}

	private bool _leftFoodDown {
		get{ return Mathf.Repeat( _stride, 1.0f ) < 0.5f; }
	}
	private bool _rightFoodDown {
		get{ return !_leftFoodDown; }
	}

	private float _inputMagnitude {
		get{ return new Vector2( _horizontal, _vertical ).magnitude; }
	}


	// Mono
	private void Start () { 
		
		EdensGarden.Instance.Input.RegisterToInputLayer( "Testing", this );
		EdensGarden.Instance.Input.RequestInput( "Testing" );
	}
	private void Update () {

		if ( _strafing ) {
			
			ApplyVelocity (
				Input.GetAxis( "Horizontal" ), 
				Input.GetAxis( "Vertical" ) 
			);

			FaceCameraForward ();
		
		} else {
			
			ApplyVelocity( 
				Input.GetAxis( "Horizontal" ), 
				Input.GetAxis( "Vertical" ) 
			);
			
			FaceMomentum ();
		}

		if ( !_ignoreGravity ) {
			ApplyGravity ();
		}

		_distanceCovered += _localVelocity * Time.deltaTime;
		_stride += new Vector2( _localVelocity.x, _localVelocity.z ).magnitude * Time.deltaTime * _stridesPerMeter;

		GetComponent<Rigidbody>().velocity = _velocity;

		_ignoreGravity = false;

		Animate ();
	}
	private void Animate () {

		
		// On Ground
		if ( !_wasOnGround && _isOnGround ) {
			OnBeginOnGround ();
		} else if ( _wasOnGround && !_isOnGround ) {
			OnEndOnGround ();
		} else if ( _isOnGround) { 
			OnGround (); 
		}

		_wasOnGround =  _isOnGround;

		
		// Jumping
		if ( !_wasJumping && _isJumping ) {
			OnBeginJump ();
		} else if ( _wasJumping && !_isJumping ) {
			OnEndJump ();
		} else if ( _isJumping ) { 
			OnJump (); 
		}

		_wasJumping =  _isJumping;


		// Falling
		if ( !_wasFalling && _isFalling ) {
			OnBeginFall ();
		} else if ( _wasFalling && !_isFalling ) {
			OnEndFall ();
		} else if ( _isFalling ) { 
			OnFall (); 
		}

		_wasFalling =  _isFalling;

	
		// Moving
		if ( !_wasMoving && _isMoving ) {
			OnBeginMoving ();
		} else if ( _wasMoving && !_isMoving ) {
			OnEndMoving ();
		} else if ( _isMoving ) { 
			OnMoving (); 
		}

		_wasMoving =  _isMoving;


		// Idling
		if ( !_wasIdling && _isIdling ) {
			OnBeginIdle ();
		} else if ( _wasIdling && !_isIdling ) {
			OnEndIdle ();
		} else if ( _isIdling ) { 
			OnIdle (); 
		}

		_wasIdling =  _isIdling;

		
		// Walk
		if ( !_wasWalking && _isWalking ) {
			OnBeginWalk ();
		} else if ( _wasWalking && !_isWalking ) {
			OnEndWalk ();
		} else if ( _isWalking ) { 
			OnWalk (); 
		}

		_wasWalking =  _isWalking;


		// Run
		if ( !_wasRunning && _isRunning ) {
			OnBeginRun ();
		} else if ( _wasRunning && !_isRunning ) {
			OnEndRun ();
		} else if ( _isRunning ) { 
			OnRun (); 
		}

		_wasRunning =  _isRunning;
	}
	private void OnDrawGizmos () {
		
		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var deg = Mathf.Rad2Deg * Mathf.Atan2(  Input.GetAxis( "Horizontal"), Input.GetAxis( "Vertical" ) );
		var inputVector = Quaternion.AngleAxis( deg, Vector3.up ) * cameraForward;
		
		Debug.DrawRay( transform.position, inputVector);
			
		var minPoint = new Vector3( transform.position.x, transform.position.y + 2, transform.position.z );
		var maxPoint = transform.position + new Vector3( inputVector.x, transform.position.y + 2, inputVector.z );
		var targetPoint = Vector3.Lerp( minPoint, maxPoint, _velocity.magnitude/_terminalVelocity );
		
		Gizmos.DrawWireSphere( targetPoint, 0.2f );
	}


	// Physics
	private void ApplyVelocity ( float horizontalInput, float verticalInput ) {

		var input = Mathf.Clamp01( new Vector2( horizontalInput, verticalInput ).magnitude );

		if ( input < _walkThreshold ) {
			_velocity = new Vector3( 0, _velocity.y, 0 );
			return;
		}

		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var deg = Mathf.Rad2Deg * Mathf.Atan2(  horizontalInput, verticalInput );
		var clampedDeg = Mathf.Clamp(  deg, -_maxTurnSpeed, _maxTurnSpeed );
		var inputVector = Quaternion.AngleAxis( clampedDeg, Vector3.up ) * cameraForward;
		var newVelocity = inputVector * ( _terminalVelocity * input );
		var newXYVelocity = new Vector3( newVelocity.x, _velocity.y, newVelocity.z  );
		var lerpedVelocity = Vector3.Lerp ( new Vector3( newXYVelocity.x, _velocity.y, newXYVelocity.z ), newXYVelocity,  0.2f );

		_velocity = lerpedVelocity;
	}
	private void ApplyVelocityStrafing ( float horizontalInput, float verticalInput ) {

		var input = Mathf.Clamp01( new Vector2( horizontalInput, verticalInput ).magnitude );

		if ( input < _walkThreshold ) {
			_velocity = new Vector3( 0, _velocity.y, 0 );
			return;
		}

		var xVelocity = horizontalInput * _terminalVelocity;
		var zVelocity = verticalInput * _terminalVelocity;

		var newXYVelocity = new Vector3( xVelocity, _velocity.y, zVelocity );
		var lerpedVelocity = Vector3.Lerp ( new Vector3( newXYVelocity.x, _velocity.y, newXYVelocity.z ), newXYVelocity,  0.2f );

		_velocity = lerpedVelocity;
	}
	private void Balance ( float horizontalInput, float verticalInput  ) {

		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var deg = Mathf.Rad2Deg * Mathf.Atan2( horizontalInput, verticalInput );			
		var inputVector = Quaternion.AngleAxis( deg, Vector3.up ) * cameraForward;
		
		var minPoint = new Vector3( transform.position.x, transform.position.y + 2, transform.position.z );
		var maxPoint = transform.position + new Vector3( inputVector.x, transform.position.y + 2, inputVector.z );
		
		var targetPoint = Vector3.Lerp( minPoint, maxPoint, _velocity.magnitude/_terminalVelocity );

		transform.rotation = Quaternion.LookRotation( transform.forward, targetPoint - transform.position );
	}
	private void FaceMomentum () {

		var xzVelocity = new Vector3( _velocity.x, 0, _velocity.z );
		
		if ( xzVelocity.magnitude < 0.1f ) {
			return;
		}

		transform.rotation = Quaternion.LookRotation( xzVelocity );
	}
	private void FaceCameraForward () {

		var cameraForward = Camera.main.transform.forward;
		var right = Vector3.Cross( cameraForward, Vector3.up );
		var forward = Vector3.Cross( Vector3.up, right );

		transform.rotation = Quaternion.LookRotation( forward, Vector3.up );
	}
	private void Jump () {

		_velocity += Vector3.up * _jumpPower;
	}
	private void ApplyGravity () {

		if ( !_isOnGround ) {
			_velocity += Vector3.down * Time.deltaTime * _gravity;
		} else {
			_velocity.y = 0;
		}
	}



	// Falling
	private bool _wasFalling;

	private void OnBeginFall () {
		
		_animator.SetProgress( 
			FALL_TAG, 
			0f 
		);

		StartCoroutine( 
			LerpWeight(
				FALL_TAG, 
				0f, 
				1f, 
				0.0f
			) 
		);
	}
	private void OnFall () {

		var prog = Mathf.Clamp01( _velocity.y / -_jumpPower );
		
		_animator.SetProgress( 
			FALL_TAG,
			prog
		);
	}
	private void OnEndFall () {

		_animator.SetWeight( 
			FALL_TAG, 
			0.0f 
		);
	}


	// Jumping
	private bool _wasJumping;

	private void OnBeginJump () {

		_animator.SetProgress( 
			JUMP_TAG, 
			0f 
		);

		StartCoroutine( 
			LerpWeight( 
				JUMP_TAG, 
				0f,
				1f,
				0.0f
			)
		);
	}
	private void OnJump () {

		var prog = Mathf.Clamp01( 1f - _velocity.y / _jumpPower );

		_animator.SetProgress( 
			JUMP_TAG,
			prog
		);
	}
	private void OnEndJump () {

		// the issue here is that when it fades out it overshoots the animation and goes back
		StartCoroutine( 
			LerpWeight(
				JUMP_TAG, 
				1f, 
				0f, 
				0.0f
			) 
		);

		// _animator.SetWeight( 
		// 	JUMP_TAG, 
		// 	0.0f
		// );
	}


	// Grounded
	private bool _wasOnGround;

	private void OnBeginOnGround () {
		
		_animator.SetWeight(
			LAND_TAG, 
			1.0f 
		);
		
		StartCoroutine( 
			LerpProgress( 
				LAND_TAG, 
				0f, 
				1f, 
				0.2f 
			) 
		);

		StartCoroutine( 
			LerpWeight( 
				LAND_TAG, 
				1f, 
				0f, 
				0.2f 
			) 
		);
	}
	private void OnGround () {
	}
	private void OnEndOnGround () {
	}

	
	// Idle
	private bool _wasIdling;

	private void OnBeginIdle () {

		StartCoroutine( 
			LerpWeight( 
				IDLE_TAG, 
				0f, 
				1f, 
				0.2f 
			)
		);
	}
	private void OnIdle () {

		var prog = Mathf.Repeat( Time.time, 1.0f );

		_animator.SetProgress( 
			IDLE_TAG, 
			prog
		);
	}
	private void OnEndIdle () {

		StartCoroutine( 
			LerpWeight( 
				IDLE_TAG, 
				1f, 
				0f, 
				0.2f 
			)
		);
	}


	// Move
	private bool _wasMoving;

	private void OnBeginMoving () {

		// StartCoroutine( 
		// 	LerpWeight( 
		// 		MOVE_TAG, 
		// 		0f, 
		// 		1f, 
		// 		0.2f 
		// 	)
		// );
	}
	private void OnMoving () {

		// _animator.SetProgress( 
		// 	MOVE_TAG,
		// 	Mathf.Repeat( _stride, 1.0f )
		// );

		// _animator.SetGrowthPoint( 
		// 	MOVE_TAG, 
		// 	_localVelocity.x / _terminalVelocity,
		// 	_localVelocity.z / _terminalVelocity
		// );
	}
	private void OnEndMoving () {

		// StartCoroutine( 
		// 	LerpWeight( 
		// 		MOVE_TAG, 
		// 		1f, 
		// 		0f, 
		// 		0.2f 
		// 	)
		// );
	}

	
	// Walking
	private bool _wasWalking;

	private void OnBeginWalk () {

		StartCoroutine( 
			LerpWeight( 
				WALK_TAG,
				0f,
				1f,
				0.2f
			)
		);
	}
	private void OnWalk () {

		var prog = Mathf.Repeat( _stride, 1.0f );

		_animator.SetProgress( 
			WALK_TAG, 
			prog
		);
	}
	private void OnEndWalk () {

		StartCoroutine( 
			LerpWeight( 
				WALK_TAG,
				1f,
				0f,
				0.2f
			)
		);
	}
	
	
	// Running
	private bool _wasRunning;

	private void OnBeginRun () {

		StartCoroutine( 
			LerpWeight( 
				RUN_TAG,
				0f,
				1f,
				0.2f
			)
		);
	}
	private void OnRun () {

		var prog = Mathf.Repeat( _stride, 1.0f );

		_animator.SetProgress( 
			RUN_TAG, 
			prog
		);
	}
	private void OnEndRun () {

		StartCoroutine( 
			LerpWeight( 
				RUN_TAG,
				1f,
				0f,
				0.2f
			)
		);
	}
	


	// helpers
	private IEnumerator LerpProgress ( string animation, float fromValue, float toValue, float time  ) {

		for ( float t=0f; t<time; t+=Time.deltaTime ) {
			_animator.SetProgress( animation , Mathf.Lerp( fromValue , toValue,  t/time ) );
			yield return null;
		}

		_animator.SetProgress( animation , toValue );
	}
	private IEnumerator LerpWeight ( string animation, float fromValue, float toValue, float time  ) {

		for ( float t=0f; t<time; t+=Time.deltaTime ) {
			_animator.SetWeight( animation , Mathf.Lerp( fromValue , toValue,  t/time ) );
			yield return null;
		}

		_animator.SetWeight( animation , toValue );
	}
}