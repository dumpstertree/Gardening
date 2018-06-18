using Dumpster.Core.BuiltInModules.Input;

namespace Eden.Life.BlackBoxes {

	public class Player : Eden.Life.Brain.BlackBoxBrain, IInputReciever<Input.Package> {


		// *************** Public ******************

		public delegate void RecieveInputEvent ( Input.Package package );
		public RecieveInputEvent OnRecieveInput;

		
		// *************** IInputReciever ******************
		
		void IInputReciever<Input.Package>.RecieveInput ( Input.Package package ) { FireRecieveInputEvent ( package ); }
		void IInputReciever<Input.Package>.EnteredInputFocus () {}
		void IInputReciever<Input.Package>.ExitInputFocus () {}

		
		// *************** Private ******************
		
		private void FireRecieveInputEvent ( Input.Package package ) {

			if ( OnRecieveInput != null ) {
				OnRecieveInput ( package );
			}
		}


		// *************** Override ******************
		
		protected override void Init () {
			
			base.Init ();

			// Add player to input stack
			EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );
				
			// Set this player as the camera focus
			EdensGarden.Instance.Camera.SetFocus( transform );	
		}
	}
}