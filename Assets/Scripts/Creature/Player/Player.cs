using System.Collections.Generic;
using UnityEngine;

public class Player : Creature, IInputReciever {

	void IInputReciever.OnConfirmDown () {}
	void IInputReciever.OnConfirmUp () {}
	void IInputReciever.OnCancelDown () {}
	void IInputReciever.OnCancelUp () {}
	void IInputReciever.OnStartDown () {}
	void IInputReciever.OnStartUp () { 
		Game.UIController.ChangeContext( UIController.UiContext.Identifier.Inventory );
	}
	void IInputReciever.HorizontalChanged ( float horizontal ) {}
	void IInputReciever.VerticalChanged ( float vertical ) {}

	
	// ***************** PUBLIC *******************

	public Dumpster.Physics.Controller Physics {
		get { return _physics; }
	}
	public Model.PartInventory GunParts { 
		get { return _gunParts; }
	}

	public QuickSlot QuickSlot { 
		get{ return _quickslot; }  
	}
	public override Animations AnimationsData {
		get{ 
			return new Animations(
				new Animation( "", 0f, 0f ),
				new Animation( "Pickup", 1.5f, 0.5f ) 
			); 
		}
	}

	// ***************** PRIVATE *******************

	[Header( "Player Properties" )]
	[SerializeField] private QuickSlot _quickslot;
	[SerializeField] private Model.Template.InventoryItemTemplate _hand;
	[SerializeField] private Dumpster.Physics.Controller _physics;

	private PlayerDataController _dataController;
	private Model.PartInventory _gunParts;
	private CameraType _cameraType;

	public PlayerRecipes PlayerRecipes; // this should be some kind of data type not a monobehavior

	private const string CAMERA_TARGET_NAME = "CameraTarget";
	private const string CAMERA_FOCUS_NAME = "CameraFocus";
	
	// *********************************************
	
	private void Start() {

		Init();
	}

	public override void Init () {

		base.Init();

		CreateDataController();	
		
		// load data
		_quickslotInventory = _dataController.LoadQuickSlotInventory();
		_inventory = _dataController.LoadInventory();
		_gunParts = _dataController.LoadPartInventory();
		
		
		// save when changes are made
		_quickslotInventory.OnInventoryItemChanged += (index, item) => { 
			_dataController.SaveQuickSlotInventory( _quickslotInventory ); 
		};
		_inventory.OnInventoryItemChanged += (index, item) => {
			_dataController.SaveInventory( _inventory );
		};
		_gunParts.OnPartListChanged += () => {
			_dataController.SavePartInventory( _gunParts );
		};




		// set hand item
		_quickslotInventory.SetInventoryItem( _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center ), _hand.GetInstance(1));


		// add input reciever
		Game.Input.AddReciever( 

			new InputRecieverLayer(
			
				new List<IInputReciever>() {
					this,
					_interactor as PlayerInteractor,
					_brain as PlayerMovement
				}
			),
		0 );

		EdensGarden.Instance.Camera.SetFocus( transform );

		print( "player init" );
	}


	// *********************************************

	private void CreateDataController () {

		_dataController = new PlayerDataController();	
	}
}