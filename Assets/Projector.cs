using UnityEngine;

namespace Gun {

	public class Projector : MonoBehaviour {

		public delegate void OnProjectEvent();
		public OnProjectEvent OnProject;

		public int X {
			get{ return GetComponentInParent<GunCraftingPanel>().GetX( transform.position ); }
		}
		public int Y {
			get{ return GetComponentInParent<GunCraftingPanel>().GetY( transform.position ); }
		}

		// *****************************

		public Collider.Connection Connection;
		
		private const float VISUAL_SIZE = 30;

		// *****************************

		private void OnDrawGizmos () {

			// draw gizmos
			Gizmos.color = Color.blue;
			Gizmos.DrawLine( transform.position, transform.position + (transform.up * 100) );

			switch( Connection ) {

				case Collider.Connection.Circle: Gizmos.DrawWireSphere( transform.position, VISUAL_SIZE/2 ); break;
				case Collider.Connection.Square: Gizmos.DrawWireCube( transform.position, new Vector3(VISUAL_SIZE,VISUAL_SIZE,VISUAL_SIZE) ); break;
			}
		}
	}
}
