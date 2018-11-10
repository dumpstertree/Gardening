using Dumpster.Core;
using Eden.Modules;
using UnityEngine;

public class Talkable : Characteristic {
	
	[SerializeField] private Eden.Model.Dialog.Sequence _dialogSequence;
	
	public void Talk () {
	
		Game.GetModule<Dumpster.BuiltInModules.UI>()?.Present( 
			
			Game.GetModule<Constants>().UILayers.Foreground, 
			Game.GetModule<Constants>().UIContexts.Dialog, 
			SetDialog 
		);
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
