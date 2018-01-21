using UnityEngine;
using System;

partial class InventoryItem {

	public bool CanInteract { get{ return _canInteract; } }

	[HeaderAttribute("Interact")]
	[SerializeField] private bool _canInteract;
	[SerializeField] private InteractData _interactData;

	private void Use ( Player player, InventoryItemData data, Action action, Action onComplete ) {
		
		// get use data
		var useData = GetUseAnimationData( data.Animation );
		var interactableObject = player.Interactor.InteractableObject;

		// force player to look at interactable
		if ( interactableObject ){
			player.FaceInteractableObject( interactableObject.transform.position );
		}
		
		// play animation
		player.Animator.SetTrigger( useData.AnimationTrigger );

		// use action
		Game.Async.WaitForSeconds( useData.AnimationUseFraction * useData.AnimationLength, () => {
			action();
		});

		// run onComplete
		Game.Async.WaitForSeconds( useData.AnimationLength, () => { 
			onComplete();
		});

		// reduce count
		if ( _expendable ){
			ReduceCount( 1 );
		}
	}
}
