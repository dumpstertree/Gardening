using System.Linq;
using UnityEngine;
using Dumpster.Core;
using Dumpster.BuiltInModules;
using Dumpster.Core.BuiltInModules;
using Dumpster.Characteristics;
using Eden.Modules;
using Eden.Characteristics;

namespace Eden.Interactors.Ranged {

	public class Bullet : MonoBehaviour {


		[SerializeField] protected LayerMask _layermask;
		[SerializeField] protected GameObject _casingPrefab;
		[SerializeField] protected GameObject _bulletCollisionParticlePrefab;		

		private const float CASING_KILL_TIME = 15.0f;

		protected Actor _user;
		protected CanUseRangedWeapons _ranged;
		protected float _bulletSize;
		protected float _bulletSpeed;
		protected float _spread;

		
		// ******************* Public **************************

		public void SetBullet ( CanUseRangedWeapons ranged, float bulletSize, float bulletSpeed, float spread ) {
			

			// set all protected variables
			_ranged = ranged;
			_user = ranged.Actor;
			_bulletSize = bulletSize;
			_bulletSpeed = bulletSpeed;
			_spread = spread;


			// set start position
			SetStartPosition( _ranged.GetSpawnLocation() );
			
			
			// get initial direction
			var forward = GetForward( _ranged.GetSpawnLocation(), _ranged.GetLookingDirection() );


			// set forward taking into account spread
			forward = AddSpread( forward, spread );

			var target = _user.GetCharacteristic<Targeter>()?.GetBestTarget();
			if ( target != null ) {
				forward = target.transform.position - _ranged.GetSpawnLocation();
			}

		
			// set forward
			transform.forward = forward;


			// set bullet size
			// SetSize( bulletSize );


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
		protected void Collide ( RaycastHit hit, bool destroyThis = true ) {

			var actor = hit.collider.GetComponentInChildren<Actor>();
			
			if ( actor != null ) {
				actor.GetCharacteristic<Dumpster.Characteristics.Damageable>()?.Damage();
			}

			if ( destroyThis ) {
				Destroy( gameObject );
			}

			CreateCollisionParticle( hit );
		}
		protected RaycastHit? LookForCollision ( CanUseRangedWeapons user ) {

			var distance = _bulletSpeed * Time.deltaTime;

			RaycastHit[] hits;
	        hits = Physics.RaycastAll( transform.position, transform.forward, distance ).OrderBy( h => h.distance ).ToArray();

	        foreach( RaycastHit hit in hits ) {
				
				if ( _layermask != (_layermask | (1 << hit.transform.gameObject.layer )) ) {
					continue;
				}

	            if ( user != null && user.GetForbiddenColliders().Contains( hit.collider ) ) {
	            	continue;
	            }

	            return hit;
	        }

	        return null;
		}
		private void CreateCollisionParticle ( RaycastHit hit ) {
			
			var inst = Instantiate( _bulletCollisionParticlePrefab );
			inst.transform.position = hit.point;
			inst.transform.forward = hit.normal;
		}


		// ******************* Private ****************************

		private void SetStartPosition ( Vector3 spawnLocation ) {

			transform.position = spawnLocation;
		}
		private Vector3 GetForward ( Vector3 spawnLocation, Vector3 forward ) {

			var targetPoint = Vector3.zero;

			RaycastHit hit;

			if ( Physics.Raycast( spawnLocation, forward, out hit, Mathf.Infinity, _layermask )) {
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