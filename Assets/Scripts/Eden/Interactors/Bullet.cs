using Eden.Properties;
using System.Linq;
using UnityEngine;

namespace Eden.Interactors {

	public interface ICanUseRangedWeapon {

		Vector3 GetSpawnLocation ();
		Vector3 GetLookingDirection ();
		Collider[] GetForbiddenColliders ();
		float GetAimAssistRange  ();
	}

	public class Bullet : MonoBehaviour {


		[SerializeField] protected LayerMask _layermask;
		[SerializeField] protected GameObject _casingPrefab;
				
		private const float CASING_KILL_TIME = 15.0f;

		protected ICanUseRangedWeapon _user;
		protected HitData _hitData;
		protected float _bulletSize;
		protected float _bulletSpeed;
		protected float _spread;

		
		// ******************* Public **************************

		public void SetBullet ( ICanUseRangedWeapon user, HitData hitData, float bulletSize, float bulletSpeed, float spread ) {
			
		
			// set all protected variables
			_user = user;
			_hitData = hitData;
			_bulletSize = bulletSize;
			_bulletSpeed = bulletSpeed;
			_spread = spread;


			// set start position
			SetStartPosition( user.GetSpawnLocation() );
			
			
			// get initial direction
			var forward = GetForward( user.GetSpawnLocation() );


			// set forward taking into account spread
			forward = AddSpread( forward, spread );


			// find targetable
			var targetable = GetTargetable( user.GetSpawnLocation(), forward, user.GetAimAssistRange() );
			if ( targetable != null ) {
				forward = targetable.transform.position - user.GetSpawnLocation();
			}

		
			// set forward
			transform.forward = forward;


			// set bullet size
			SetSize( bulletSize );


			// create a bullet casing
			CreateCasing ();


			// play effects through effects system
			PlayEffects();
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
				
				interactable.HitDelegate.Hit( _hitData );
			}

			if ( destroyThis ) {
				Destroy( gameObject );
			}
		}
		protected Collider LookForCollision ( ICanUseRangedWeapon user ) {

			var distance = _bulletSpeed * Time.deltaTime;

			RaycastHit[] hits;
	        hits = Physics.RaycastAll( transform.position, transform.forward, distance ).OrderBy( h => h.distance ).ToArray();

	        foreach( RaycastHit hit in hits ) {

	            if ( user.GetForbiddenColliders().Contains( hit.collider ) ) {
	            	continue;
	            }

	            return hit.collider;
	        }

	        return null;
		}
		protected Targetable FindClosestTarget () {

			return EdensGarden.Instance.Targeting.GetClosestTargetableToPoint( transform.position,  new Vector2( Screen.width/2f, Screen.height/2f ) );
		}


		// ******************* Private ****************************
		
		private void OnDrawGizmos () {

			Debug.DrawRay( transform.position, transform.forward * _bulletSpeed * Time.deltaTime );

		}
		private void SetStartPosition ( Vector3 spawnLocation ) {

			transform.position = spawnLocation;
		}
		private Vector3 GetForward ( Vector3 spawnLocation ) { // this will only work for the player, create an interface and ask the interface for the forward

			var targetPoint = Vector3.zero;

			RaycastHit hit;

			if ( Physics.Raycast( Camera.main.transform.position, Camera.main.transform.forward, out hit, Mathf.Infinity, _layermask )) {
				targetPoint = hit.point;
			} else {
				targetPoint = Camera.main.transform.forward * 9999f;
			}

			return targetPoint - spawnLocation;
		}
		private Vector3 AddSpread ( Vector3 forward, float spread ) {

			var ySpread = Random.Range( -spread, spread );
			var xSpread = Random.Range( -spread, spread );

			var newFor = Quaternion.AngleAxis( ySpread, Vector3.up ) * 
						 Quaternion.AngleAxis( xSpread, Vector3.Cross( forward, Vector3.up ) ) *
						 forward;

			return newFor;
		}
		private Targetable GetTargetable ( Vector3 spawnLocation, Vector3 forwardWithSpread, float aimAssistRange ) {

			var targetable = FindClosestTarget (); // this does not work for anyone besides the player
			var directionToTargetable = targetable.transform.position - spawnLocation;
			var angle = Vector3.Angle( forwardWithSpread, directionToTargetable );

			return ( angle < aimAssistRange ) ? targetable : null;
		}



		private void SetSize ( float bulletSize ) {

			transform.localScale = new Vector3( 
				transform.localScale.x * bulletSize, 
				transform.localScale.y * bulletSize, 
				transform.localScale.z * bulletSize 
			);
		}
		private void PlayEffects () {

			EdensGarden.Instance.Effects.Shake( transform.position, Dumpster.Core.BuiltInModules.Effects.ShakePower.Miniscule, Dumpster.Core.BuiltInModules.Effects.DecayRate.Quick );
		}
	}
}