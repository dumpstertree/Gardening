using UnityEngine;

namespace Dumpster.Controllers {
	
	public class ThirdPersonCharacterController : MonoBehaviour {

		
		// ************** Public ****************

		public float HorizontalInput { get; set; }
		public float VerticalInput { get; set; }
		public bool IsStrafing { get; set; }
		public bool IsDashing { get; set; }

		public bool IsSwinging { get; set; }
		public float SwingProgress { get; set; }

		public bool IsSwingingCombo1 { get; set; }
		public float SwingProgressCombo1 { get; set; }

		public bool IsSwingingCombo2 { get; set; }
		public float SwingProgressCombo2 { get; set; }

		public void Jump () {
			
			if ( _isOnGround ) {
				_jumping = true;
				_velocity += Vector3.up * _jumpPower;
			}
		}


		// ************** Private ****************

		[Header( "References" )]
		[SerializeField] private Rigidbody _rigidBody;
		[SerializeField] private Animator2 _animator;

		[Header( "Physics" )]	
		[SerializeField] private float _terminalVelocity = 20f;
		[SerializeField] private float _jumpPower = 15f;
		[SerializeField] private float _gravity = 25f;

		[Header( "Movement Settings" )]
		[SerializeField] private float _airMovementSpeed = 20f;
		[SerializeField] private float _walkMovementSpeed = 20f;
		[SerializeField] private float _runMovementSpeed = 20f;
		[SerializeField] private float _dashMovementSpeed = 20f;
		[SerializeField] private float _walkThreshold = 0.15f;
		[SerializeField] private float _runThreshold = 0.95f;

		[Header( "Animation Settings" )]
		[SerializeField] private float _stridesPerMeterWalk = 0.5f;
		[SerializeField] private float _stridesPerMeterRun  = 0.2f;

		[Header( "Gameplay Settings" )]
		[SerializeField] private LayerMask _mask;
		
		private Vector3 _distanceCovered;
		private Vector3 _velocity;
		private bool _jumping;
		private float _stride;

		private AnimationDampener _combo1DampenerIdling;
		private AnimationDampener _combo1DampenerMoving;

		private AnimationDampener _combo2Dampener;

		private AnimationDampener _attackDampener;
		private AnimationDampener _jumpDampener;	
		private AnimationDampener _fallDampener;
		private AnimationDampener _landDampener;
		private AnimationDampener _aimDampener;

		private KindaBlendTree _runningBlendTree;
		private KindaBlendTree _dashingBlendTree;
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
		private bool _isSwingingCombo1Moving {
			get{ return IsSwingingCombo1 && (_isWalking || _isRunning); }
		}
		private bool _isSwingingCombo1Idling {
			get{ return IsSwingingCombo1 && _isIdling; }
		}


		private bool _leftFootDown {
			get{ return Mathf.Repeat( _stride, 1.0f ) < 0.5f; }
		}
		private bool _rightFootDown {
			get{ return !_leftFootDown; }
		}
		private float _inputMagnitude {
			get{ return new Vector2( HorizontalInput, VerticalInput ).magnitude; }
		}


