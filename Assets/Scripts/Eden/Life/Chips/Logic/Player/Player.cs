using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Dumpster.Core.BuiltInModules.Input;
using Dumpster.BuiltInModules.Camera.Defaults;
using Eden.Model;
using Eden.Model.Life;
using UnityEngine;
using Eden.Modules;

namespace Eden.Life.Chips.Logic {

	public class Player : Dumpster.Core.Life.LogicChip<Model.Life.Visual>, IInputReciever<Input.Package> {

		
		// *************** IInputReciever ******************
		
		public void RecieveInput ( Input.Package package ) {

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

			RecieveInput( new Input.Package() );
		}

		
		// *************** Public ******************

		private Eden.Life.BlackBox _blackBox {
			get { return BlackBox as Eden.Life.BlackBox; }
		}
		private Item _currentItem {
			get {

				Item item;
				if ( _itemIsEquiped ) {
					var index = _quickslotChip.EquipedIndex;
					item = _blackBox.EquipedItems.GetInventoryItem( index );
					if ( item == null ) {
						item  = _blackBox.PrimaryEquipedItem;
					}
				} else {
					item = _blackBox.PrimaryEquipedItem;
				}
				return item;
			}
		}

		[Header( "Camera" )]
		[SerializeField] private Transform _cameraTarget;
		[SerializeField] private ShoulderCameraController _cameraController;

		[Header( "Movement" )]
		[SerializeField] private Dumpster.Controllers.ThirdPersonCharacterController _characterController;

		[Header( "Interactable" )]
		[SerializeField] private QuickSlotChip _quickslotChip;
		[SerializeField] private InteractorChip _interactorChip;
		
		[Header( "UI" )]
		[SerializeField] private UIChip _uiChip;

		private bool _itemIsEquiped;


		private void BeginAiming () {

			_itemIsEquiped = true;

			if ( _currentItem.IsShootable )  {

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
		private void OpenInventoryMenu () {

			_uiChip.OpenInventory ();
		}
		private void UseItem () {

			_interactorChip.Use( _currentItem );
		}
		private void ShiftQuickSlotLeft () {

			_quickslotChip.ShiftLeft ();
		}
		private void ShiftQuickSlotRight () {
			
			_quickslotChip.ShiftRight ();	
		}

		protected override void Think () {
		}

		protected override void GetVisual( Visual visual ) {
			
			// i should bring back the targeting chip probably to interfact with the targetable system

			if ( _currentItem.IsShootable ) {
				var t = Game.GetModule<Targeting>()?.GetTargetable( UnityEngine.Camera.main.transform.position, UnityEngine.Camera.main.transform.forward, 15f );
				if ( t != null ) visual.Target = t.GetComponentInParent<BlackBox>();
			}

			visual.CurrentItemInHand = _currentItem;
			visual.InteractingWith = _interactorChip.GetInteractableObject( _currentItem );
		}

		private void Start () {

			Game.GetModule<Eden.Input>()?.RegisterToInputLayer( Game.GetModule<Constants>().InputLayers.Player, this );		
			Game.GetModule<Eden.Input>()?.RequestInput( Game.GetModule<Constants>().InputLayers.Player );

			Game.GetModule<Dumpster.Core.BuiltInModules.Camera>()?.SetFocus( _cameraTarget );	

			Game.GetModule<Dumpster.BuiltInModules.UI>().Present( 
				Game.GetModule<Constants>().UILayers.Midground,
				Game.GetModule<Constants>().UIContexts.Player 
			);
		}
	}
}