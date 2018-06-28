using UnityEngine;

public class Reciever : MonoBehaviour {

	private const float PROJECTION_LENGTH = 1;

	public delegate void RecieveEvent( BuiltStats stats );
	public RecieveEvent OnRecieve;

	public void Recieve ( BuiltStats stats ) {

		TryToRecieve ( stats );
	}
	private void TryToRecieve ( BuiltStats stats ) {

		RaycastHit hit;
		if( Physics.Raycast( transform.position, transform.up, out hit, PROJECTION_LENGTH ) ){
			
			if( hit.transform.GetComponent<Projector>() != null ) {
				FireRecieveEvent ( stats );
			}
		}
	}
	private void FireRecieveEvent ( BuiltStats stats ) {

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
