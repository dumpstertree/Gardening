using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Core {

	public class Orbit : MonoBehaviour {

		[SerializeField] private float _speed;
		[SerializeField] private Vector3 _axis;
		[SerializeField] private Transform _center;

		private void Update () {

			transform.RotateAround( _center.position, _axis, _speed );
		}
	}
}