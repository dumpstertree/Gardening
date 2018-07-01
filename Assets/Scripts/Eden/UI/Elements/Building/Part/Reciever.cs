using UnityEngine;

namespace Eden.UI.Elements.Building {

	public class Reciever : MonoBehaviour {


		// ***************** Public ********************
		
		public delegate void RecieveEvent( Eden.Model.Building.Stats.Gun stats );
		public RecieveEvent OnRecieve;

		public void Recieve ( Eden.Model.Building.Stats.Gun stats ) {

			TryToRecieve ( stats );
		}


		// ***************** Private ********************

		private const float PROJECTION_LENGTH = 1;

		private void TryToRecieve ( Eden.Model.Building.Stats.Gun stats ) {

			RaycastHit hit;
			if( Physics.Raycast( transform.position, transform.up, out hit, PROJECTION_LENGTH ) ){
				
				if( hit.transform.GetComponent<Projector>() != null ) {
					FireRecieveEvent ( stats );
				}
			}
		}
		private void FireRecieveEvent ( Eden.Model.Building.Stats.Gun stats ) {

			if ( OnRecieve != null ) {
				OnRecieve( stats );
			}
		}
		private void OnDrawGizmos () {

			Gizmos.color = Color.blue;
			Gizmos.DrawRay( transform.position, transform.up * PROJECTION_LENGTH );
			Gizmos.DrawWireCube( transform.position, new Vector3( 0.5f, 0.5f, 0.5f) );
		}
	}
}