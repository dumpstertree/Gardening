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

		public float CameraHorizontal{ get; set; }
		public float CameraVertical{ get; set; }

		public override void Control( Transform cameraTarget, Transform cameraFocus ) {

			MoveCameraTarget( cameraTarget, cameraFocus );
		}

		private void MoveCameraTarget ( Transform cameraTarget, Transform cameraFocus ) {
				
			_horizontalRot += CameraHorizontal * _horizontalSensitivity;
			_horizontalRot = _horizontalRot * _lerpSpeed;

			_verticalRot = Mathf.Clamp( _verticalRot + (CameraVertical * _verticalSensitivity), -1f, 1f );

			var startPos = cameraTarget.position;

			var p1 = _followObject.position;
			var p2 = cameraTarget.position;
			var angle = Mathf.Atan2( p2.x-p1.x, p2.z-p1.z );
			var yRot = Mathf.Lerp( angle, angle + _horizontalRot, _lerpSpeed ) * Mathf.Rad2Deg;
			var zRot = Mathf.Lerp( transform.localRotation.z, _verticalRot, _lerpSpeed ) * Mathf.Rad2Deg;
			
			var targetRotation = Quaternion.Euler( new Vector3( _followObject.transform.rotation.eulerAngles.x, yRot, zRot ) );

			// set xz
			cameraTarget.rotation = targetRotation;
			cameraTarget.position = _followObject.position + cameraTarget.forward * _distanceFromTarget;
			
			// set y
			cameraTarget.transform.position += new Vector3( 0f, Mathf.Lerp( _minHeight, _maxHeight, _verticalRot ), 0f);

			// lerp the value to the new one 
			cameraTarget.transform.position = Vector3.Lerp( startPos, cameraTarget.position, _lerpSpeed );
			
			// set rotation
			cameraTarget.LookAt( cameraFocus );
		}
	}	
}