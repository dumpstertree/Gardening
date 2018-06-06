using UnityEngine;

namespace Dumpster.Physics {
	
	public class Controller: MonoBehaviour  {

		// data types
		public struct Package {
			
			public Vector3 Velocity { get; }
			public Vector3 ChangeInVelocity { get; }
			public Vector3 Position { get; }
			public Vector3 ChangeInPosition { get; }
			public bool DownIsColliding { get; }
			public bool UpIsColling { get; }
			public bool ForwardIsColliding { get; }
			public bool BackisColliding { get; }
			public bool RightIsColliding { get; }
			public bool LeftIsColliding { get; }

			public Package ( Vector3 velocity,
							 Vector3 changeInVelocity ,
							 Vector3 position,
							 Vector3 changeInPosition,
							 bool downIsColliding,
							 bool upIsColling,
							 bool forwardIsColliding,
							 bool backisColliding,
							 bool rightIsColliding,
							 bool leftIsColliding ) {

				Velocity = velocity;
				ChangeInVelocity = changeInVelocity;
				Position = position;
				ChangeInPosition = changeInPosition;
				DownIsColliding = downIsColliding;
				UpIsColling = upIsColling;
				ForwardIsColliding = forwardIsColliding;
				BackisColliding = backisColliding;
				RightIsColliding = rightIsColliding;
				LeftIsColliding = leftIsColliding;
			}
		}	

		
		// ****************** Public *********************
		
		public Package State {
			get { return _package; }
		}
		public void AddVelocity ( Vector3 change ) {
			CommitVelocityChanges( change );
		}		
		public void MovePosition( Vector3 offset ) {
			CommitPositionChanges( offset );
		}


		// ****************** Private *********************

		[Header( "Use Gravity" )]
		[SerializeField] private bool _useGravity = true;

		[Header( "Physics Planes" )]
		[SerializeField] private PhysicsPlane[] _downPlanes;
		[SerializeField] private PhysicsPlane[] _upPlanes;
		[SerializeField] private PhysicsPlane[] _forwardPlanes;
		[SerializeField] private PhysicsPlane[] _backPlanes;
		[SerializeField] private PhysicsPlane[] _leftPlanes;
		[SerializeField] private PhysicsPlane[] _rightPlanes;
		
		private const float RAYCAST_INSET = 0.1f;
		private const float RAYCAST_DISTANCE = 0.01f;
		public const float GRAVITY = 9.8f;
		public const float TERMINAL_VELOCITY = 10f;

		private Package _package;
		private Vector3 _position;
		private Vector3 _posOffset;
		private Vector3 _velocity;
		private Vector3 _velocityChange;


		// mono
		private void Start () {

			_position = transform.position;
		}
		private void LateUpdate () {
		
			// update package
			_package = new Package(
			 	velocity : _velocity,
				changeInVelocity :_velocityChange,
				position : _position,
				changeInPosition : _posOffset,
				downIsColliding : IsColliding ( _downPlanes ),
				upIsColling : IsColliding ( _upPlanes ) ,
				forwardIsColliding : IsColliding ( _forwardPlanes ) ,
				backisColliding : IsColliding ( _backPlanes ) ,
				rightIsColliding : IsColliding ( _rightPlanes ) ,
				leftIsColliding : IsColliding ( _leftPlanes )
			);


			// gravity
			ApplyGravity ();

			// velocity
			ApplyVelocity ();

			// offsets
			ApplyOffset ( _downPlanes );
			ApplyOffset ( _upPlanes );
			ApplyOffset ( _forwardPlanes );
			ApplyOffset ( _backPlanes );
			ApplyOffset ( _leftPlanes );
			ApplyOffset ( _rightPlanes );
		}


		// apply physics
		private void ApplyGravity () {

			if ( _useGravity ) {
				CommitVelocityChanges ( Vector3.down * (GRAVITY * Time.deltaTime) );
			}
		}
		private void ClampVelocity () {

			// cancel velocity that has a collision
			if ( IsColliding ( _upPlanes ) ) {
				if ( _velocity.y > 0 ) {
					_velocity.y = 0f;
				}
			}
			if ( IsColliding ( _downPlanes ) ) {
				if ( _velocity.y < 0 ) {
					_velocity.y = 0f;
				}
			}
			if ( IsColliding ( _forwardPlanes ) ) {
				if ( _velocity.z > 0 ) {
					_velocity.z = 0f;
				}
			}
			if ( IsColliding ( _backPlanes ) ) {
				if ( _velocity.z < 0 ) {
					_velocity.z = 0f;
				}
			}
			if ( IsColliding ( _rightPlanes ) ) {
				if ( _velocity.x > 0 ) {
					_velocity.x = 0f;
				}
			}
			if ( IsColliding ( _leftPlanes ) ) {
				if ( _velocity.z < 0 ) {
					_velocity.z = 0f;
				}
			}

			_velocity.x = Mathf.Clamp( _velocity.x, -TERMINAL_VELOCITY, TERMINAL_VELOCITY );
			_velocity.y = Mathf.Clamp( _velocity.y, -TERMINAL_VELOCITY, TERMINAL_VELOCITY );
			_velocity.z = Mathf.Clamp( _velocity.z, -TERMINAL_VELOCITY, TERMINAL_VELOCITY );
		}
		private void ApplyVelocity () {

			PushVelocityChanges ();
			CommitPositionChanges( _velocity * Time.deltaTime );
			PushPositionChanges();
		}
		private void ApplyOffset( PhysicsPlane[] forPlanes ) {
			
			foreach ( PhysicsPlane plane in forPlanes ) {
				
				plane.ForceUpdate ();
				
				if ( plane.IsColiding ) {

					CommitPositionChanges( plane.Forward * plane.InsetIntoCollision );
					PushPositionChanges ();
				}
			}
		}
		private bool IsColliding ( PhysicsPlane[] forPlanes ) {
			
			foreach ( PhysicsPlane plane in forPlanes ) {
				if ( plane.IsColiding ) {
					return true;
				}
			}

			return false;
		}


		// commit changes
		private void CommitVelocityChanges ( Vector3 change ) {

			_velocityChange += change;
		}	
		private void PushVelocityChanges () {

			// move object
			_velocity += _velocityChange;
			
			// clamp the new values
			ClampVelocity();
			
			// clear offset
			_velocityChange = Vector3.zero;
		}
		private void CommitPositionChanges ( Vector3 offset ) {

			_posOffset += offset;
		}
		private void PushPositionChanges () {

			// move object
			transform.position = _position + _posOffset;
			_position = transform.position;

			// clear offset
			_posOffset = Vector3.zero;
		}
	}
}