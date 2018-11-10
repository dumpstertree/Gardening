using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {

	public class EquippedItemsInventory : Dumpster.Core.Characteristic {

		public Eden.Model.Item DefaultEquipedItem {
			get{ return _defaultItem; }
		}
		public int NumOfItem {
			get { return _numOfItems; }
		}
		public Eden.Controller.Inventory Inventory {
			get{ return _inventory; }
		}

		[SerializeField] private int _numOfItems;
		[SerializeField] private Eden.Templates.Item[] _items;
		[SerializeField] private Eden.Templates.Item _defaultEquipedItem;

		private Eden.Controller.Inventory _inventory;
		private Eden.Model.Item _defaultItem;

		protected override void OnInit () {

			_inventory = new Eden.Controller.Inventory( _numOfItems );
			
			if ( _numOfItems > _items.Length ) {

				foreach( Eden.Templates.Item item in _items ) {
				
					_inventory.AddInventoryItem( item.CreateInstance() );
				}
			}

			_defaultItem = _defaultEquipedItem.CreateInstance();
		}
	}
}