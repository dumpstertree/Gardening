using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eden.Model;
using Dumpster.Core;
using System.Linq;

namespace Eden.Characteristics {
	
	[RequireComponent( typeof( Collider ) )]
	public class Interactor : Characteristic {

		public const string ACTOR_ENTER_RANGE = "ACTOR_ENTER_RANGE";
		public const string ACTOR_LEFT_RANGE = "ACTOR_LEFT_RANGE";
		
		public void Use ( Item item ) {

			if ( item != null && !_inAction ) {

				_inAction = true;
				item.Use( _actor, EndAction );
			}
		}

		public Actor GetActor ( Item item ) {
			
			// cleanup
			CleanupActorsInRange ();

			// copy objects
			var actorsInRange = new List<Actor>( _actorsInRange );

			// sort by distance	
			actorsInRange = actorsInRange.OrderBy(x => Vector2.Distance(this.transform.position,x.transform.position) ).ToList();

			// get the best fit
			if ( actorsInRange.Count > 0 ) {
				return actorsInRange[ 0 ];
			}

			return null;	
		}

	
		// *********************** Protected ************************

		protected override void OnInit () {

			_actorsInRange = new List<Actor>();
			GetComponent<Collider>().isTrigger = true;	
		}


		// *********************** Private ************************
		
		private List<Actor> _actorsInRange;
		private bool _inAction;


		private void OnTriggerEnter ( Collider collider ) {
			
			var otherActor = collider.GetComponent<Actor>();
			if ( otherActor != null ) {
			
				_actorsInRange.Add( otherActor );
				_actor.PostNotification( ACTOR_ENTER_RANGE );
			}

		}
		private void OnTriggerExit ( Collider collider ) {

			var otherActor = collider.GetComponent<Actor>();
			if ( otherActor != null && _actorsInRange.Contains( otherActor ) ) {
				
				_actorsInRange.Remove( collider.GetComponent<Actor>() );
				_actor.PostNotification( ACTOR_LEFT_RANGE );
			}
		}
		private void EndAction () {

			_inAction = false;
		}
		private void CleanupActorsInRange () {

			for ( int i=_actorsInRange.Count-1; i>=0; i-- ) {

				if ( _actorsInRange[ i ] == null ) {
					_actorsInRange.RemoveAt( i );
				}
			}
		}
	}
}