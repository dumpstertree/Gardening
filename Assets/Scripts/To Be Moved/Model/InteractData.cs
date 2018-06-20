using UnityEngine;

namespace Controller.Item {

	public class InteractData {

		private string _animationTrigger;		
		
		public void Interact ( Eden.Life.BlackBox user, InventoryItem item, System.Action onComplete ) {

			// if ( interactor.Interactor ){
			// 	interactor.FaceInteractableObject( interactor.Interactor.transform.position );
			// }
			
			// play animation
			//interactor.Animator.SetTrigger( _animationTrigger );

			user.Interactor.InteractableObject.ActionDelegate.Action( user );
			EdensGarden.Instance.Async.WaitForSeconds( 0.5f, () => { 
				onComplete();
			});
			
			// interactor.Interactor.InteractableObject.InteractDelegate.Interact( interactor, item );

			// run onComplete
		// 	Game.Async.WaitForSeconds( useData.AnimationLength, () => { 
		// 		onComplete();
		// 	});
		
		}
	}
}