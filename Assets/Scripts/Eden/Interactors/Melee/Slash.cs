using System;
using Eden.Interactable;
using Eden.Model.Interactable;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;

namespace Eden.Interactors.Melee {
	
	public class Slash : MonoBehaviour { // To be renamed swing

		public void Set ( ICanUseMeleeWeapon user, Hit hit, int comboNumber, Action endSwing ) {
				

			_alreadyHitColliders = new List<Collider>();
			_comboNumber = comboNumber;
			_user = user;
			_hit = hit;

			Destroy( gameObject, _maxTime );
			
			user.SetSwingActive( true, comboNumber );

			Game.GetModule<Async>()?.WaitForSeconds( _maxTime, () => {
				_user.SetSwingActive( false, comboNumber );
				endSwing ();
			});
		}


		[SerializeField] private float _maxTime;
		[SerializeField] private LayerMask _layermask;
		[SerializeField] private Path _path;

		private int _comboNumber;
		private float _time;

		private Hit _hit;
		private ICanUseMeleeWeapon _user;
		private List<Collider> _alreadyHitColliders;


		private void Update () {
			
			gameObject.transform.position = _user.GetSpawnLocation();
			gameObject.transform.forward = _user.GetLookingDirection();

			_time += Time.deltaTime;
			_user.SetSwingProgress( _time/_maxTime, _comboNumber );

			var collider = LookForCollision ();
			if ( collider != null ) { Collide( collider ); }
		}


		private Collider LookForCollision () {

			var collisionLine = _path.GetLine( _time/_maxTime );

			RaycastHit[] hits;
	        
	        hits = Physics.RaycastAll( collisionLine.Point1, -collisionLine.Direction, collisionLine.Length, _layermask ).OrderBy( h => h.distance ).ToArray();
	        foreach( RaycastHit hit in hits ) {

	            if ( _user.GetForbiddenColliders().Contains( hit.collider ) ) {
	            	continue;
	            }
	           	 if ( _alreadyHitColliders.Contains( hit.collider ) ) {
	            	continue;
	            }

	            return hit.collider;
	        }

	        hits = Physics.RaycastAll( collisionLine.Point2, collisionLine.Direction, collisionLine.Length, _layermask ).OrderBy( h => h.distance ).ToArray();
	        foreach( RaycastHit hit in hits ) {

	            if ( _user.GetForbiddenColliders().Contains( hit.collider ) ) {
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

			var interactable = collider.GetComponent<InteractableObject>();
			if ( interactable && interactable.Hitable ) {
				interactable.HitDelegate.Hit( _hit );
			}
		}
	}
}