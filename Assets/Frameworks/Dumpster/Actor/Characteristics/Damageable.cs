using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {
	
	public class Damageable : Dumpster.Core.Characteristic {

		public void Damage () {

			_actor.GetCharacteristic<Health>( true )?.SubtractHealth( 1 );
		}
	}
}