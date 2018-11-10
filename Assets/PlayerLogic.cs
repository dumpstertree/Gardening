using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Dumpster.Core.BuiltInModules.Input;
using Dumpster.BuiltInModules.Camera.Defaults;
using Eden.Model;
using UnityEngine;
using Eden.Modules;
using Eden.Characteristics;


public class PlayerLogic : Characteristic, IInputReciever<Eden.Input.Package> {
	
	
	// **************** IInputReciever ********************

	public Item CurrentItemInHand {
		get{ return _currentItem; }
	}
	
	public void RecieveInput ( Eden.Input.Package package ) {

		if ( package.Dpad.Left_Down )     	{ ShiftQuickSlotLeft (); }
		if ( package.Dpad.Right_Down )     	{ ShiftQuickSlotRight (); }
		if ( package.BackRight.Bumper )     { UseItem (); }
		if ( package.Face.Up_Down )         { OpenInventoryMenu (); }
		if ( package.BackLeft.Bumper_Down ) { BeginAiming (); }
		if ( package.BackLeft.Bumper_Up )   { EndAiming (); }
		if ( package.Face.Down_Down )       { Jump (); }
		
		MoveCameraControllerX ( package.RightAnalog.Horizontal );
		MoveCameraControllerY ( package.RightAnalog.Vertical );
		MoveCharacterControllerX ( package.LeftAnalog.Horizontal );
		MoveCharacterControllerY ( package.LeftAnalog.Vertical );
	}
	public void EnteredInputFocus () {
	}
	public void ExitInputFocus () {	
		
		RecieveInput( new Eden.Input.Package() );
	}

	
	// **************** Private ********************
	
	[Header( "Camera" )]
	[SerializeField] private Transform _cameraTarget;
	[SerializeField] private ShoulderCameraController _cameraController;

	[Header( "Movement" )]
	[SerializeField] private Dumpster.Controllers.ThirdPersonCharacterController _characterController;

	private bool _itemIsEquiped;
	private Item _currentItem {
		get {
			Item item;
			if ( _itemIsEquiped ) {
				var index = _actor.GetCharacteristic<EquipedItemSwapper>( true ).EquipedIndex;
				item =_actor.GetCharacteristic<EquippedItemsInventory>( true ).Inventory.GetInventoryItem( index );
				if ( item == null ) {
					item = _actor.GetCharacteristic<EquippedItemsInventory>( true ).DefaultEquipedItem;
				}
			} else {
				item =  _actor.GetCharacteristic<EquippedItemsInventory>( true ).DefaultEquipedItem;
			}
			return item;
		}
	}


	// character controls
	private void BeginAiming () {

		_itemIsEquiped = true;

		if ( _currentItem != null && _currentItem.IsShootable )  {

			_cameraController.IsStrafing = true;
			_characterController.IsStrafing = true;
		}
	}
	private void EndAiming () {

		_itemIsEquiped = false;
		
		_cameraController.IsStrafing = false;
		_characterController.IsStrafing = false;
	}
	private void MoveCameraControllerX ( float x ) {

		_cameraController.HorizontalInput = x;
	}
	private void MoveCameraControllerY ( float y ) {

		_cameraController.VericalInput = y;
	}
	private void MoveCharacterControllerX ( float x ) {

		_characterController.HorizontalInput = x;
	}
	private void MoveCharacterControllerY ( float y ) {

		_characterController.VerticalInput = y;
	}
	private void Jump () {

		_characterController.Jump ();
	}
	

	// ui
	private void OpenInventoryMenu () {

		Game.GetModule<Dumpster.BuiltInModules.UI>()?.Present( 
				
			Game.GetModule<Constants>().UILayers.Midground,
			Game.GetModule<Constants>().UIContexts.Inventory
		);
	}
	

	// use
	private void UseItem () {

		_actor.GetCharacteristic<Interactor>( true )?.Use( _currentItem );
	}
	

	// equiped items
	private void ShiftQuickSlotLeft () {

		_actor.GetCharacteristic<EquipedItemSwapper>( true )?.ShiftLeft();
	}
	private void ShiftQuickSlotRight () {
		
		_actor.GetCharacteristic<EquipedItemSwapper>( true )?.ShiftRight();
	}


	// to be moved to a an init of some sort
	private void Start () {

		// listen for input
		Game.GetModule<Eden.Input>()?.RegisterToInputLayer (
			Game.GetModule<Constants>().InputLayers.Player, 
			this 
		);

		Game.GetModule<Eden.Input>()?.RequestInput (
			Game.GetModule<Constants>().InputLayers.Player 
		);


		// // set the camera focus
		Game.GetModule<Dumpster.Core.BuiltInModules.Camera>()?.SetFocus ( 
			_cameraTarget 
		);	


		// // present the HUD
		Game.GetModule<Dumpster.BuiltInModules.UI>()?.Present ( 
			Game.GetModule<Constants>().UILayers.Midground,
			Game.GetModule<Constants>().UIContexts.Player 
		);
	}
}
