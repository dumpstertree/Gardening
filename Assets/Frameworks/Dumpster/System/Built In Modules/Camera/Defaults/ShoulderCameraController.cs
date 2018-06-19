using UnityEngine;
using Dumpster.Core.BuiltInModules;

namespace Dumpster.Core.BuiltInModules {

	public class ShoulderCameraController : CameraController {

		[SerializeField] private Transform _followObject;
		[SerializeField] private Vector3 _targetPosLeft;
		[SerializeField] private Vector3 _targetPosRight;

		public float MovementHorizontal { get; set; }
		public float MovementVertical { get; set; }
		public float CameraHorizontal{ get; set; }
		public float CameraVertical{ get; set; }

		[SerializeField] private float _horizontalSensitivity= 0.1f;
		[SerializeField] private float _verticalSensitivity= 0.05f;

		private float _horizontalRot = 0f;
		private float _verticalRot = 0f;

		public override void Control( Transform cameraTarget, Transform cameraFocus ) {

			MoveCameraTarget( cameraTarget, cameraFocus );
		}

		private void MoveCameraTarget ( Transform cameraTarget, Transform cameraFocus ) {
			
			var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (MovementHorizontal+1) /2 );
			var worldPos = _followObject.TransformPoint( targetPosLocal );

			_horizontalRot += CameraHorizontal * _horizontalSensitivity;
			_verticalRot -=  CameraVertical * _verticalSensitivity;
			_verticalRot = Mathf.Clamp( _verticalRot, -0.5f, 0.5f);

			cameraTarget.position = Vector3.Lerp( cameraTarget.position, worldPos, 0.2f );
			cameraTarget.rotation = Quaternion.AxisAngle( Vector3.up, _horizontalRot ) * Quaternion.AxisAngle( Vector3.right, _verticalRot );
		}
		private void OnDrawGizmos(){
				
			var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (MovementHorizontal+1) /2 );
			var worldPos = transform.TransformPoint( targetPosLocal );

			Gizmos.DrawWireSphere( worldPos, 0.25f );
		}
	}
}