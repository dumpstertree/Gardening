using UnityEngine;
using System.Linq;
using Dumpster.Core;

namespace Eden.Interactors.Ranged {
	
	public class Bouncy : Bullet {

		private void Update () {

			MoveForward();

			var collision = LookForCollision ( _ranged );
			if ( collision != null ) {
				
				var interactable = collision?.collider.GetComponent<Actor>();

				if ( interactable == null ) {

					transform.forward = Vector3.Reflect( transform.forward, collision.Value.normal );
				}

				Collide( collision.Value, interactable != null );
			}
		}
		private Vector3 GetNormal ( Collider objectToCollideWith ) {

			var distance = _bulletSpeed * Time.deltaTime;

			RaycastHit[] hits;
	        hits = Physics.RaycastAll( transform.position, transform.forward, distance ).OrderBy( h => h.distance ).ToArray();

	        foreach( RaycastHit hit in hits ) {

	            if ( hit.collider != objectToCollideWith ) {
	            	continue;
	            }

	            return hit.normal;
	        }

	        return Vector3.zero;
		}
	}
}