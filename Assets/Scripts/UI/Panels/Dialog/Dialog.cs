using UnityEngine;
using UI.Subpanels.Dialog;

namespace UI.Panels {
	
	public class Dialog : UiPanel {

		[Header( "Prefab" )]
		[SerializeField] private DialogBox _leftAlignedPrefab;
		[SerializeField] private DialogBox _rightAlignedPrefab;
		[SerializeField] private DialogBox _noneAlignedPrefab;

		[Header( "Refrence" )]
		[SerializeField] private Transform _content;

		private Model.Dialog.Sequence _sequence;
		private System.Action _onComplete;
		private DialogBox _presentedDialog;

		// ************ Public **************

		public void PresentDialogSequence ( Model.Dialog.Sequence sequence, System.Action onComplete ) {
			
			_sequence = sequence;
			_onComplete = onComplete;

			Next ( _sequence.Next () );
		}

		// ************ Public **************

		protected override void Update () {
			
			if ( Input.GetKeyDown( KeyCode.RightShift ) ) {
				
				// if is currently presenting, skip presentation
				if ( _presentedDialog.IsPresenting ) {
					_presentedDialog.SkipPresenting();
					return;
				}

				// if the sequence is not done move next, else run on complete
				if ( !_sequence.isDone ) {
					Next ( _sequence.Next () );
				} else {
					_onComplete ();
				}
			}
		}

		private void Next ( Model.Dialog.Sequence.Dialog dialog ) {

			if ( _presentedDialog != null ) {
				Destroy( _presentedDialog.gameObject );
			}

			_presentedDialog = GetDialogBox( dialog.Alignment );
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