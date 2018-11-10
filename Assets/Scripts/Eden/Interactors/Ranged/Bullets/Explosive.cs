using UnityEngine;

namespace Eden.Interactors.Ranged {
	
	public class Explosive : Bullet {

		[SerializeField] private Explosion _explosivePrefab;

		private void Update () {

			MoveForward();

			var collision = LookForCollision( _ranged );
			if ( collision != null ) {
				
				Explode();
				Collide( collision.Value );
			}
		}
		private void Explode () {

			var explosion = Instantiate( _explosivePrefab );
			explosion.transform.position = transform.position;
		}
	}
}