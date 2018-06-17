using UnityEngine;

namespace Controller.Item {

	public class InteractData {

		private string _animationTrigger;		
		
		public void Interact ( Creature interactor, InventoryItem item ) {

			if ( interactor.Interactor ){
				interactor.FaceInteractableObject( interactor.Interactor.transform.position );
			}
			
			// play animation
			interactor.Animator.SetTrigger( _animationTrigger );

			interactor.Interactor.InteractableObject.InteractDelegate.Interact( interactor, item );

			// // use action
			// Game.Async.WaitForSeconds( useData.AnimationUseFraction * useData.AnimationLength, () => {
			// 	action();
			// });

			// // run onComplete
			// Game.Async.WaitForSeconds( useData.AnimationLength, () => { 
			// 	onComplete();
			// });
			// }
		}
	}
}