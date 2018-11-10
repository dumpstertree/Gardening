using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Eden.Characteristics;

namespace Eden.Interactors.Melee {
	
	public class Slash : MonoBehaviour { // To be renamed swing

		public void Set ( Actor actor, int comboNumber, Action endSwing ) {
				

			_alreadyHitColliders = new List<Collider>();
			_comboNumber = comboNumber;
			_user = actor;
			_melee = actor.GetCharacteristic<CanUseMeleeItems>( true );

			
			_melee.SetSwingActive( true, comboNumber );

			Game.GetModule<Async>()?.WaitForSeconds( _maxTime, () => {
				_melee.SetSwingActive( false, comboNumber );
				endSwing ();
			});

			Destroy( gameObject, _maxTime );
		}


		[SerializeField] private float _maxTime;
		[SerializeField] private LayerMask _layermask;
		[SerializeField] private Path _path;

		private int _comboNumber;
		private float _time;

		private Actor _user;
		private CanUseMeleeItems _melee;
		private List<Collider> _alreadyHitColliders;


		private void Update () {
			
			if ( _melee == null ) {
				return;
			}

			gameObject.transform.position = _melee.GetSpawnLocation();
			gameObject.transform.forward = _melee.GetLookingDirection();

			_time += Time.deltaTime;
			_melee.SetSwingProgress( _time/_maxTime, _comboNumber );

			var collider = LookForCollision ();
			if ( collider != null ) { Collide( collider ); }
		}


		private Collider LookForCollision () {

			var collisionLine = _path.GetLine( _time/_maxTime );

			RaycastHit[] hits;
	        
	        hits = Physics.RaycastAll( collisionLine.Point1, -collisionLine.Direction, collisionLine.Length, _layermask ).OrderBy( h => h.distance ).ToArray();
	        foreach( RaycastHit hit in hits ) {

	            if ( _melee.GetForbiddenColliders().Contains( hit.collider ) ) {
	            	continue;
	            }
	           	 if ( _alreadyHitColliders.Contains( hit.collider ) ) {
	            	continue;
	            }

	            return hit.collider;
	        }

	        hits = Physics.RaycastAll( collisionLine.Point2, collisionLine.Direction, collisionLine.Length, _layermask ).OrderBy( h => h.distance ).ToArray();
	        foreach( RaycastHit hit in hits ) {

	            if ( _melee.GetForbiddenColliders().Contains( hit.collider ) ) {
	            	continue;
	            }
	           	 if ( _alreadyHitColliders.Contains( hit.collider ) ) {
	            	continue;
	            }

	            return hit.collider;
	        }

	        return null;
		}
		private void Collide ( Collider collider ) {

			_alreadyHitColliders.Add( collider );
			
			var otherActor = collider.GetComponent<Actor>();
			if ( otherActor ) {
				otherActor.GetCharacteristic<Dumpster.Characteristics.Damageable>( true )?.Damage(  );
			}
		}
	}
}