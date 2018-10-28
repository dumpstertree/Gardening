using Eden.Properties;
using Eden.Model.Interactable;
using System.Linq;
using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;
using Dumpster.Core.BuiltInModules;
using Eden.Modules;

namespace Eden.Interactors.Ranged {

	public class Bullet : MonoBehaviour {


		[SerializeField] protected LayerMask _layermask;
		[SerializeField] protected GameObject _casingPrefab;
				
		private const float CASING_KILL_TIME = 15.0f;

		protected ICanUseRangedWeapon _user;
		protected Hit _hitData;
		protected float _bulletSize;
		protected float _bulletSpeed;
		protected float _spread;

		
		// ******************* Public **************************

		public void SetBullet ( ICanUseRangedWeapon user, Hit hitData, float bulletSize, float bulletSpeed, float spread ) {
			

			// set all protected variables
			_user = user;
			_hitData = hitData;
			_bulletSize = bulletSize;
			_bulletSpeed = bulletSpeed;
			_spread = spread;


			// set start position
			SetStartPosition( user.GetSpawnLocation() );
			
			
			// get initial direction
			var forward = GetForward( user.GetSpawnLocation(), user.GetLookingDirection() );


			// set forward taking into account spread
			forward = AddSpread( forward, spread );


			// find targetable
			var targetable = Game.GetModule<Targeting>()?.GetTargetable( UnityEngine.Camera.main.transform.position, user.GetLookingDirection(), 3f );
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

			Game.GetModule<Async>()?.WaitForSeconds( CASING_KILL_TIME, () => { Destroy( inst ); } );
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


		// ******************* Private ****************************

		private void SetStartPosition ( Vector3 spawnLocation ) {

			transform.position = spawnLocation;
		}
		private Vector3 GetForward ( Vector3 spawnLocation, Vector3 forward ) {

			var targetPoint = Vector3.zero;

			RaycastHit hit;

			if ( Physics.Raycast( UnityEngine.Camera.main.transform.position, forward, out hit, Mathf.Infinity, _layermask )) {
				targetPoint = hit.point;
			} else {
				targetPoint = forward * 9999f;
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

		private void SetSize ( float bulletSize ) {

			transform.localScale = new Vector3( 
				transform.localScale.x * bulletSize, 
				transform.localScale.y * bulletSize, 
				transform.localScale.z * bulletSize 
			);
		}
		private void PlayEffects () {

			Game.GetModule<Effects>()?.Shake( transform.position, Dumpster.BuiltInModules.ShakePower.Miniscule, Dumpster.BuiltInModules.DecayRate.Quick );
		}
	}
}