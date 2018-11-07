using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eden.Characteristics {

	public class Inventory : Dumpster.Core.Characteristic {

		public Eden.Controller.Inventory Inv {
			get{ return _inventory; }
		}

		[SerializeField] private int _numOfItems;
		[SerializeField] private Eden.Templates.Item[] _items;


		private Eden.Controller.Inventory _inventory;

		protected override void OnInit () {

			_inventory = new Eden.Controller.Inventory( _numOfItems );
			
			if ( _numOfItems > _items.Length ) {

				foreach( Eden.Templates.Item item in _items ) {
				
					_inventory.AddInventoryItem( item.CreateInstance() );
				}
			}
		}
	}
}