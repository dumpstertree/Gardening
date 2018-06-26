using UnityEngine;

public class Reciever : MonoBehaviour {

	private const float PROJECTION_LENGTH = 1;

	public delegate void RecieveEvent();
	public RecieveEvent OnRecieve;

	public void Recieve () {

		TryToRecieve ();
	}
	private void TryToRecieve () {

		RaycastHit hit;
		if( Physics.Raycast( transform.position, -transform.right, out hit, PROJECTION_LENGTH ) ){
			
			if( hit.transform.GetComponent<Projector>() != null ) {
				FireRecieveEvent ();
			}
		}
	}
	private void FireRecieveEvent () {

		if ( OnRecieve != null ) {
			OnRecieve ();
		}
	}
	private void OnDrawGizmos () {

		Gizmos.color = Color.blue;
		Gizmos.DrawRay( transform.position, -transform.right * PROJECTION_LENGTH );
		Gizmos.DrawWireCube( transform.position, new Vector3( 0.5f, 0.5f, 0.5f) );
	}
}
