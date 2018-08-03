using UnityEngine;
using System.Collections.Generic;

namespace Eden.UI.Elements.Building {

	public class Projector : MonoBehaviour {

		
		// ***************** Public ********************
		
		public void Project ( List<Eden.Model.Building.Parts.Gun> parts ) {

			RaycastHit hit;
			if( Physics.Raycast( transform.position, transform.up, out hit, PROJECTION_LENGTH ) ){

				var reciever = hit.transform.GetComponent<Reciever>();
				reciever.Recieve( parts );
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