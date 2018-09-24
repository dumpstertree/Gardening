using UnityEngine;

namespace Eden.Interactors.Bullets {
	
	public class Standard : Bullet {

		private void Update () {

			MoveForward();

			var collision = LookForCollision( _user );
			if ( collision != null ) {
					
				Collide( collision );
			}
		}
	}
}