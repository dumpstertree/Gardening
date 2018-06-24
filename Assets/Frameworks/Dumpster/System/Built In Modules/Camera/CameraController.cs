using UnityEngine;

namespace Dumpster.Core.BuiltInModules {

	public abstract class CameraController: MonoBehaviour {


		// ****************** Public *********************

		public Camera.Priority Priority {
			get{ return _priority; }
		}

		public delegate void RequestControlEvent ();
		public RequestControlEvent OnRequestControl;
		
		public delegate void RelinquishControlEvent ();
		public RelinquishControlEvent OnRelinquishControl;

		public bool IsDefaultController {
			get { return _isDefaultController; }
		}

		public abstract void Control ( Transform cameraInstance, Transform focus );
		public virtual void WillGainControl () {}
		public virtual void WillLoseControl () {}

		// ****************** Private *********************

		[SerializeField] private bool _isDefaultController;
		[SerializeField] private Camera.Priority _priority;

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