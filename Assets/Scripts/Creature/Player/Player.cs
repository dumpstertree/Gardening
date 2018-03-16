using UnityEngine;

public class Player : Creature {

	
	// ***************** PUBLIC *******************

	public Model.PartInventory GunParts { 
		get { return _gunParts; }
	}

	public QuickSlot QuickSlot { 
		get{ return _quickslot; }  
	}
	public Transform CameraTarget {
		get{ return _cameraTarget; }
	}
	public Transform CameraFocus {
		get{ return _cameraFocus; }
	}


	// ***************** PRIVATE *******************

	[Header( "Player Properties" )]
	[SerializeField] private QuickSlot _quickslot;

	private Transform _cameraTarget;	
	private Transform _cameraFocus;
	private PlayerDataController _dataController;
	private Model.PartInventory _gunParts;
	private CameraType _cameraType;

	public PlayerRecipes PlayerRecipes; // this should be some kind of data type not a monobehavior

	private const string CAMERA_TARGET_NAME = "CameraTarget";
	private const string CAMERA_FOCUS_NAME = "CameraFocus";
	
	// *********************************************
	
	public override void Init () {

		base.Init();

		_dataController = new PlayerDataController();

		CreateCameraTarget();
		CreateCameraFocus();

		
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
	}


	// *********************************************

	private void CreateCameraTarget () {
		
		_cameraTarget = new GameObject( CAMERA_TARGET_NAME ).transform;
	}
	private void CreateCameraFocus () {

		_cameraFocus = new GameObject( CAMERA_FOCUS_NAME ).transform;
	}
}