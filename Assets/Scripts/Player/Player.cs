using UnityEngine;

public class Player : Creature {

	// ***************** PUBLIC *******************

	private QuickSlotInventory _quickslotInventory;
	public QuickSlotInventory QuickslotInventory{ get{ return _quickslotInventory; } }
	
	private Inventory _inventory;
	public Inventory Inventory { get{ return _inventory; }  }

	private Model.PartInventory _gunParts;
	public Model.PartInventory GunParts { get { return _gunParts; }}

	[SerializeField] private QuickSlot _quickslot;
	public QuickSlot QuickSlot { get{ return _quickslot; }  }
	
	[SerializeField] private Interactor _interactor;
	public Interactor Interactor { get{return _interactor; } }

	// ***************** PRIVATE *******************

	public PlayerRecipes PlayerRecipes; 	// convert to prop
	public Transform CameraTarget; 			// convert to prop
	public Transform CameraFocus;			// convert to prop

	private PlayerDataController _dataController;
	

	// *********************************************
	
	public override void Init () {

		base.Init();

		_dataController = new PlayerDataController();

		CreateCameraTarget();
		CameraFocus = Animator.transform;

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
		
		CameraTarget = new GameObject( "CameraTarget" ).transform;
	}
}
