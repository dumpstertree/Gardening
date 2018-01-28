using UnityEngine;
using Interactable.OptionalComponent;

public class Enemy : Creature {

	[SerializeField] private InventoryItem _holdItem;

	private ItemDropper _itemDropper;
	private bool _usingItem;


	public override void Init () {

		_inventory = new Inventory( 15 );
		_quickslotInventory = new QuickSlotInventory( 5 );

		_itemDropper = GetComponent<ItemDropper>();

		var itemInst = Game.ItemManager.RequestItem( _holdItem.name );
		var index = _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center );
		_quickslotInventory.SetInventoryItem( index, itemInst );

		base.Init();
	}
	public void Attack () {

		if ( !_usingItem ) {

			var index = _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center );
			var item = _quickslotInventory.GetInventoryItem( index );

			item.Use( this, () => { _usingItem = false; } );
		}
	}

	// ********************************

	protected override void Start () {

		base.Start();

		Init();
	}
	protected override void Faint () {

		base.Faint();

		_itemDropper.DropItems();
	}

}
