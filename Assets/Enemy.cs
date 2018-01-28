using UnityEngine;
using Interactable.OptionalComponent;

public class Enemy : Creature {

	[SerializeField] private InventoryItem _topSlotItem;
	[SerializeField] private InventoryItem _rightSlotItem;
	[SerializeField] private InventoryItem _bottomSlotItem;
	[SerializeField] private InventoryItem _leftSlotItem;
	[SerializeField] private InventoryItem _centerSlotItem;
	[SerializeField] private QuickSlotInventory _quickSlotInventory;

	private ItemDropper _itemDropper;

	public override void Init () {

		_itemDropper = GetComponent<ItemDropper>();

		base.Init();
	}

	// ********************************

	protected override void Awake () {

		base.Awake();

		Init();
	}
	protected override void Faint () {

		base.Faint();

		_itemDropper.DropItems();
	}
}
