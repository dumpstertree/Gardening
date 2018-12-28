using UnityEngine;

namespace Dumpster.Controllers {
	
	public class ThirdPersonCharacterController : MonoBehaviour {

		
		// ************** Public ****************
		
		public bool IsDashing { 
			get { return _isDashing; }
			set { SetDashing( value ); }
		}
		public bool IsJumping { 
			get { return _isJumping; }
			set { SetJumping( value ); }
		}
		
		public float HorizontalInput { get; set; }
		public float VerticalInput { get; set; }
		public bool IsStrafing { get; set; }

		public Vector3 Velocity {
			get{ return _lastInputVelocity + new Vector3( 0, _verticalVelocity, 0); }
		}
		public Vector3 LocalVelocity {
			get{ return transform.InverseTransformVector( Velocity ); }
		}
		public float DashSpeed {
			get{ return _maxDashMovementSpeed; }
		}
		public float WalkSpeed {
			get{ return _walkMovementSpeed; }
		}
		public float RunSpeed {
			get{ return _runMovementSpeed; }
		}
		public float JumpPower {
			get{ return _jumpPower; }
		}
		public float TerminalVelocity {
			get { return _terminalVelocity; }
		}
		
		private bool _jumpIsDown;
		private float _jumpStartTime;
		private float _verticalVelocity;
		
		private Vector3 _lastInputVelocity;
		public Vector3 InputVelocity;

		private bool _isDashing;
		private bool _isJumping;
		private float _dashStartTime;
		
		// ************** Private ****************

		[Header( "Walking" )]
		[SerializeField] private float _walkThreshold = 0.15f;
		[SerializeField] private float _walkMovementSpeed = 20f;
		[SerializeField] private float _walkTurnAccuracy = 0.9f;

		[Header( "Running" )]
		[SerializeField] private float _runThreshold = 0.95f;
		[SerializeField] private float _runMovementSpeed = 20f;
		[SerializeField] private float _runTurnAccuracy = 0.6f;

		[Header( "Dashing" )]
		[SerializeField] private float _dashBurstTime = 1.0f;
		[SerializeField] private float _minDashMovementSpeed = 20f;
		[SerializeField] private float _maxDashMovementSpeed = 20f;
		[SerializeField] private float _dashGravityMultiplier = 0.5f;
		[SerializeField] private float _dashTurnAccuracy = 0.2f;

		[Header( "Jumping" )]
		[SerializeField] private float _jumpPower = 15f;
		[SerializeField] private float _jumpingGravityMultiplier = 1f;

		[Header( "Falling" )]
		[SerializeField] private float _airMovementSpeed = 20f;
		[SerializeField] private float _airTurnAccuracy = 0.3f;
		[SerializeField] private float _fallingGravityMultiplier = 1.5f;

		[Header( "General" )]
		[SerializeField] private float _gravity = 25f;
		[SerializeField] private float _terminalVelocity = 20f;

		[Header( "Gameplay Settings" )]
		[SerializeField] private LayerMask _mask;
		[SerializeField] private float _height = 1.1f;
		

		public bool IsJumpingUp {
			get { return !IsOnGround && Velocity.y > 0; }
		}
		public bool IsFallingDown {
			get { return !IsOnGround && Velocity.y <= 0f; }
		}
		public bool IsOnGround {
			get { return Velocity.y <=0 && !_jumping && RaycastDown() < _height; }
		}
		public bool IsIdling {
			get{ return IsOnGround && _inputMagnitude < _walkThreshold; }
		}
		public bool IsWalking {
			get{ return IsOnGround && _inputMagnitude > _walkThreshold && _inputMagnitude < _runThreshold; }
		}
		public bool IsRunning {
			get{ return IsOnGround && _inputMagnitude > _runThreshold; }
		}
		private float _inputMagnitude {
			get{ return new Vector2( HorizontalInput, VerticalInput ).magnitude; }
		}
		private Vector3 _inputDirection {
			get{
			
				if ( new Vector2( HorizontalInput, VerticalInput ).magnitude < 0.1f ) {
					return Vector3.zero;
				}

				var cameraRight = Camera.main.transform.right;
				var cameraForward = Vector3.Cross( cameraRight, Vector3.up );
				var inputDegrees = Mathf.Rad2Deg * Mathf.Atan2(  HorizontalInput, VerticalInput );
				var inputVector = Quaternion.AngleAxis( inputDegrees, Vector3.up ) * cameraForward;
							
				// ground vector
				var groundNormalVector = GetGroundNormal();

				// new direction vector
				var inputRight = Vector3.Cross( Vector3.up, inputVector );
				var newVector = Vector3.Cross( inputRight, groundNormalVector );

				return inputVector.normalized;
			}
		}
		private bool _isInAir {
			get { return !IsOnGround; }
		}
		private float _speed {
			get { return Vector3.Distance( Vector3.zero, new Vector3( Velocity.x, 0, Velocity.z ) ); }
		}