		private float RaycastDown () {

			var shortest = Mathf.Infinity;
			var rows = 5;
			var collumns = 5;
			var width = 0.5f;
			var depth = 0.5f;
			var startPos = transform.position + Vector3.up + new Vector3( -width/2f, 0f, -depth/2f );
			
			Gizmos.color = Color.green;
			for ( int x=0; x<rows; x++ ) {
				for ( int y=0; y<collumns; y++ ) {
					
					RaycastHit hit;
					var pos = startPos + new Vector3( (float)x/(float)rows * width, 0, (float)y/(float)collumns * depth );
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

			RaycastHit? rh = null;
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

			if ( rh.HasValue) {
				return rh.Value.normal;
			} else {
				return Vector3.up;
			}
		}


		// Mono
		private void Update () {
			
			if ( IsDashing ) {

				CalculateVelocityInDash ();

			} else if ( _isOnGround && !_jumping) {
			
				CalculateVelocityOnGround ();
			
			} else {
				
				CalculateVelocityInAir ();
			}

			// rotate character
			if ( IsStrafing ) {
				FaceCameraForward ();

			} else {
				FaceMomentum ();
			}

			
			// update values
			UpdateDistanceCovered ();
			UpdateStride ();
			
			
			// stop ignoring gravity	
			_jumping = false;
		}
		private void Start () { 

			_combo1DampenerIdling  = new AnimationDampener( _animator, "Combo 1 Idling" );
			_combo1DampenerMoving  = new AnimationDampener( _animator, "Combo 1 Moving" );

			_combo2Dampener = new AnimationDampener( _animator, "Combo 2" );
			_attackDampener  = new AnimationDampener( _animator, "Attack" );
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

			_dashingBlendTree = new KindaBlendTree( 
				animator : _animator,
				posX : "Dash_Right",
				negX : "Dash_Left",
				posY : "Dash_Forward",
				negY : "Dash_Back"
			);


			_animator.SetWeight( "Idle", 1f );
		}


		private void FixedUpdate () {

			UpdateRigidbody ();
		}
		private void LateUpdate () {

			Animate ();
		}
		private void UpdateDistanceCovered () {
		
			_distanceCovered += _localVelocity * Time.deltaTime;
		}
		private void UpdateStride () {
		
			_stride += new Vector2( _localVelocity.x, _localVelocity.z ).magnitude * Time.deltaTime * _stridesPerMeter;
		}
		private void UpdateRigidbody () {

			_rigidBody.velocity = Vector3.zero;
			_rigidBody.MovePosition( transform.position + (_velocity * Time.fixedDeltaTime) );
		}
		private void Animate () {


			// Swinging Combo 1 Idle
			if ( !_wasSwingingCombo1Idling && _isSwingingCombo1Idling) {
				OnBeginSwingingCombo1Idling ();
			} else if ( _wasSwingingCombo1Idling && !_isSwingingCombo1Idling) {
				OnEndSwingingCombo1Idling ();
			} else if ( _isSwingingCombo1Idling) { 
				OnSwingingCombo1Idling (); 
			}

			_wasSwingingCombo1Idling = _isSwingingCombo1Idling;


			// Swinging Combo 1 Move
			if ( !_wasSwingingCombo1Moving && _isSwingingCombo1Moving  ) {
				OnBeginSwingingCombo1Moving ();
			} else if ( _wasSwingingCombo1Moving && !_isSwingingCombo1Moving ) {
				OnEndSwingingCombo1Moving ();
			} else if ( _isSwingingCombo1Moving ) { 
				OnSwingingCombo1Moving (); 
			}

			_wasSwingingCombo1Moving = _isSwingingCombo1Moving;


			// Swinging Combo 1
			if ( !_wasSwingingCombo2 && IsSwingingCombo2  ) {
				OnBeginSwingingCombo2 ();
			} else if ( _wasSwingingCombo2  && !IsSwingingCombo2  ) {
				OnEndSwingingCombo2 ();
			} else if ( IsSwingingCombo2 ) { 
				OnSwingingCombo2 (); 
			}

			_wasSwingingCombo2 = IsSwingingCombo2;


			// Strafing
			if ( !_wasStrafing && IsStrafing ) {
				OnBeginStrafing ();
			} else if ( _wasStrafing && !IsStrafing ) {
				OnEndStrafing ();
			} else if ( IsStrafing) { 
				OnStrafing (); 
			}

			_wasStrafing = IsStrafing;
			

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
		

			// Dash
			if ( !_wasDashing && IsDashing ) {
				OnBeginDash ();
			} else if ( _wasDashing && !IsDashing ) {
				OnEndDash ();
			} else if ( IsDashing ) { 
				OnDash (); 
			}

			_wasDashing =  IsDashing;
		}


		// Physics
		private void CalculateVelocityInDash () {
			
			// input vector
			var cameraRight = Camera.main.transform.right;
			var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
			var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  HorizontalInput, VerticalInput );
			var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
			
			
			// ground vector
			var groundNormalVector = GetGroundNormal();

			
			// new direction vector
			var inputRight = Vector3.Cross( Vector3.up, inputVector );
			var newVector = Vector3.Cross( inputRight, groundNormalVector );

			
			// calculate new velocity
			var newVelocity = newVector * _dashMovementSpeed;
			// var averagedVelocity =  Vector3.Lerp( _velocity.normalized, newVector.normalized, 0.2f ) * _dashMovementSpeed;

			// set new velocity
			_velocity = Vector3.Lerp( _velocity, newVelocity, 0.2f );
		}
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
			var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  HorizontalInput, VerticalInput );
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

