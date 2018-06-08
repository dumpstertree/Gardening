using UnityEngine;

namespace Dumpster.Core.BuiltInModules {

	public class StaticCameraController : CameraController {

		[Header( "Trigger Zones" )]
		[SerializeField] private Dumpster.Triggers.BoxZone _enterTriggerZone;
		[SerializeField] private Dumpster.Triggers.BoxZone _exitTriggerZone;

		[Header("Camera Target")]
		[SerializeField] private Transform _cameraTarget;
		[SerializeField] private bool _lookAtFocus;

		[Header("Movement Speed")]
		[SerializeField] private float _lerpSpeed = 0.5f;

		
		public override void Control( Transform cameraInstance, Transform focus ) {
			
			if (_cameraTarget != null) {
				
				// position
				cameraInstance.transform.position = Vector3.Lerp( 
					cameraInstance.transform.position,
					_cameraTarget.position, 
					_lerpSpeed );


				// rotation
				if ( _lookAtFocus ) {
					cameraInstance.LookAt( focus );
				} else {
					cameraInstance.transform.rotation = Quaternion.Slerp( cameraInstance.transform.rotation, _cameraTarget.rotation, _lerpSpeed );
				}
			}
		}

		private void Awake () {

			if ( _enterTriggerZone ) {
				_enterTriggerZone.OnTriggerZoneEnter += EnterRange;
			}

			if ( _exitTriggerZone ) {
				_exitTriggerZone.OnTriggerZoneExit += ExitRange;
			}
		}
		private void EnterRange () {

			FireRequestEvent ();
		}
		private void ExitRange () {

			FireRelinquishEvent ();
		}
	}
}