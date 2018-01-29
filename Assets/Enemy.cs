using UnityEngine;
using Interactable.OptionalComponent;

public class Enemy : Creature {

	[SerializeField] private InventoryItem _holdItem;

	private ItemDropper _itemDropper;
	private bool _usingItem;
	private string _itemID = "FUCK";

	public override void Init () {

		_inventory = new Inventory( 15 );
		_quickslotInventory = new QuickSlotInventory( 5 );

		_itemDropper = GetComponent<ItemDropper>();

		var itemInst = Game.ItemManager.RequestItem( _holdItem.name, _itemID );
		var index = _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center );
		_quickslotInventory.SetInventoryItem( index, itemInst );

		base.Init();
	}
	public void Attack () {

		var enemyInteractor = _interactor as EnemyInteractor;

		if ( enemyInteractor != null ) {
			enemyInteractor.Use();
		}
	}

	// ********************************

	protected override void Faint () {

		base.Faint();

		_itemDropper.DropItems();
	}

}
