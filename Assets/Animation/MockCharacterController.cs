using System.Collections;
using UnityEngine;

public class MockCharacterController : MonoBehaviour {

	[SerializeField] private Animator2 _animator;

	[Header( "Physics" )]	
	[SerializeField] private float _terminalVelocity = 20f;

	[Header( "Strides" )]
	[SerializeField] private float _stridesPerMeterWalk = 0.5f;
	[SerializeField] private float _stridesPerMeterRun  = 0.2f;

	[SerializeField] private Vector3 _distanceCovered;
	[SerializeField] private float _stride;
	[SerializeField] private Vector3 _velocity;
	[SerializeField] private Vector2 _strideGrowth;

	[SerializeField] private float _jumpPower;
	[SerializeField] private float _gravity;

	[SerializeField] private bool _strafing = false;

	[SerializeField] private Transform _camera;
	[SerializeField] private Transform _cameraPos;
	
	[SerializeField] private float _walkThreshold = 0.15f;
	[SerializeField] private float _maxTurnSpeed = 30f;

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
		get { return Physics.Raycast( transform.position, -Vector3.up, 0.3f ); }
	}



	// Mono
	private void Start () { 
	
		_animator.SetWeight( "Move", 1.0f );
	}
	private void Update () {

		ApplyVelocity( 
			Input.GetAxis( "Horizontal" ), 
			Input.GetAxis( "Vertical" ) 
		);


		ApplyGravity ();

		if ( Input.GetKeyDown( KeyCode.Space ) && _isOnGround ) {
			Jump ();
		}

		if ( !_strafing ) {
			FaceMomentum ();
			Balance ( 
				Input.GetAxis( "Horizontal" ), 
				Input.GetAxis( "Vertical" ) 
			);
		}

		_distanceCovered += _localVelocity * Time.deltaTime;
		_stride += new Vector2( _localVelocity.x, _localVelocity.z ).magnitude * Time.deltaTime * _stridesPerMeter;

		GetComponent<Rigidbody>().velocity = _velocity;
	}
	private void LateUpdate () {

	
		if ( !_isJumping && !_isFalling && !_wasJumping && !_wasFalling ) {
				
			OnGround ();
		
		} else {

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
		}

	

		_animator.SetProgress( 
			"Move",
			Mathf.Repeat( _stride, 1.0f )
		);



		// set camera garbage
		// _camera.position = Vector3.Lerp( _camera.position, _cameraPos.position, 0.5f);
		
		// var startRot = _camera.rotation;
		// _camera.LookAt( transform );
		// var targetRot = _camera.rotation;
		// _camera.rotation = Quaternion.Slerp( startRot, targetRot, 0.5f );
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
		
		_animator.SetProgress( "Fall", 0f );
		_animator.SetWeight( "Fall", 1.0f );
	}
	private void OnFall () {

		_animator.SetProgress( 
			"Fall",
			Mathf.Clamp01( _velocity.y / -_jumpPower )
		);
	}
	private void OnEndFall () {

		_animator.SetWeight( "Fall", 0.0f );

		_animator.SetWeight( "Land", 1.0f );

		StartCoroutine( LerpProgress( "Land", 0f, 1f, 0.2f ) );
		StartCoroutine( LerpWeight( "Land", 1f, 0f, 0.2f ) );
	}


	// Jumping
	private bool _wasJumping;

	private void OnBeginJump () {

		_animator.SetProgress( "Jump", 0f );
		_animator.SetWeight( "Jump", 1.0f, 0.1f );
	}
	private void OnJump () {
		
		_animator.SetProgress( 
			"Jump",
			Mathf.Clamp01( 1f - _velocity.y / _jumpPower )
		);
	}
	private void OnEndJump () {

		_animator.SetWeight( "Jump", 0.0f, 0 );
	}


	// Grounded

	private void OnGround () {

		_animator.SetGrowthPoint( 
			"Move", 
			_localVelocity.x / _terminalVelocity,
			_localVelocity.z / _terminalVelocity
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
		print ( animation + "," + toValue + " : "+ toValue );
		for ( float t=0f; t<time; t+=Time.deltaTime ) {
			_animator.SetWeight( animation , Mathf.Lerp( fromValue , toValue,  t/time ) );
			yield return null;
		}

		_animator.SetProgress( animation , toValue );
	}
}