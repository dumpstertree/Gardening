using UnityEngine;
using System.Linq;

namespace Eden.Interactors.Bullets {
	
	public class Bouncy : Bullet {

		private void Update () {

			MoveForward();

			var collision = LookForCollision ( _user );
			if ( collision != null ) {
				
				var interactable = collision.GetComponent<Eden.Interactable.InteractableObject>();

				if ( interactable == null ) {

					transform.forward = Vector3.Reflect( transform.forward, GetNormal( collision ) );
				}

				Collide( collision, interactable != null );
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