		private bool _jumping;
		private float _turnAccuracy;
		
		private float RaycastDown () {

			var shortest = Mathf.Infinity;
			var rows = 5;
			var collumns = 5;
			var width = 0.45f;
			var depth = 0.45f;
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


		// mono
		private void Update () {
			
			if ( IsDashing ) {
				
				CalculateVelocityInDash ();
			
			} else if ( IsOnGround ) {
			
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

			_jumping = false;
		}
		private void FixedUpdate () {

			UpdateRigidbody ();
		}
		
		
		// physics
		private void UpdateRigidbody () {

			var inputVelocity = Vector3.Lerp( _lastInputVelocity, InputVelocity, _turnAccuracy );
			
			// apply gravity
			GetComponentInParent<CharacterController>().Move( new Vector3( 0, _verticalVelocity, 0 ) * Time.fixedDeltaTime );
			
			// apply input movement
			GetComponentInParent<CharacterController>().Move( inputVelocity * Time.fixedDeltaTime );

			_lastInputVelocity = inputVelocity;
		}


		// dashing
		private void SetDashing ( bool dashing ) {

			if ( dashing != _isDashing ) {
				if (  dashing ) { StartDashing(); }
				if ( !dashing ) { StopDashing(); }
			}

			_isDashing = dashing;
		}
		private void StartDashing () {

			_dashStartTime = Time.time;
		}
		private void StopDashing () {
		}


		// jumping
		private void SetJumping ( bool jumping ) {

			if ( _isJumping != jumping ) {
				if (  jumping ) { StartJumping(); }
				if ( !jumping ) { StopJumping(); }
			}

			jumping = _isJumping;
		}
		private void StartJumping () {
			
			if ( IsOnGround ) {
				
				_jumping = true;
				_jumpIsDown = true;

				_verticalVelocity += _jumpPower;
				// IsDashing = false;
			}
		}
		private void StopJumping () {
			
			_jumpIsDown = false;
		}


		// get velocity
		private void CalculateVelocityInDash () {

			_turnAccuracy = _dashTurnAccuracy;

			_verticalVelocity -= IsOnGround ? 0f : _gravity * _dashGravityMultiplier * Time.deltaTime;
			InputVelocity = _inputDirection * Mathf.Lerp( _minDashMovementSpeed, _maxDashMovementSpeed, 1f - Mathf.Clamp01( (Time.time-_dashStartTime)/_dashBurstTime ) );
		}
		private void CalculateVelocityOnGround () {

			_turnAccuracy = IsRunning ? _runTurnAccuracy : _walkTurnAccuracy;

			// calculate speed
			var speed = 0f;
			
			if ( IsWalking ) { speed = _walkMovementSpeed; }
			if ( IsRunning ) { speed = _runMovementSpeed; }

			// calculate new velocity
			var newVelocity = _inputDirection * speed;

			// set new velocity
			_verticalVelocity = 0f;
			InputVelocity = newVelocity;
		}
		private void CalculateVelocityInAir () {
			
			_turnAccuracy = _airTurnAccuracy;

			// holding jump
			if( IsJumpingUp && _jumpIsDown ) {
				_verticalVelocity -= _gravity * _jumpingGravityMultiplier * Time.deltaTime;

			// falling
			} else {
				_verticalVelocity -= _gravity * _fallingGravityMultiplier * Time.deltaTime;
			}

			var speed = _speed;
			if ( speed < _airMovementSpeed ) {
				speed = _airMovementSpeed;
			}

			InputVelocity = _inputDirection * speed;
		}


		// rotation
		private void FaceMomentum () {

			if ( _lastInputVelocity == Vector3.zero ) {
				return;
			}

			// var up = (Vector3.up + ((_inputVelocity + transform.position) - transform.position).normalized).normalized;
			GetComponentInParent<CharacterController>().transform.rotation = Quaternion.LookRotation( _lastInputVelocity, Vector3.up ); 
		}
		private void FaceCameraForward () {

			var cameraForward = Camera.main.transform.forward;
			var right = Vector3.Cross( cameraForward, Vector3.up );
			var forward = Vector3.Cross( Vector3.up, right );

			GetComponentInParent<CharacterController>().transform.rotation = Quaternion.LookRotation( forward, Vector3.up );
		}

		
		// gizmos
		private void OnDrawGizmos () {

			var up = Vector3.up + ((InputVelocity + transform.position) - transform.position).normalized;
			Debug.DrawRay( transform.position, up, Color.red, 0.01f );
		}
	}
}