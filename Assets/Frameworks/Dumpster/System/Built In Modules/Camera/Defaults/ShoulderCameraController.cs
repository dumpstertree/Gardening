using UnityEngine;
using Dumpster.Core.BuiltInModules;

namespace Dumpster.Core.BuiltInModules {

	public class ShoulderCameraController : CameraController {


		// *********************** Public ***********************

		public float MovementHorizontal { get; set; }
		public float MovementVertical { get; set; }
		public float CameraHorizontal{ get; set; }
		public float CameraVertical{ get; set; }

		public Vector3 Forward { get; set; }

		public Vector2 ReticlePosition { get; set; }

		public override void Control( Transform cameraTarget, Transform cameraFocus ) {

			MoveCameraTarget( cameraTarget, cameraFocus );
			// CenterOnReticle( cameraTarget );
		}


		// *********************** Private ***********************

		[Header( "Settings" )]
		[SerializeField] private Transform _followObject;
		[SerializeField] private float _lerpSpeed = 0.5f;

		[Header( "Positions" )]
		[SerializeField] private Vector3 _targetPosLeft;
		[SerializeField] private Vector3 _targetPosRight;

		[Header( "Sensitivity" )]
		[SerializeField] private float _horizontalSensitivity= 0.1f;
		[SerializeField] private float _verticalSensitivity= 0.05f;

		[Header( "Aim Assist" )]
		[SerializeField] private float _reticlePullForce = 0.1f;


		private void MoveCameraTarget( Transform cameraTarget, Transform cameraFocus ) {
			
			var targetPosLocal = Vector3.Lerp( _targetPosLeft, _targetPosRight, (MovementHorizontal+1) /2 );
			var worldPos = _followObject.TransformPoint( targetPosLocal );

			cameraTarget.position = Vector3.Lerp( cameraTarget.position, worldPos, _lerpSpeed );
			cameraTarget.forward = Forward;
		}
		private void CenterOnReticle( Transform cameraTarget ) {

			var offsetFromCenter = new Vector2( Screen.width/2, Screen.height/2 ) - ReticlePosition;
			cameraTarget.rotation = Quaternion.AngleAxis( -offsetFromCenter.x * _reticlePullForce, Vector3.up ) * Quaternion.AngleAxis( offsetFromCenter.y * _reticlePullForce, cameraTarget.right ) * cameraTarget.rotation ; 
		}
		public override void WillGainControl () {
			UnityEngine.Camera.main.fieldOfView = 45;
		}
		public override void WillLoseControl () {
			UnityEngine.Camera.main.fieldOfView = 60;
		}
	}
}