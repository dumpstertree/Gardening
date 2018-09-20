using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

namespace Eden.Life.BlackBoxes {

	public class Player : Eden.Life.BlackBox, IInputReciever<Input.Package> {


		// *************** Public ******************

		public delegate void RecieveInputEvent ( Input.Package package );
		public RecieveInputEvent OnRecieveInput;

		
		// *************** IInputReciever ******************
		
		void IInputReciever<Input.Package>.RecieveInput ( Input.Package package ) { FireRecieveInputEvent ( package ); }
		void IInputReciever<Input.Package>.EnteredInputFocus () {}
		void IInputReciever<Input.Package>.ExitInputFocus () {}

		
		// *************** Private ******************
		
		[SerializeField] private Transform _cameraTarget;

		private void FireRecieveInputEvent ( Input.Package package ) {

			if ( _isPowered ) {
			
				if ( OnRecieveInput != null ) {
					
					OnRecieveInput ( package );
				}
			}
		}


		// *************** Override ******************
		protected override void Shutdown () {

			// Clear all current input
			FireRecieveInputEvent( new Input.Package() );

			base.Shutdown ();
		}
		
		protected override void Init () {
			
			base.Init ();

			Inventory.OnInventoryItemChanged += ( index, item ) => {
				EdensGarden.Instance.Data.Save( Data.Path.Player, "Inv", Inventory.Clone() );
			};

			EquipedItems.OnInventoryItemChanged += ( index, item ) => {
				EdensGarden.Instance.Data.Save( Data.Path.Player, "EquipedInv", EquipedItems.Clone() );
			};

			// Add player to input stack
			EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );
				
			// Set this player as the camera focus
			EdensGarden.Instance.Camera.SetFocus( _cameraTarget );	
		}
	}
}