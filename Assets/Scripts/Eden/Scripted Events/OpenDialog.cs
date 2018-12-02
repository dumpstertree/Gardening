using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;
using Eden.Modules;

public class OpenDialog : MonoBehaviour {

	[SerializeField] private Eden.Model.Dialog.Sequence _dialogSequence;
	private bool _hasBeenTriggered;

	private void OnTriggerEnter ( Collider other ) {	
		
		if ( _hasBeenTriggered || !Game.GetModule<Eden.Modules.Constants>().UseDialogs ) {
			return;
		}

		var otherActor = other.GetComponent<Actor>();
		var isPlayer = otherActor?.GetCharacteristic<Alignment>()?.MyAlignment == Alignment.Type.Player;
		
		if ( isPlayer ) {
			
			Game.GetModule<Dumpster.BuiltInModules.UI>()?.Present( 
				
				Game.GetModule<Constants>().UILayers.Foreground, 
				Game.GetModule<Constants>().UIContexts.Dialog, 
				SetDialog 
			);
			
			_hasBeenTriggered = true;
		}
	}
	private void SetDialog ( Dumpster.BuiltInModules.Context context ) {

		var panel = context.GetContext( "DialogUIPanel(Clone)" );
		if ( panel != null ) {

			var dialog = panel.GetComponent<Eden.UI.Panels.Dialog>();
			if ( dialog != null ) {

				dialog.PresentDialogSequence( _dialogSequence.GetController() );
			}
		}
	}
}
