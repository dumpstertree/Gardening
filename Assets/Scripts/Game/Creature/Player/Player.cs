using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class Player : Creature, IInputReciever<Eden.Input.Package> {

	void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {}
	void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {
		print( "player enter focus" );
	}
	void IInputReciever<Eden.Input.Package>.ExitInputFocus () {
		print( "player left focus" );
	}
	
	// ***************** PUBLIC *******************

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

	private PlayerDataController _dataController;
	private Model.PartInventory _gunParts;
	private CameraType _cameraType;

	public PlayerRecipes PlayerRecipes; // this should be some kind of data type not a monobehavior

	private const string CAMERA_TARGET_NAME = "CameraTarget";
	private const string CAMERA_FOCUS_NAME = "CameraFocus";
	
	// *********************************************
	
	private void Awake() {

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

		EdensGarden.Instance.Input.RegisterToInputLayer( EdensGarden.Constants.InputLayers.Player, this );
		
		EdensGarden.Instance.Camera.SetFocus( transform );
	}


	// *********************************************

	private void CreateDataController () {

		_dataController = new PlayerDataController();	
	}
}