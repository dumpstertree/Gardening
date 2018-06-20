using UnityEngine;
using Dumpster.Events;

namespace Eden.Events {
	
	public class PresentDialog : MonoBehaviour {
		
		[SerializeField] private Eden.Interactable.Actionable _actionable;
		[SerializeField] private SmartEvent[] _onExit;
		[SerializeField] private Model.Dialog.Sequence _dialogSequence;

		private void Awake () {

			_actionable.OnAction += HandleRecieveAction;
		}
		private void HandleRecieveAction( Eden.Life.BlackBox user ) {
			
			EdensGarden.Instance.UI.Present( EdensGarden.Constants.NewUILayers.Foreground, EdensGarden.Constants.UIContexts.Dialog, SetDialog );
		}
		private void SetDialog ( Dumpster.Core.BuiltInModules.UI.Context context ) {

			var panel = context.GetContext( "DialogUIPanel(Clone)" );
			if ( panel != null ) {

				var dialog = panel.GetComponent<Eden.UI.Panels.Dialog>();
				if ( dialog != null ) {

					var seq = _dialogSequence.GetController();
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