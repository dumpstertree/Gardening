using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class Expolosive : Dumpster.Characteristics.NotificationResponder {

		[Header( "Explosion" )]
		[SerializeField] private GameObject _explosionPrefab;
		[SerializeField] private Transform _position;

		protected override void Respond () {
			
			Explode ();
		}

		private void Explode () {

			var inst = Instantiate( _explosionPrefab );
			inst.transform.position = _position.position;

			var explosion = inst.GetComponent<Explosion>();
		}
	}
}