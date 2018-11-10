using UnityEngine;

namespace Dumpster.BuiltInModules {
	
	public class Panel : MonoBehaviour {

		public void Init () {

			OnInit();
		}
		public void Present () {

			OnPresent ();
		}
		public void Dismiss () {

			OnDismiss ();
		}
		public void EnterFocus () {

			OnEnterFocus ();
		}
		public void ExitFocus () {

			OnExitFocus ();
		}

		protected virtual void OnInit () {}
		protected virtual void OnPresent () {
			
			gameObject.SetActive( true );
		}
		protected virtual void OnDismiss () {
			
			gameObject.SetActive( false );
		}
		protected virtual void OnEnterFocus () {}
		protected virtual void OnExitFocus () {}
	}
}