using UnityEngine;

namespace Eden.Interactors.Bullets {
	
	public class Explosive : Bullet {

		[SerializeField] private Explosion _explosivePrefab;

		private void Update () {

			MoveForward();

			var collision = LookForCollision();
			if ( collision != null ) {
				
				Explode();
				Collide( collision );
			}
		}
		private void Explode () {

			var explosion = Instantiate( _explosivePrefab );
			explosion.transform.position = transform.position;

			var hitData = new HitData();
			hitData.Power = 1;

			explosion.Set( _user, hitData );
		}
	}
}