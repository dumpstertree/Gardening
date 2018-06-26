using UnityEngine;

public class Projector : MonoBehaviour {

	private const float PROJECTION_LENGTH = 1;

	public void Project () {

		RaycastHit hit;
		if( Physics.Raycast( transform.position, -transform.right, out hit, PROJECTION_LENGTH ) ){
			
			var reciever = hit.transform.GetComponent<Reciever>();
			reciever.Recieve();
		}
	}

	private void OnDrawGizmos () {

		Gizmos.color = Color.red;
		Gizmos.DrawRay( transform.position, -transform.right * PROJECTION_LENGTH );
		Gizmos.DrawWireCube( transform.position, new Vector3( 0.5f, 0.5f, 0.5f) );
	}
}
