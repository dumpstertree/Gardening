using UnityEngine;
using Dumpster.Core.BuiltInModules.Input;

public class Player : Creature, IInputReciever<Eden.Input.Package> {

	void IInputReciever<Eden.Input.Package>.RecieveInput ( Eden.Input.Package package ) {
		FireRecieveInputEvent ( package );
	}
	void IInputReciever<Eden.Input.Package>.EnteredInputFocus () {}
	void IInputReciever<Eden.Input.Package>.ExitInputFocus () {}
	
	public delegate void RecieveInputEvent ( Eden.Input.Package package );
	public RecieveInputEvent OnRecieveInput;


	// ***************** PUBLIC *******************

	public Model.PartInventory GunParts { 
		get { return _gunParts; }
	}

	public Eden.BlackBox.QuickSlot QuickSlot { 
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
	[SerializeField] private Eden.BlackBox.QuickSlot _quickslot;
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
	private void FireRecieveInputEvent ( Eden.Input.Package package ) {

		if ( OnRecieveInput != null ) {
			OnRecieveInput ( package );
		}
	}
}