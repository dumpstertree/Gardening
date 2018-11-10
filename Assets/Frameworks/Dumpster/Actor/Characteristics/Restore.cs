using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Dumpster.Characteristics {

	public class Restore : Dumpster.Core.Characteristic {

		public const string RESTORE = "Restore.Restore";

		private bool _alreadyGiven;

		public override List<string> GetNotifications () {

			return new List<string>() {
				RESTORE
			};
		}
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