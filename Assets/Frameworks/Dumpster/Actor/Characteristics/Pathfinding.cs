using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dumpster.Characteristics {
	
	public class Pathfinding : Dumpster.Core.Characteristic {


		// ****************** Public *****************
	
		public bool HasDestination {
			get { return _hasDestination; }
		}
		public float MovementSpeed {
			get { return _agent.speed; }
			set { _agent.speed = value; }
		}
		public Vector3? Destination {
			get { return _destination; }
		}
		
		public float? GetPathLength ( Vector3 destination ) {

			var path = new NavMeshPath();
			var pathExists = _agent.CalculatePath( destination, path );

			if ( pathExists && path.status == NavMeshPathStatus.PathComplete ) {

				var length = 0f;
				for( int i=1; i<path.corners.Length; i++ ) {
					
					var lastPath = path.corners[ i-1 ];
					length += Vector3.Distance( lastPath, path.corners[ i ] );
				}

				return length;
			}

			return null;
		}
		public void GoToDestination ( Vector3 destination ) {
			
			var path = new NavMeshPath();
			var pathExists = _agent.CalculatePath( destination, path );

			if ( pathExists && path.status == NavMeshPathStatus.PathComplete ) {

				_hasDestination = true;
				_destination = destination;

				_agent.SetPath( path );
			}
		}
		public void ClearDestination () {
		
			_agent.SetPath( new NavMeshPath() );
			_hasDestination = false;
		}

		// ****************** Protected *****************

		protected override void OnActorUpdate() {
			
			if ( !_hasDestination ) {
				return;
			}

			var dist = Vector3.Distance( _agent.transform.position, _destination );
			if ( dist < _reachDeadZone ) {
				ClearDestination();
			}
		}
		

		// ****************** Private *****************
		
		[SerializeField] private NavMeshAgent _agent;
		[SerializeField] private float _reachDeadZone;

		private Vector3 _destination;
		private bool _hasDestination;
	}
}