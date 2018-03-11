using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAnimator : MonoBehaviour {

	[SerializeField] private Animator _animator;
	[SerializeField] private Collider _collider;
	[SerializeField] private float _distanceFromGround = 0f;

	private void Update () {
		
		var lowestPoint = _collider.transform.position;

	    Debug.DrawRay( lowestPoint, Vector3.down * 100 );

	    RaycastHit hit;
		if ( Physics.Raycast( lowestPoint, Vector3.down, out hit, 100 )) {
			
			UpdateDistanceFromGround( hit.distance );
		}
	}

	private void UpdateDistanceFromGround ( float newDistance ) {


		if ( _distanceFromGround - newDistance > 0.1f ) {
			//print( "Going Down" );
		}

		if ( _distanceFromGround - newDistance < 0.1f ) {
			//print( "Going Up" );
		}


		_distanceFromGround = newDistance;
	}
}
