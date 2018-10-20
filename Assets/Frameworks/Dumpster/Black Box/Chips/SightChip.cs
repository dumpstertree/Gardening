using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Life.Chips {
	
	public class SightChip : MonoBehaviour {

		[SerializeField] private Transform _eyeProjector;
		[SerializeField] private LayerMask _lookingFor;
		[SerializeField] private float _FOV = 45f;
		[SerializeField] private int _numOfRays = 10;

		private const float LOOK_DISTANCE = 10f;

		public List<Eden.Life.BlackBox> LookForTargets () {
		
			RaycastHit hit;

			var blackBoxes = new List<Eden.Life.BlackBox>();

			for ( int i=0; i<_numOfRays; i++ )  {

				var offset = _FOV/_numOfRays;
				var angle = Quaternion.AngleAxis( -(_FOV / 2f) + (offset * i), _eyeProjector.up );
				var angleVector = angle * _eyeProjector.forward;

				if ( UnityEngine.Physics.Raycast( _eyeProjector.position, angleVector, out hit, LOOK_DISTANCE, _lookingFor )) {
					
					var c = hit.collider.GetComponent<Eden.Life.BlackBox>();	
					if ( c != null && c.IsPowered ) {
						blackBoxes.Add( c );
					}
				}
			}

			return blackBoxes;
  		}
  		
  		private void OnDrawGizmos () {

  			for ( int i=0; i<_numOfRays; i++ )  {

				var offset = _FOV/_numOfRays;
				var angle = Quaternion.AngleAxis( -(_FOV / 2f) + (offset * i), _eyeProjector.up );
				var angleVector = angle * _eyeProjector.forward;
  				
  				Debug.DrawRay( _eyeProjector.position, angleVector * LOOK_DISTANCE );
  			}
  		}
	}
}