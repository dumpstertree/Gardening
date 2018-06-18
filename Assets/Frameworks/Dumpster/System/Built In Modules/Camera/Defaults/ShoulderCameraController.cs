using UnityEngine;
using Dumpster.Core.BuiltInModules;

namespace Dumpster.Core.BuiltInModules {

	public class ShoulderCameraController : CameraController {

		[SerializeField] private Transform _followObject;
		[SerializeField] private Vector3 _targetPosLeft;
		[SerializeField] private Vector3 _targetPosRight;
		[SerializeField] private float _horizontal;
		[SerializeField] private float _vertical;

		private Vector3 _mouseDown;
		private Quaternion _startRot;
		private float xRot;

		public override void Control( Transform cameraTarget, Transform cameraFocus ) {

			MoveCameraTarget( cameraTarget, cameraFocus );
		}

		private void MoveCameraTarget ( Transform cameraTarget, Transform cameraFocus ) {
			
			var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (_horizontal+1) /2 );
			var worldPos = _followObject.TransformPoint( targetPosLocal );

			cameraTarget.position = Vector3.Lerp( cameraTarget.position, worldPos, 0.2f );
			cameraTarget.LookAt( cameraFocus.forward * 1000 );
		}
		private void OnDrawGizmos(){
				
			var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (_horizontal+1) /2 );
			var worldPos = transform.TransformPoint( targetPosLocal );

			Gizmos.DrawWireSphere( worldPos, 0.25f );
		}
	}
}