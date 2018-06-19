using UnityEngine;

namespace Dumpster.Triggers {

	[RequireComponent( typeof( BoxCollider ) )]
	public class BoxZone : MonoBehaviour {

		public delegate void TriggerZoneEnterEvent ();
		public TriggerZoneEnterEvent OnTriggerZoneEnter;

		public delegate void TriggerZoneExitEvent ();
		public TriggerZoneExitEvent OnTriggerZoneExit;

		private void OnTriggerEnter ( Collider collider ) {

			if ( collider.gameObject.GetComponent<Eden.Life.BlackBox>() ) {
				FireOnTriggerZoneEnter ();
			}
		}
		private void OnTriggerExit ( Collider collider ) {
			
			if ( collider.gameObject.GetComponent<Eden.Life.BlackBox>() ) {
				FireOnTriggerZoneExit ();
			}
		}
		private void FireOnTriggerZoneEnter () {
			if( OnTriggerZoneEnter != null ) {
				OnTriggerZoneEnter ();
			}
		}
		private void FireOnTriggerZoneExit () {
			if( OnTriggerZoneExit != null ) {
				OnTriggerZoneExit ();
			}
		}
		private void OnDrawGizmos () {

			GetComponent<BoxCollider>().isTrigger = true;
		}
	}
}