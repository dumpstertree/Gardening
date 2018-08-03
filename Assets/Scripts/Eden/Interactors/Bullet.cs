using UnityEngine;
using Eden.Properties;
using System.Linq;

namespace Eden.Interactors {

	public class Bullet : MonoBehaviour {

		[SerializeField] protected LayerMask _layermask;
		[SerializeField] protected GameObject _casingPrefab;
		
		private const float CASING_KILL_TIME = 15.0f;
		
		protected RaycastHit _hit;
		protected Eden.Life.BlackBox _user;
		protected HitData _hitData;
		protected float _bulletSize;
		protected float _bulletSpeed;
		protected float _spread;

		
		// ******************* Public **************************

		public void SetBullet ( Eden.Life.BlackBox user, HitData hitData, float bulletSize, float bulletSpeed, float spread ) {
			
			_user = user;
			_hitData = hitData;
			_bulletSize = bulletSize;
			_bulletSpeed = bulletSpeed;
			_spread = spread;

			transform.position = user.ProjectileSpawner.position;
			transform.localScale = new Vector3( transform.localScale.x * bulletSize, transform.localScale.y * bulletSize, transform.localScale.z * bulletSize );

			if ( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, _layermask )) {
				transform.LookAt( _hit.point );
			} else {
				transform.LookAt( Camera.main.transform.forward * 100 );
			}

			transform.forward = Quaternion.Euler( new Vector3( Random.Range( -spread, spread ), Random.Range( -spread, spread ), Random.Range( -spread, spread ) ) ) * transform.forward;

			CreateCasing ();
			EdensGarden.Instance.Effects.Shake( transform.position, Dumpster.Core.BuiltInModules.Effects.ShakePower.Miniscule, Dumpster.Core.BuiltInModules.Effects.DecayRate.Quick );
		}


		// ******************* Protected **************************

		protected void CreateCasing () {
			
			var inst = Instantiate( _casingPrefab );
			inst.transform.position = transform.position;
			inst.transform.rotation = transform.rotation * Quaternion.AngleAxis( 90, Vector3.right );

			var velocity = new Vector3();
			velocity.x = Random.Range( -3, 1);
			velocity.y = Random.Range(  1, 3);
			velocity.z = Random.Range( -3, 3);

			inst.GetComponent<Rigidbody>().velocity = velocity;

			EdensGarden.Instance.Async.WaitForSeconds( CASING_KILL_TIME, () => { Destroy( inst ); } );
		}
		protected void MoveForward () {

			transform.position += transform.forward * ( _bulletSpeed * Time.deltaTime );
		}
		protected void Collide ( Collider collision, bool destroyThis = true ) {

			var interactable = collision.GetComponentInChildren<Eden.Interactable.InteractableObject>();
			
			if ( interactable != null && interactable.Hitable ) {
				
				interactable.HitDelegate.Hit( _user, _hitData );
			}

			if ( destroyThis ) {
				Destroy( gameObject );
			}
		}
		protected Collider LookForCollision () {

			var distance = _bulletSpeed * Time.deltaTime;

			RaycastHit[] hits;
	        hits = Physics.RaycastAll( transform.position, transform.forward, distance ).OrderBy( h => h.distance ).ToArray();

	        foreach( RaycastHit hit in hits ) {

	            var invalid = false;

	           	Transform asda = null;

	            if ( hit.transform == _user.transform ) {
	            	asda = _user.transform;
	            	invalid = true;
	            } else {
		            foreach ( Transform t in _user.transform ) {
		            	if ( hit.transform == t ) { asda = t; invalid = true; break; }
		            }
		        }

	            if ( !invalid ) {

	            	return hit.collider;
	            }
	        }

	        return null;
		}
		protected Targetable FindClosestTarget () {

			return EdensGarden.Instance.Targeting.GetClosestTargetableToPoint( transform.position,  new Vector2( Screen.width/2f, Screen.height/2f ) );
		}

	 	
	 	// private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		        
		//         float u = 1 - t;
		//         float tt = t * t;
		//         float uu = u * u;
		//         float uuu = uu * u;
		//         float ttt = tt * t;
		        
		//         Vector3 p = uuu * p0; 
		//         p += 3 * uu * t * p1; 
		//         p += 3 * u * tt * p2; 
		//         p += ttt * p3; 
		        
		//         return p;
		// 	}
		//}

	}
}

