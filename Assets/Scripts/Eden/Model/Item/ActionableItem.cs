using System;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Core.BuiltInModules;

namespace Eden.Model {
	
	public class ActionableItem : Item {

		public ActionableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) : base (prefabID, displayName, maxCount, expendable, sprite)  {}

		protected override void OnUse (  Eden.Life.Chips.InteractorChip interactor, Action onComplete ) {

			var interactableObject = interactor.GetInteractableObject( this );
			
			if ( interactableObject != null && interactableObject.Actionable ) {

				interactableObject.ActionDelegate.Action( interactor.BlackBox as Eden.Life.BlackBox );

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
