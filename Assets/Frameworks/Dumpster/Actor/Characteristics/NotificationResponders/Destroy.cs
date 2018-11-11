using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {
	
	public class Destroy : Dumpster.Characteristics.NotificationResponder {

		[SerializeField] private float afterSeconds = 0f;

		protected override void Respond () {

			_actor.DestroyActor( afterSeconds );
		}
	}
}