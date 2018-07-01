using UnityEngine;

namespace Eden.UI.Elements.Building {

	public class Projector : MonoBehaviour {

		
		// ***************** Public ********************
		
		public void Project ( Eden.Model.Building.Stats.Gun stats ) {

			RaycastHit hit;
			if( Physics.Raycast( transform.position, transform.up, out hit, PROJECTION_LENGTH ) ){

				var reciever = hit.transform.GetComponent<Reciever>();
				reciever.Recieve( stats );
			}
		}


		// ***************** Private ********************

		private const float PROJECTION_LENGTH = 1;

		private void OnDrawGizmos () {

			Gizmos.color = Color.red;
			Gizmos.DrawRay( transform.position, transform.up * PROJECTION_LENGTH );
			Gizmos.DrawWireCube( transform.position, new Vector3( 0.5f, 0.5f, 0.5f) );
		}
	}
}