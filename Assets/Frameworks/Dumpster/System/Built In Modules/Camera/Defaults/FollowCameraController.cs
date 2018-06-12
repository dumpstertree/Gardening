using UnityEngine;

namespace Dumpster.Core.BuiltInModules {

	public class FollowCameraController : CameraController {

		[SerializeField] private float _lerpSpeed = 0.5f;
		[SerializeField] private Transform _followObject;
		[SerializeField] private float _distanceFromTarget = 4.0f;
		[SerializeField] private float _height = 1.0f;

		public override void Control( Transform cameraTarget, Transform cameraFocus ) {
			
			_followObject = FindObjectOfType<Player>().transform;

			MoveCameraTarget( cameraTarget, cameraFocus );
		}

		private void MoveCameraTarget ( Transform cameraTarget, Transform cameraFocus ) {
			
			var startPos = cameraTarget.position;

			var p1 = _followObject.position;
			var p2 = cameraTarget.position;
			var angle = Mathf.Atan2( p2.x-p1.x, p2.z-p1.z ) * Mathf.Rad2Deg;

			// set xz
			cameraTarget.rotation = Quaternion.Euler( new Vector3( _followObject.transform.rotation.eulerAngles.x, angle, _followObject.transform.rotation.eulerAngles.z ) ); // this rotation is currently only being set to move the object
			cameraTarget.position = _followObject.position + cameraTarget.forward * _distanceFromTarget;
			
			// set y
			cameraTarget.transform.position += new Vector3( 0f, _height, 0f);

			// lerp the value to the new one 
			cameraTarget.transform.position = Vector3.Lerp( startPos, cameraTarget.position, _lerpSpeed );
			
			// set rotation
			cameraTarget.LookAt( cameraFocus );
		}
	}	
}