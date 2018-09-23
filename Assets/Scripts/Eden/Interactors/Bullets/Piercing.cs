using UnityEngine;

namespace Eden.Interactors.Bullets {
	
	public class Piercing : Bullet {

		private void Update () {

			MoveForward();

			var collision = LookForCollision( _user );
			if ( collision != null ) {
					
				Collide( collision, false );
			}
		}
	}
}