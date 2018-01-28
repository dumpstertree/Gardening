using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour {

  [SerializeField] private Transform _eyeProjector;
  [SerializeField] private LayerMask _lookingFor;

	private const float LOOK_DISTANCE = 10f;
	public List<Creature> LookForTargets () {

		RaycastHit hit;

      Debug.DrawRay( _eyeProjector.position, _eyeProjector.forward * LOOK_DISTANCE );
      
      if ( Physics.Raycast( _eyeProjector.position, _eyeProjector.forward, out hit, LOOK_DISTANCE, _lookingFor )) {
 			
 			    var c = hit.collider.GetComponents<Creature>();
          if ( c != null ) {
              return new List<Creature>( c );
            }
        }

      return new List<Creature>();
	}
}
