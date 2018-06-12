using UnityEngine;
using UI.Subpanels.Dialog;
using Dumpster.Core.BuiltInModules.Input;

namespace UI.Panels {
	
	public class Dialog : UiPanel, IInputReciever<Eden.Input.Package> {


		void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {
			if( package.ConfirmUp ){ OnConfirmUp (); }
		}
		void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {}
		void IInputReciever<Eden.Input.Package>.ExitInputFocus () {}
		
		protected override void OnInit () { 

			EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Dialog, this );
		}
		protected override void OnPresent () {

			EdensGarden.Instance.Input.RequestInput( EdensGarden.Constants.InputLayers.Dialog );
		}
		protected override void OnDismiss () {
			
			EdensGarden.Instance.Input.RelinquishInput( EdensGarden.Constants.InputLayers.Dialog );
		}

		private void OnConfirmUp () {

			if( _sequence != null ) {
			
				// if is currently presenting, skip presentation
				if ( _presentedDialog.IsPresenting ) {
					_presentedDialog.SkipPresenting();
					return;
				}

				// if the sequence is not done move next, else run on complete
				if ( !_sequence.isDone ) {
					Next ( _sequence.Next () );
				} else {
					Exit();
				}
			}
		}


		// ************ Public **************

		public void PresentDialogSequence ( Model.Dialog.Sequence sequence ) {
			
			_sequence = sequence;

			Next ( _sequence.Next () );
		}


		// ************ Private **************

		[Header( "Prefab" )]
		[SerializeField] private DialogBox _leftAlignedPrefab;
		[SerializeField] private DialogBox _rightAlignedPrefab;
		[SerializeField] private DialogBox _noneAlignedPrefab;

		[Header( "Refrence" )]
		[SerializeField] private Transform _content;

		private Model.Dialog.Sequence _sequence;
		private DialogBox _presentedDialog;

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