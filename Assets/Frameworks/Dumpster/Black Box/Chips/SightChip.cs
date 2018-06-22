using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Core.Life {
	
	public class SightChip : MonoBehaviour {

		[SerializeField] private Transform _eyeProjector;
		[SerializeField] private LayerMask _lookingFor;

		private const float LOOK_DISTANCE = 10f;

		public List<Eden.Life.BlackBox> LookForTargets () {
		
			RaycastHit hit;

			if ( UnityEngine.Physics.Raycast( _eyeProjector.position, _eyeProjector.forward, out hit, LOOK_DISTANCE, _lookingFor )) {
					
				var c = hit.collider.GetComponents<Eden.Life.BlackBox>();	
				if ( c != null && c.Length >0 && c[0].IsPowered ) {
					return new List<Eden.Life.BlackBox>( c );	
				}
			}

			return new List<Eden.Life.BlackBox>();
  		}
  		
  		private void OnDrawGizmos () {

  			Debug.DrawRay( _eyeProjector.position, _eyeProjector.forward * LOOK_DISTANCE );
  		}
	}
}