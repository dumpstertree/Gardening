using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {

	public class GiveZen : Dumpster.Core.Characteristic {
		
		public const string GIVE = "GiveZen.Give";

		[SerializeField] private int _amountOfZen;

		private bool _alreadyGiven;

		public override List<string> GetNotifications () {

			return new List<string>() {
				GIVE
			};
		}
		protected override void OnActorEnterTrigger ( Actor actor ) {
			
			if ( !_alreadyGiven ) {

				var recieve = actor.GetCharacteristic<RecieveZen>();
				if ( recieve != null) {

					recieve.Recieve( _amountOfZen );
					_alreadyGiven = true;

					_actor.PostNotification( GIVE );
				}
			}
		}
	}
}
