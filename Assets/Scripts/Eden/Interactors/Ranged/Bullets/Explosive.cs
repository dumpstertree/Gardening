using Eden.Model.Interactable;
using UnityEngine;

namespace Eden.Interactors.Ranged {
	
	public class Explosive : Bullet {

		[SerializeField] private Explosion _explosivePrefab;

		private void Update () {

			MoveForward();

			var collision = LookForCollision( _user );
			if ( collision != null ) {
				
				Explode();
				Collide( collision );
			}
		}
		private void Explode () {

			var explosion = Instantiate( _explosivePrefab );
			explosion.transform.position = transform.position;

			var hitData = new Hit( null, 1 );

			// explosion.Set( _user, hitData );
		}
	}
}