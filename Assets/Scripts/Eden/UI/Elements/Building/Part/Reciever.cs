using UnityEngine;
using System.Collections.Generic;

namespace Eden.UI.Elements.Building {

	public class Reciever : MonoBehaviour {


		// ***************** Public ********************
		
		public delegate void RecieveEvent( List<Eden.Model.Building.Parts.Gun> parts );
		public RecieveEvent OnRecieve;

		public void Recieve ( List<Eden.Model.Building.Parts.Gun> parts ) {

			TryToRecieve( parts );
		}


		// ***************** Private ********************

		private const float PROJECTION_LENGTH = 1;

		private void TryToRecieve ( List<Eden.Model.Building.Parts.Gun> parts ) {

			RaycastHit hit;
			if( Physics.Raycast( transform.position, transform.up, out hit, PROJECTION_LENGTH ) ){
				
				if( hit.transform.GetComponent<Projector>() != null ) {
					FireRecieveEvent ( parts );
				}
			}
		}
		private void FireRecieveEvent ( List<Eden.Model.Building.Parts.Gun> parts ) {

			if ( OnRecieve != null ) {
				OnRecieve( parts );
			}
		}
		private void OnDrawGizmos () {

			Gizmos.color = Color.blue;
			Gizmos.DrawRay( transform.position, transform.up * PROJECTION_LENGTH );
			Gizmos.DrawWireCube( transform.position, new Vector3( 0.5f, 0.5f, 0.5f) );
		}
	}
}