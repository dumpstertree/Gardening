using UnityEngine;

namespace Eden.Interactors.Ranged {
	
	public class Standard : Bullet {

		private void Update () {

			MoveForward();

			var collision = LookForCollision( _ranged );
			if ( collision != null ) {
					
				Collide( collision );
			}
		}
	}
}