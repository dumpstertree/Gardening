using UnityEngine;

public class Projector : MonoBehaviour {

	private const float PROJECTION_LENGTH = 1;

	public void Project ( BuiltStats stats ) {

		RaycastHit hit;
		if( Physics.Raycast( transform.position, transform.up, out hit, PROJECTION_LENGTH ) ){

			var reciever = hit.transform.GetComponent<Reciever>();
			reciever.Recieve( stats );
		}
	}

	private void OnDrawGizmos () {

		Gizmos.color = Color.red;
		Gizmos.DrawRay( transform.position, transform.up * PROJECTION_LENGTH );
		Gizmos.DrawWireCube( transform.position, new Vector3( 0.5f, 0.5f, 0.5f) );
	}
}
