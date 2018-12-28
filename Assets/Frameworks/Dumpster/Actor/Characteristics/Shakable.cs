using Dumpster.Core;
using Dumpster.BuiltInModules;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Characteristics {
	
	public class Shakable : Dumpster.Core.Characteristic {
		
		// *************** Public *********************

		public void Shake( float magnitude ) {
			
			transform.localPosition = new Vector3( 
				Random.Range( -magnitude, magnitude ),
 				Random.Range( -magnitude, magnitude ),
 	 			Random.Range( -magnitude, magnitude )
			);

			// _actor.PostNotification( SHAKE );
		}

		public override List<string> GetNotifications () {
			
			return new List<string>() {
				SHAKE
			};
		}

	
		// *************** Protected *********************
		
		protected override void OnActorUpdate () {

			Game.GetModule<Effects>()?.RegisterShakableForFrame( this );
		}

		
		// *************** Private *********************

		private const string SHAKE = "Shakable.Shake";

	}
}