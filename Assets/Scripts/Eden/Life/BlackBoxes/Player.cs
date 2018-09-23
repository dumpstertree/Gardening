using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

namespace Eden.Life.BlackBoxes {

	public class Player : Eden.Life.BlackBox, IInputReciever<Input.Package> {

		
		// *************** IInputReciever ******************
		

		public void RecieveInput ( Input.Package package ) {
			
			FireRecieveInputEvent ( package ); 

			if ( package.Face.Up_Down )         { FireOpenInventoryMenuEvent(); }
			if ( package.BackLeft.Bumper_Down ) { FireBeginAimingEvent (); }
			if ( package.BackLeft.Bumper_Up )   { FireEndAimingEvent (); }
			if ( package.Face.Down_Down )       { FireOpenInventoryMenuEvent(); }

			
			FireMoveCameraControllerXEvent ( package.RightAnalog.Horizontal );
			FireMoveCameraControllerYEvent ( package.RightAnalog.Vertical );
			FireMoveCharacterControllerXEvent ( package.LeftAnalog.Horizontal );
			FireMoveCharacterControllerYEvent ( package.LeftAnalog.Vertical );
		}
		public void EnteredInputFocus () {
		}
		public void ExitInputFocus () {
		}

		
		// *************** Public ******************


		public delegate void RecieveInputEvent ( Input.Package package );
		public RecieveInputEvent OnRecieveInput;

		public delegate void BeginAimingEvent ();
		public  BeginAimingEvent OnBeginAiming;
		
		public delegate void EndAimingEvent ();
		public  EndAimingEvent OnEndAiming;
		
		public delegate void MoveCameraControllerXEvent ( float x );
		public  MoveCameraControllerXEvent OnMoveCameraControllerX;
		
		public delegate void MoveCameraControllerYEvent ( float y );
		public  MoveCameraControllerYEvent OnMoveCameraControllerY;

		public delegate void MoveCharacterControllerXEvent ( float x );
		public  MoveCharacterControllerXEvent OnMoveCharacterControllerX;
		
		public delegate void MoveCharacterControllerYEvent ( float y );
		public  MoveCharacterControllerYEvent OnMoveCharacterControllerY;

		public delegate void JumpEvent ();
		public  JumpEvent OnJump;
		
		public delegate void OpenInventoryMenuEvent ();
		public  OpenInventoryMenuEvent OnOpenInventoryMenu;


		
		// *************** Private ******************
		
		
		[Header( "Camera" )]
		[SerializeField] private Transform _cameraTarget;


		private void FireRecieveInputEvent ( Input.Package package ) {

			if ( _isPowered ) {
			
				if ( OnRecieveInput != null ) {
					
					OnRecieveInput ( package );
				}
			}
		}
		private void FireBeginAimingEvent () {

			if ( OnBeginAiming != null ) { OnBeginAiming (); }
		}
		private void FireEndAimingEvent () {
			
			if ( OnEndAiming != null ) { OnEndAiming (); }
		}
		private void FireMoveCameraControllerXEvent ( float x ) {

			if ( OnMoveCameraControllerX != null ) { OnMoveCameraControllerX ( x ); }
		}
		private void FireMoveCameraControllerYEvent ( float y ) {

			if ( OnMoveCameraControllerY != null ) { OnMoveCameraControllerY ( y ); }
		}
		private void FireMoveCharacterControllerXEvent ( float x ) {

			if ( OnMoveCharacterControllerX != null ) { OnMoveCharacterControllerX ( x ); }
		}
		private void FireMoveCharacterControllerYEvent ( float y ) {

			if ( OnMoveCharacterControllerY != null ) { OnMoveCharacterControllerY ( y ); }
		}
		private void FireJumpEvent () {

			if ( OnJump != null ) { OnJump (); };
		}
		private void FireOpenInventoryMenuEvent () {

			if ( OnOpenInventoryMenu != null ) { OnOpenInventoryMenu (); }
		}


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

			
			// // Add player to input stack
			// EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );
				
			
			// Set this player as the camera focus
			// EdensGarden.Instance.Camera.SetFocus( _cameraTarget );
		}
	}
}