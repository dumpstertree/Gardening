using Dumpster.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class Eyes : Dumpster.Core.Characteristic {
		
		[SerializeField] private Transform _projector;
		[SerializeField] private float _maxDistance;
		[SerializeField] private LayerMask _mask;
		[SerializeField] private float _fov = 90;
		[SerializeField] private float _numOfRays = 11;
		[SerializeField] private bool _visualize;

		public bool CanSeeActor ( Actor actor ) {

			RaycastHit hit;
			if( Physics.Raycast( _projector.position, (actor.transform.position - _projector.position).normalized, out hit, _maxDistance, _mask ) ) {

				if ( hit.transform.GetComponent<Actor>() == actor ) {
					return true;
				}
			}

			return false;
		}
		public List<Actor> LookForActors ( System.Action<List<Actor>> sort ) {
			
			var allActorsInSight = new List<Actor>();
			for( int i=0;i<_numOfRays;i++ ) {

				var angle = -(_fov/2f) + ((_fov/_numOfRays ) * i);
				var forward = Quaternion.AngleAxis( angle , Vector3.up ) * _projector.forward;
				
				RaycastHit hit;
				if ( Physics.Raycast( _projector.position, forward, out hit, _maxDistance, _mask ) ) {
					
					var actor = hit.transform.GetComponent<Actor>();
					if ( actor != null ) {
						allActorsInSight.Add( actor );
					}
				}
			}
			
			sort( allActorsInSight );
			
			return allActorsInSight;
		}
		private void OnDrawGizmos () {

			if( !_visualize ){
				return;
			}

			Gizmos.color = Color.green;
			for( int i=0;i<_numOfRays;i++ ) {

				var angle = -(_fov/2f) + ((_fov/_numOfRays ) * i);
				var forward = Quaternion.AngleAxis( angle , Vector3.up ) * _projector.forward;
				Gizmos.DrawRay( _projector.position, forward * _maxDistance );
			}
		}
	}
}