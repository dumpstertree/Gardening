using UnityEngine;

namespace Gun {
	
	public class Reciever : MonoBehaviour {

		public delegate void OnRecieveEvent();
		public OnRecieveEvent OnRecieve;

		public void Recieve () {
			
			if ( OnRecieve != null ) {
			//	print( transform.parent.parent.name );
				OnRecieve();
			}
		}

		public int X {
			get{ return GetComponentInParent<GunCraftingPanel>().GetX( transform.position ); }
		}
		public int Y {
			get{ return GetComponentInParent<GunCraftingPanel>().GetY( transform.position ); }
		}

		public Collider.Connection Connection;

		private const float VISUAL_SIZE = 30;

		private void OnDrawGizmos () {

			Gizmos.color = Color.red;
			Gizmos.DrawLine( transform.position, transform.position + (transform.up * 100) );
			
			switch( Connection ) {

				case Collider.Connection.Circle: Gizmos.DrawWireSphere( transform.position, VISUAL_SIZE/2 ); break;
				case Collider.Connection.Square: Gizmos.DrawWireCube( transform.position, new Vector3(VISUAL_SIZE,VISUAL_SIZE,VISUAL_SIZE) ); break;
			}
		}
	}
}