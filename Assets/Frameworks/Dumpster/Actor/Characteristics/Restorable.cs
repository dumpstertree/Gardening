using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {

	public class Restorable : Dumpster.Core.Characteristic {

		public void Restore ( int amount ) {

			_actor.GetCharacteristic<Health>()?.AddHealth( amount );
		}
	}
}