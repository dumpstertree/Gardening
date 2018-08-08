using UnityEngine;

namespace Dumpster.Core.BuiltInModules {

	public class FollowCameraController : CameraController {

		[SerializeField] private float _lerpSpeed = 0.1f;
		[SerializeField] private Transform _followObject;
		[SerializeField] private float _distanceFromTarget = 4.0f;
		[SerializeField] private float _maxHeight = 1.0f;
		[SerializeField] private float _minHeight = 1.0f;

		private float _horizontalSensitivity = 0.4f;
		private float _horizontalRot = 0f;
		private float _verticalSensitivity = 0.1f;
		private float _verticalRot = 0.5f;

		private Vector2 _velocity;
		private float _acceleration = 20;
		private float _drag = 100f;

		public float CameraHorizontal{ get; set; }
		public float CameraVertical{ get; set; }

		public override void Control( Transform cameraTarget, Transform cameraFocus ) {

			MoveCameraTarget( cameraTarget, cameraFocus );
		}

		private void MoveCameraTarget ( Transform cameraTarget, Transform cameraFocus ) {
			
			// apply velocity
			if ( Mathf.Abs( CameraHorizontal ) > 0.01 || Mathf.Abs( CameraVertical ) > 0.01 ) {
				_velocity.x += CameraHorizontal * _acceleration * Time.deltaTime;
				_velocity.y += CameraVertical * _acceleration * Time.deltaTime;
			} else {
				_velocity.x = Mathf.Clamp( Mathf.Abs( _velocity.x ) - _drag * Time.deltaTime, 0f, 50 ) * ( _velocity.x > 0 ? 1 : -1 );
				_velocity.y = Mathf.Clamp( Mathf.Abs( _velocity.y ) - _drag * Time.deltaTime, 0f, 50 ) * ( _velocity.y > 0 ? 1 : -1 );
			}

			_horizontalRot += _velocity.x;
			_verticalRot += _velocity.y;
			_verticalRot = Mathf.Clamp( _verticalRot, -60, -20 );
			// _verticalRot = -45f;
				
			var p1 = _followObject.position;
			var p2 = cameraTarget.position;
			var angle = Mathf.Atan2( p2.x-p1.x, p2.z-p1.z ) * Mathf.Rad2Deg;


			// get rotation
			var cameraForward = Vector3.Cross( Vector3.up, UnityEngine.Camera.main.transform.right );
			var cameraRight = Vector3.Cross( Vector3.up , cameraForward );
			var rot = Quaternion.Euler( new Vector3( _verticalRot, _horizontalRot, 0 ) );
			var forwardVector = rot * Vector3.forward;
			
			
			// set position
			cameraTarget.position = _followObject.position + ( forwardVector * _distanceFromTarget );
			
		
			// set rotation

			var startRot = cameraTarget.transform.rotation;
			cameraTarget.LookAt( cameraFocus );
			var targetRot = cameraTarget.transform.rotation;

			cameraTarget.transform.rotation = Quaternion.Slerp( startRot, targetRot, _lerpSpeed );
		}

		private void OnDrawGizmos () {

			// var cameraForward = Vector3.Cross( Vector3.up, UnityEngine.Camera.main.transform.right );
			// var cameraRight = Vector3.Cross( Vector3.up , cameraForward );

			// Debug.DrawRay( transform.position, cameraRight);
		}



		// private void MoveCameraTarget ( Transform cameraTarget, Transform cameraFocus ) {
				
		// 	_horizontalRot += CameraHorizontal * _horizontalSensitivity;
		// 	_horizontalRot = _horizontalRot * _lerpSpeed;

		// 	_verticalRot = Mathf.Clamp( _verticalRot + (CameraVertical * _verticalSensitivity), -1f, 1f );

		// 	var startPos = cameraTarget.position;

		// 	var p1 = _followObject.position;
		// 	var p2 = cameraTarget.position;
		// 	var angle = Mathf.Atan2( p2.x-p1.x, p2.z-p1.z );
			
		// 	var yRot = Mathf.Lerp( angle, angle + _horizontalRot, _lerpSpeed ) * Mathf.Rad2Deg;
		// 	var zRot = Mathf.Lerp( transform.localRotation.z, _verticalRot, _lerpSpeed ) * Mathf.Rad2Deg;
			
		// 	var targetRotation = Quaternion.Euler( new Vector3( _followObject.transform.rotation.eulerAngles.x, yRot, zRot ) );
		// 	var targetWorldForward = Vector3.Cross( Vector3.up, _followObject.right );
			
		// 	// set xz
		// 	cameraTarget.rotation = targetRotation;
		// 	cameraTarget.position = _followObject.position + (targetWorldForward * _distanceFromTarget);
			
		// 	// set y
		// 	cameraTarget.transform.position += new Vector3( 0f, Mathf.Lerp( _minHeight, _maxHeight, _verticalRot ), 0f);

		// 	// lerp the value to the new one 
		// 	cameraTarget.transform.position = Vector3.Lerp( startPos, cameraTarget.position, _lerpSpeed );
			
		// 	// set rotation
		// 	cameraTarget.LookAt( cameraFocus );
		// }
	}	
}