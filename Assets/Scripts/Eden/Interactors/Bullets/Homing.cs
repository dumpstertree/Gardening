using UnityEngine;
using Eden.Properties;

namespace Eden.Interactors.Bullets {

	public class Homing : Bullet {

		private const float MAX_HOMING_SPEED = 0.40f;
		private const float MIN_HOMING_SPEED = 0.02f;
		private const float MAX_DISTANCE = 25f;

		private void Update () {

			MoveForward();

			var target = FindClosestTarget ();
			if ( target != null ) { RotateTowardsTarget( target ); }

			var collision = LookForCollision ( _user );
			if ( collision != null ) { Collide( collision ); }
		}

		private void RotateTowardsTarget ( Targetable target ) {

			var distance = Mathf.Clamp( Vector3.Distance(transform.position, target.transform.position ), 0f, MAX_DISTANCE );
			var direction = (target.transform.position - transform.position).normalized;
	      	var lookRotation = Quaternion.LookRotation( direction );
	      	var speed = ( 1 - distance/MAX_DISTANCE) * (MAX_HOMING_SPEED - MIN_HOMING_SPEED) + MIN_HOMING_SPEED;

			transform.rotation = Quaternion.Slerp( transform.rotation, lookRotation, speed );
		}
	}
}