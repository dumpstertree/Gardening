using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Dumpster.Characteristics {

	public class Restore : Dumpster.Core.Characteristic {

		public const string RESTORE = "RESTORE";

		private bool _alreadyGiven;

		protected override void OnActorEnterTrigger ( Actor actor ) {

			if ( !_alreadyGiven ) {
				
				var restorable = actor.GetCharacteristic<Restorable>();
				if ( restorable != null) {
					
					restorable.Restore( 10 );
					_alreadyGiven = true;

					_actor.PostNotification( RESTORE );
				}
			}
		}
	}
}