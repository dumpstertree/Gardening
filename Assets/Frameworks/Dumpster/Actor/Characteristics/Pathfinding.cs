using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Dumpster.Characteristics {
	
	public class Pathfinding : Dumpster.Core.Characteristic {


		// ****************** Public *****************
	
		public bool HasDestination {
			get{ return _hasDestination; }
		}
		public float MovementSpeed {
			get{ return _agent.speed; }
			set{ _agent.speed = value; }
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

			_hasDestination = false;
		}

		// ****************** Protected *****************

		protected override void OnActorUpdate() {
			
			if ( !_hasDestination ) {
				return;
			}

			var dist = Vector3.Distance( _agent.transform.position, _destination );
			if ( dist < _reachDeadZone ) {
				_hasDestination = false;
			}
		}
		

		// ****************** Private *****************
		
		[SerializeField] private NavMeshAgent _agent;
		[SerializeField] private float _reachDeadZone;

		private Vector3 _destination;
		private bool _hasDestination;
	}
}