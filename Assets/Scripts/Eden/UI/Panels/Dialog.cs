using UnityEngine;
using UI.Subpanels.Dialog;

namespace Eden.UI.Panels {
	
	public class Dialog : InteractivePanel {

		public override void ReciveInput ( Eden.Input.Package package ) {

			if ( package.Face.Down_Down ) {
				Progress ();
			}
		}
		protected override void OnDismiss () {

			base.OnDismiss();
			Destroy( _presentedDialog.gameObject );
		}

		private void Progress () {
			
			// if is currently presenting, skip presentation
			if ( _presentedDialog.IsPresenting ) {
				_presentedDialog.SkipPresenting();
				return;
			}

			// if the sequence is not done move next, else run on complete
			if ( !_sequence.isDone ) {
				Next ( _sequence.Next () );
			} else {
				Exit ();
			}
		}
		private void Exit () {

			EdensGarden.Instance.UI.Dismiss( EdensGarden.Constants.NewUILayers.Foreground, EdensGarden.Constants.UIContexts.Dialog );

			if ( _onExit != null ) {
				_onExit();
			}
		}


		// ************ Public **************

		public void PresentDialogSequence ( Eden.Controller.Dialog.Sequence sequence, System.Action onExit = null ) {
				
			_sequence = sequence;
			_onExit = onExit;

			Next ( _sequence.Next () );
		}


		// ************ Private **************

		[Header( "Prefab" )]
		[SerializeField] private DialogBox _leftAlignedPrefab;
		[SerializeField] private DialogBox _rightAlignedPrefab;
		[SerializeField] private DialogBox _noneAlignedPrefab;

		[Header( "Refrence" )]
		[SerializeField] private Transform _content;

		private Eden.Controller.Dialog.Sequence _sequence;
		private System.Action _onExit;

		private DialogBox _presentedDialog;

		private void Next ( Model.Dialog.Dialog dialog ) {

			if ( _presentedDialog != null ) {
				Destroy( _presentedDialog.gameObject );
			}

			_presentedDialog = GetDialogBox( dialog.Speaker.Alignment );
			_presentedDialog.Present( dialog, !_sequence.isDone );
		}
		private DialogBox GetDialogBox ( Model.Dialog.PortraitAlignment alignment ) {
			
			switch ( alignment ) {
				
				case Model.Dialog.PortraitAlignment.None :
					return Instantiate( _noneAlignedPrefab, _content );
				
				case Model.Dialog.PortraitAlignment.Left :
					return Instantiate( _leftAlignedPrefab, _content );
				
				case Model.Dialog.PortraitAlignment.Right :
					return Instantiate( _rightAlignedPrefab, _content );
			}

			return null;
		}
	}
}