			if ( !_jumping ) {
				_velocity += Vector3.down * Time.deltaTime * _gravity;
			}

			if ( Mathf.Approximately( _inputMagnitude, 0f ) ) {
				_velocity = new Vector3( 0, _velocity.y, 0 );
				return;
			}

			// input vector
			var cameraRight = Camera.main.transform.right;
			var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
			var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  HorizontalInput, VerticalInput );
			var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
			
			var newVelocity = inputVector * _airMovementSpeed;
			var xzVelocity = new Vector3( newVelocity.x, _velocity.y, newVelocity.z );
			
			_velocity = Vector3.Lerp( _velocity, xzVelocity, 0.2f );
		}
		private void FaceMomentum () {

			var xzVelocity = new Vector3( _velocity.x, 0, _velocity.z );
			
			if ( xzVelocity.magnitude < 0.1f ) {
				return;
			}

			_rigidBody.MoveRotation( Quaternion.LookRotation( xzVelocity ) ); 
		}
		private void FaceCameraForward () {

			var cameraForward = Camera.main.transform.forward;
			var right = Vector3.Cross( cameraForward, Vector3.up );
			var forward = Vector3.Cross( Vector3.up, right );

			_rigidBody.MoveRotation( Quaternion.LookRotation( forward, Vector3.up ) );
		}


		// Combo 1
		private bool _wasSwingingCombo1Idling;

		private void OnBeginSwingingCombo1Idling () {
				
			_combo1DampenerIdling.SetProgress( 0f );
			_combo1DampenerIdling.SetWeight( 1f, 0.2f );
		}
		private void OnSwingingCombo1Idling () {

			_combo1DampenerIdling.SetProgress( SwingProgressCombo1 );
		}
		private void OnEndSwingingCombo1Idling () {

			_combo1DampenerIdling.SetWeight( 0f, 0.2f );		
		}


		private bool _wasSwingingCombo1Moving;

		private void OnBeginSwingingCombo1Moving () {
				
			_combo1DampenerMoving.SetProgress( 0f );
			_combo1DampenerMoving.SetWeight( 1f, 0.2f );
		}
		private void OnSwingingCombo1Moving () {

			_combo1DampenerMoving.SetProgress( SwingProgressCombo1 );
		}
		private void OnEndSwingingCombo1Moving () {

			_combo1DampenerMoving.SetWeight( 0f, 0.2f );		
		}


		// Combo 2
		private bool _wasSwingingCombo2;

		private void OnBeginSwingingCombo2 () {

			_combo2Dampener.SetProgress( 0f  );
			_combo2Dampener.SetWeight( 1f, 0.2f );
		}
		private void OnSwingingCombo2 () {

			_combo2Dampener.SetProgress( SwingProgressCombo2 );
		}
		private void OnEndSwingingCombo2 () {

			_combo2Dampener.SetWeight( 0f, 0.2f );
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


		// Dashing
		private bool _wasDashing;
		private float _dashStartTime;

		private void OnBeginDash () {

			_dashingBlendTree.SetWeight( 1f, 0.2f );
			_dashStartTime= Time.time;
		}
		private void OnDash () {

			_dashingBlendTree.SetProgress( Mathf.Clamp01(Time.time - _dashStartTime/0.5f) );

			_dashingBlendTree.SetBlendPoint( 
				_localVelocity.x / _dashMovementSpeed,
				_localVelocity.z / _dashMovementSpeed
			);
		}
		private void OnEndDash () {

			_dashingBlendTree.SetWeight( 0f, 0.2f );
		}
	}
}