using System;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Eden.Characteristics;

namespace Eden.Model {
	
	public class ActionableItem : Item {

		public ActionableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) : base (prefabID, displayName, maxCount, expendable, sprite)  {}

		protected override void OnUse ( Dumpster.Core.Actor actor, Action onComplete ) {

			var otherActor = actor.GetCharacteristic<Interactor>()?.GetActor( this );
			
			if ( otherActor != null) {

				otherActor.GetCharacteristic<Talkable>()?.Talk();

				Game.GetModule<Async>()?.WaitForSeconds( 0.5f, () => { 
					onComplete ();
				});
			}
			else {

				onComplete ();
			}
		}
	}
}
