using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour {

	[SerializeField] private float _speed;
	void OnDrawGizmos () {
		transform.RotateAround( transform.parent.position, Vector3.up, _speed );
	}
}
