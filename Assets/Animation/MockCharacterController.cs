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
			_isStrafing= true;
		}
		if ( package.BackLeft.Bumper_Up ) {
			_isStrafing= false;
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
	[SerializeField] private float _airMovementSpeed = 5f;
	[SerializeField] private float _walkMovementSpeed = 20f;
	[SerializeField] private float _runMovementSpeed = 20f;
	[SerializeField] private float _walkThreshold = 0.15f;
	[SerializeField] private float _runThreshold = 0.95f;

	[Header( "Animation Settings" )]
	[SerializeField] private float _stridesPerMeterWalk = 0.5f;
	[SerializeField] private float _stridesPerMeterRun  = 0.2f;


	[Header( "Gameplay Settings" )]
	[SerializeField] private bool _isStrafing = false;

	[SerializeField] private Dumpster.Physics.PhysicsPlane _down;
	[SerializeField] private LayerMask _mask;
	
	private Vector3 _distanceCovered;
	private Vector3 _velocity;
	private bool _ignoreGravity;
	private float _stride;

	private float _horizontal;
	private float _vertical;

	private AnimationDampener _jumpDampener;	
	private AnimationDampener _fallDampener;
	private AnimationDampener _landDampener;
	private AnimationDampener _aimDampener;

	private KindaBlendTree _runningBlendTree;
	private KindaBlendTree _walkingBlendTree;


	private Vector3 _localVelocity {
		get { return transform.InverseTransformVector( _velocity ); }
	}
	private float _stridesPerMeter {
		get { return Mathf.Lerp( _stridesPerMeterWalk, _stridesPerMeterRun, Vector3.Distance(Vector3.zero, _velocity)/_terminalVelocity ); }
	}
	private bool _isJumping {
		get { return !_isOnGround && _velocity.y > 0; }
	}
	private bool _isFalling {
		get { return !_isOnGround && _velocity.y <= 0f; }
	}
	private bool _isOnGround {
		get { return RaycastDown() < 1.1f; }
	}
	private bool _isInAir {
		get { return !_isOnGround; }
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

	private float RaycastDown () {

		var shortest = Mathf.Infinity;
		var startPos = transform.position + Vector3.up + new Vector3( -0.5f, 0f, -0.5f );

		for ( int x=0; x<5; x++ ) {
			for ( int y=0; y<5; y++ ) {
				
				RaycastHit hit;
				var pos = startPos + new Vector3( (float)x/5f, 0, (float)y/5f );
				var ray = -Vector3.up;
				
				if( Physics.Raycast( pos, ray, out hit, 1.1f, _mask ) ) {
					var d = Vector3.Distance( pos, hit.point);
					
					if ( d < shortest ) {
					 	shortest = d;
					}
				}
			}
		}

		return shortest;
	}
	private Vector3 GetGroundNormal () {

		RaycastHit rh = new RaycastHit();
		var shortest = Mathf.Infinity;
		var startPos = transform.position + Vector3.up + new Vector3( -0.5f, 0f, -0.5f );

		for ( int x=0; x<5; x++ ) {
			for ( int y=0; y<5; y++ ) {
				
				RaycastHit hit;
				var pos = startPos + new Vector3( (float)x/5f, 0, (float)y/5f );
				var ray = -Vector3.up;
				
				if( Physics.Raycast( pos, ray, out hit, 1.1f, _mask ) ) {
					var d = Vector3.Distance( pos, hit.point);
					
					if ( d < shortest ) {
					 	shortest = d;
					 	rh = hit;
					}
				}
			}
		}

		return rh.normal;
	}


	// Mono
	private void Start () { 
		
		EdensGarden.Instance.Input.RegisterToInputLayer( "Testing", this );
		EdensGarden.Instance.Input.RequestInput( "Testing" );

		_aimDampener  = new AnimationDampener( _animator, "Aim" );
		_jumpDampener = new AnimationDampener( _animator, "Jump" );
		_fallDampener = new AnimationDampener( _animator, "Fall" );
		_landDampener = new AnimationDampener( _animator, "Land" );

		_runningBlendTree = new KindaBlendTree( 
			animator : _animator,
			posX : "Run_Right",
			negX : "Run_Left",
			posY : "Run_Forward",
			negY : "Run_Back"
		);

		_walkingBlendTree = new KindaBlendTree( 
			animator : _animator,
			posX : "Walk_Right",
			negX : "Walk_Left",
			posY : "Walk_Forward",
			negY : "Walk_Back"
		);

		_animator.SetWeight( "Idle", 1f );
	}
	private void Update () {
		

		if ( _isOnGround ) {
		
			CalculateVelocityOnGround ();
		
		} else {
			
			CalculateVelocityInAir ();
		}

	
		// rotate character
		if ( _isStrafing ) {
			FaceCameraForward ();

		} else {
			FaceMomentum ();
		}

		
		// update values
		UpdateDistanceCovered ();
		UpdateStride ();
		
		
		// stop ignoring gravity	
		_ignoreGravity = false;
	}
	private void FixedUpdate () {

		UpdateRigidbody ();
	}
	private void LateUpdate () {

		Animate ();
	}
	private void OnDrawGizmos () {

		var shortest = Mathf.Infinity;
		var startPos = transform.position + Vector3.up + new Vector3( -0.5f, 0f, -0.5f );

		for ( int x=0; x<5; x++ ) {
			for ( int y=0; y<5; y++ ) {
				
				RaycastHit hit;
				var pos = startPos + new Vector3( (float)x/5f, 0, (float)y/5f );
				var ray = -Vector3.up;
				
				Debug.DrawRay( pos, ray * 1.2f );
			}
		}

		Gizmos.color = Color.red;
		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  _horizontal, _vertical );
		var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
		var groundNormal = GetGroundNormal();

		var right = Vector3.Cross( Vector3.up, inputVector );
		var newVector = Vector3.Cross( right, groundNormal );

		Gizmos.DrawRay( transform.position, newVector * 5);
	}
	private void UpdateDistanceCovered () {
	
		_distanceCovered += _localVelocity * Time.deltaTime;
	}
	private void UpdateStride () {
	
		_stride += new Vector2( _localVelocity.x, _localVelocity.z ).magnitude * Time.deltaTime * _stridesPerMeter;
	}
	private void UpdateRigidbody () {

		GetComponent<Rigidbody>().velocity = Vector3.zero;
		GetComponent<Rigidbody>().MovePosition( transform.position + (_velocity * Time.fixedDeltaTime) );
	}
	private void Animate () {

		// Strafing
		if ( !_wasStrafing && _isStrafing ) {
			OnBeginStrafing ();
		} else if ( _wasStrafing && !_isStrafing ) {
			OnEndStrafing ();
		} else if ( _isStrafing) { 
			OnStrafing (); 
		}

		_wasStrafing = _isStrafing;
		

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



	// Physics
	private void CalculateVelocityOnGround () {

		// if no input just abort
		if ( Mathf.Approximately( _inputMagnitude, 0f ) ) {
			_velocity = new Vector3( 0, 0, 0 );
			return;
		}


		// calculate speed
		var speed = 0f;
		
		if ( _isWalking ) { speed = _walkMovementSpeed; }
		if ( _isRunning ) { speed = _runMovementSpeed; }

		
		// input vector
		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  _horizontal, _vertical );
		var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
		
		
		// ground vector
		var groundNormalVector = GetGroundNormal();

		
		// new direction vector
		var inputRight = Vector3.Cross( Vector3.up, inputVector );
		var newVector = Vector3.Cross( inputRight, groundNormalVector );

		
		// calculate new velocity
		var newVelocity = newVector * speed;


		// set new velocity
		_velocity = Vector3.Lerp( _velocity, newVelocity, 0.5f );
	}
	private void CalculateVelocityInAir () {
		
		if ( !_ignoreGravity ) {
			_velocity += Vector3.down * Time.deltaTime * _gravity;
		}

		if ( Mathf.Approximately( _inputMagnitude, 0f ) ) {
			_velocity = new Vector3( 0, _velocity.y, 0 );
			return;
		}

		// input vector
		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  _horizontal, _vertical );
		var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
		
		var newVelocity = inputVector * _runMovementSpeed;
		var xzVelocity = new Vector3( newVelocity.x, _velocity.y, newVelocity.z );
		
		_velocity = Vector3.Lerp( _velocity, xzVelocity, 0.2f );
	}
	

	private void ApplyVelocity ( float horizontalInput, float verticalInput ) {

		if ( Mathf.Approximately( _inputMagnitude, 0f ) ) {
			_velocity = new Vector3( 0, 0, 0 );
			return;
		}

		var speed = 0f;
		
		if ( _isWalking ) { speed = _walkMovementSpeed; }
		if ( _isRunning ) { speed = _runMovementSpeed; }
		if ( !_isOnGround ) { speed = _runMovementSpeed; }

		
		// input vector
		var cameraRight = Camera.main.transform.right;
		var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
		var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  horizontalInput, verticalInput );
		var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
		
		// ground vector
		var groundNormal = GetGroundNormal();

		// new direction vector
		var inputRight = Vector3.Cross( Vector3.up, inputVector );
		var newVector = Vector3.Cross( inputRight, groundNormal );


		var newVelocity = newVector * speed;


		if ( _isOnGround ) {
			_velocity = new Vector3( newVelocity.x, newVelocity.y, newVelocity.z );
		} else {
			_velocity = new Vector3( newVelocity.x, _velocity.y, newVelocity.z  );
		}
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



	// Strafing
	private bool _wasStrafing;

	private void OnBeginStrafing () {

		_aimDampener.SetProgress( 1f );
		_aimDampener.SetWeight( 1f, 0.2f );
	}
	private void OnStrafing () {
	}
	private void OnEndStrafing () {

		_aimDampener.SetWeight( 0f, 0.2f );
	}


	// Falling
	private bool _wasFalling;

	private void OnBeginFall () {
		
		_fallDampener.SetProgress( 0f );
		_fallDampener.SetWeight( 1f, 0.2f );
	}
	private void OnFall () {

		var prog = Mathf.Clamp01( _velocity.y / -_jumpPower );
		
		_fallDampener.SetProgress( prog );
	}
	private void OnEndFall () {
	}


	// Jumping
	private bool _wasJumping;

	private void OnBeginJump () {

		_jumpDampener.SetProgress( 0f );
		_jumpDampener.SetWeight( 1f );
	}
	private void OnJump () {

		var prog = Mathf.Clamp01( 1f - _velocity.y / _jumpPower );

		_jumpDampener.SetProgress( prog );
	}
	private void OnEndJump () {
	}


	// Grounded
	private bool _wasOnGround;

	private void OnBeginOnGround () {
		_fallDampener.SetWeight( 0f, 0.2f );
		_jumpDampener.SetWeight( 0f, 0.2f );
		
		_landDampener.SetWeight( 1f );
		_landDampener.SetProgress( 0, 0.3f );
		_landDampener.SetWeight( 0, 0.3f );
	}
	private void OnGround () {
	}
	private void OnEndOnGround () {
	}

	
	// Idle
	private bool _wasIdling;

	private void OnBeginIdle () {
	}
	private void OnIdle () {

		var prog = Mathf.Repeat( Time.time, 1.0f );

		_animator.SetProgress( 
			"Idle", 
			prog
		);
	}
	private void OnEndIdle () {
	}

	
	// Walking
	private bool _wasWalking;

	private void OnBeginWalk () {

		_walkingBlendTree.SetWeight( 1f, 0.2f );
	}
	private void OnWalk () {

		var prog = Mathf.Repeat( _stride, 1.0f );

		_walkingBlendTree.SetProgress( prog );

		_walkingBlendTree.SetBlendPoint( 
			_localVelocity.x / _walkMovementSpeed,
			_localVelocity.z / _walkMovementSpeed
		);
	}
	private void OnEndWalk () {

		_walkingBlendTree.SetWeight( 0f, 0.2f );
	}
	
	
	// Running
	private bool _wasRunning;

	private void OnBeginRun () {

		_runningBlendTree.SetWeight( 1f, 0.2f );
	}
	private void OnRun () {

		var prog = Mathf.Repeat( _stride, 1.0f );
		
		_runningBlendTree.SetProgress( prog );

		_runningBlendTree.SetBlendPoint( 
			_localVelocity.x / _runMovementSpeed,
			_localVelocity.z / _runMovementSpeed
		);
	}
	private void OnEndRun () {

		_runningBlendTree.SetWeight( 0f, 0.2f );
	}
}