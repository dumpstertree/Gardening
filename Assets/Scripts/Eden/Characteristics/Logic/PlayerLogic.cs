using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Dumpster.Core.BuiltInModules.Input;
using Dumpster.BuiltInModules.Camera.Defaults;
using Eden.Model;
using Eden.Modules;
using Eden.Characteristics;
using System.Collections.Generic;
using UnityEngine;


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
		if ( package.Face.Down_Down )       { BeginJump(); }
		if ( package.Face.Down_Up )         { EndJump(); }
		if ( package.Face.Left_Down ) { if( !_hasDashed ) { BeginDash(); _hasDashed = true; } }
		if ( package.Face.Left_Up ) { EndDash(); _hasDashed = false; }
		
		MoveCameraControllerX ( package.RightAnalog.Horizontal );
		MoveCameraControllerY ( package.RightAnalog.Vertical );
		MoveCharacterControllerX ( package.LeftAnalog.Horizontal );
		MoveCharacterControllerY ( package.LeftAnalog.Vertical );
		
		_movementMagnitude = Vector2.Distance( Vector2.zero, new Vector2( package.LeftAnalog.Horizontal, package.LeftAnalog.Vertical ) );
	}
	public void EnteredInputFocus () {
	}
	public void ExitInputFocus () {	
		
		RecieveInput( new Eden.Input.Package() );
	}

	// **************** Protected **************************

	public override List<string> GetNotifications () {

		return new List<string> () {
			ON_ZEN_LEVEL_UP,
			ON_ZEN_BREAK
		};
	}
	
	protected override void OnActorUpdate() {
		
		if ( _isDashing ) {
			if ( Time.time - _lastDashStartTime > _dashLength ) {
				EndDash();
			}
		}

		if ( _isSprinting ) {
			if ( _movementMagnitude < 0.5f ) {
				EndSprinting();
			}
		}

		if ( _isJumping ) {
			if ( Time.time - _jumpStartTime > 0.5f ) {
				EndJump();
			}
		}
	}
	
	// **************** Private ********************
	
	[Header( "Camera" )]
	[SerializeField] private Transform _cameraTarget;
	[SerializeField] private ShoulderCameraController _cameraController;

	[Header( "Movement" )]
	[SerializeField] private Dumpster.Controllers.ThirdPersonCharacterController _characterController;
	[SerializeField] private CanUseRangedWeapons[] _rangedSpawners;
	
	[Header( "Dash Visuals" )]
	[SerializeField] private TrailRenderer _dashTrailRendererLeftFoot;
	[SerializeField] private TrailRenderer _dashTrailRendererRightFoot;

	[Header( "Jump Visuals" )] 
	[SerializeField] private GameObject _dustParticle;

	private bool _hasDashed;
	private bool _jumping;
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

	private const string ON_ZEN_LEVEL_UP = "PlayerLogic.ZenLevelUp";
	private const string ON_ZEN_BREAK = "PlayerLogic.ZenBreak";
	

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

	private bool _isSprinting;
	private float _movementMagnitude;
	private bool _isDashing;
	private bool _isJumping;
	private float _lastDashEndTime;
	private float _lastDashStartTime;
	private float _dashCooldown = 0.75f;
	private float _dashLength = 0.5f;

	private float _jumpStartTime;


	// dashing
	private void BeginDash () {

		if ( !_isDashing && Time.time - _lastDashEndTime > _dashCooldown ) {
			
			_lastDashStartTime = Time.time;
			_isDashing = true;
			_characterController.IsDashing = true;
		}
	}	
	private void EndDash () {

		if ( _isDashing ){

			_lastDashEndTime = Time.time;
			_characterController.IsDashing = false;
			_isDashing = false;
		}
	}

	// jumping
	private void BeginJump () {

		if ( !_isJumping ) {
			_isJumping = true;
			_jumpStartTime = Time.time;
			_characterController.IsJumping = true;

			Instantiate( _dustParticle, _actor.transform.position, Quaternion.Euler( new Vector3( 90,0,0 ) ) );
		}
	}
	private void EndJump () {

		if ( _isJumping ) {
			_characterController.IsJumping = false;
			_isJumping = false;
		}
	}

	// sprinting
	private void ToggleSprinting () {

		if ( _isSprinting ) {
			EndSprinting ();
		}else {
			BeginSprinting ();
		}
	}
	private void BeginSprinting () {

		_isSprinting = true;
	}
	private void EndSprinting () {

		_isSprinting = false;
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

	
	// zen
	private void ZenLevelUp ( int level ) {

		for ( int i=1; i<1+level; i++ ) {
			_rangedSpawners[ i ].Secret = false;
			_rangedSpawners[ i ].gameObject.SetActive( true );
		}

		_actor.PostNotification( ON_ZEN_LEVEL_UP );
	}
	private void ZenBreak () {
		
		for ( int i=1; i<_rangedSpawners.Length; i++ ) {
			_rangedSpawners[ i ].Secret = true;
			_rangedSpawners[ i ].gameObject.SetActive( false );
		}

		_actor.PostNotification( ON_ZEN_BREAK );
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

		
		// listen to zen events
		Game.GetModule<Eden.Modules.Zen>().OnLevelUp += ZenLevelUp;
		Game.GetModule<Eden.Modules.Zen>().OnBreak += ZenBreak;

		for ( int i=1; i<_rangedSpawners.Length; i++ ) {
			_rangedSpawners[ i ].Secret = true;
			_rangedSpawners[ i ].gameObject.SetActive( false );
		}
	}
}
