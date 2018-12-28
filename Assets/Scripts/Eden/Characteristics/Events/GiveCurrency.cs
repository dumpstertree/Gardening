using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;

namespace Eden.Characteristics {
  
    public class GiveCurrency : Dumpster.Core.Characteristic {

		[SerializeField] private int _amountOfCurrency;

	    private const string GIVE = "GiveCurency.Give";

        private bool _alreadyGiven;

        public override List<string> GetNotifications () {

			return new List<string>() {
				GIVE
			};
		}
        protected override void OnActorEnterTrigger ( Actor actor ) {
			
			if ( !_alreadyGiven ) {

				var recieve = actor.GetCharacteristic<RecieveCurrency>();
				if ( recieve != null) {

					recieve.Recieve( _amountOfCurrency );
					_alreadyGiven = true;

					_actor.PostNotification( GIVE );
				}
			}
		}
    }
}