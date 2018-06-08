using UnityEngine;

namespace Dumpster.Core.BuiltInModules {

	public abstract class CameraController: MonoBehaviour {


		// ****************** Public *********************

		public delegate void RequestControlEvent ();
		public RequestControlEvent OnRequestControl;
		
		public delegate void RelinquishControlEvent ();
		public RelinquishControlEvent OnRelinquishControl;

		public bool IsDefaultController {
			get { return _isDefaultController; }
		}

		public abstract void Control ( Transform cameraInstance, Transform focus );


		// ****************** Private *********************

		[SerializeField] private bool _isDefaultController;

		protected void FireRequestEvent () {

			if ( OnRequestControl != null ) {
				OnRequestControl();
			}
		}
		protected void FireRelinquishEvent () {
			
			if ( OnRelinquishControl != null ) {
				OnRelinquishControl();
			}
		}
	}
}