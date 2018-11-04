using Dumpster.Core;
using Dumpster.Events;
using Eden.Modules;
using UnityEngine;

namespace Eden.Events {
	
	public class PresentDialog : MonoBehaviour {
		
		[SerializeField] private Eden.Interactable.Actionable _actionable;
		[SerializeField] private SmartEvent[] _onExit;
		[SerializeField] private Model.Dialog.Sequence _dialogSequence;

		private void Awake () {

			_actionable.OnAction += HandleRecieveAction;
		}
		private void HandleRecieveAction( Eden.Life.BlackBox user ) {
			
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

					dialog.PresentDialogSequence( _dialogSequence.GetController(), FireOnExitEvent );
				}
			}
		}
		private void FireOnExitEvent () {
			
			foreach (  SmartEvent e in _onExit ) {
				e.EventTriggered();
			}
		} 
	}
}