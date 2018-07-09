using System;
using UnityEngine;

namespace Eden.Model {
	
	public class ActionableItem : Item {

		public ActionableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) : base (prefabID, displayName, maxCount, expendable, sprite)  {}

		protected override void OnUse ( Eden.Life.BlackBox user, Eden.Interactable.InteractableObject interactable, Action onComplete ) {

			Debug.Log ( "use" );
			user.Interactor.InteractableObject.ActionDelegate.Action( user );
			EdensGarden.Instance.Async.WaitForSeconds( 0.5f, () => { 
				onComplete();
			});
		}
	}
}
