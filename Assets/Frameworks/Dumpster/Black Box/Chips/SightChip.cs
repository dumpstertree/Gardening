using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Core.Life {
	
	public class SightChip : MonoBehaviour {

		[SerializeField] private Transform _eyeProjector;
		[SerializeField] private LayerMask _lookingFor;

		private const float LOOK_DISTANCE = 10f;

		public List<Eden.Life.Brain.BlackBoxBrain> LookForTargets () {
		
			RaycastHit hit;

			if ( UnityEngine.Physics.Raycast( _eyeProjector.position, _eyeProjector.forward, out hit, LOOK_DISTANCE, _lookingFor )) {
					
				var c = hit.collider.GetComponents<Eden.Life.Brain.BlackBoxBrain>();	
				if ( c != null ) {
					return new List<Eden.Life.Brain.BlackBoxBrain>( c );	
				}
			}

			return new List<Eden.Life.Brain.BlackBoxBrain>();
  		}
  		
  		private void OnDrawGizmos () {

  			Debug.DrawRay( _eyeProjector.position, _eyeProjector.forward * LOOK_DISTANCE );
  		}
	}
}