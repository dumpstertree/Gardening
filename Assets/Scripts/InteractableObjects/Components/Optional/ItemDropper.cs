using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Interactable.OptionalComponent {
	
	public class ItemDropper : MonoBehaviour {

		[SerializeField] private List<ItemTemplate> _dropItems;
		[SerializeField] private DropItem _dropItemPrefab;

		private const string ITEMS_NAMESPACE = "Items.";

		public void DropItems (){

			var rolledItem = Roll();

			if ( rolledItem != null){
				CreateDropItem(  rolledItem );
			}
		}
		private InventoryItem Roll () {

			InventoryItem rolledItem = null;
			//var itemList = new List<InventoryItem>();
			var roll = UnityEngine.Random.Range(0.0f,1f);
			var maxSuccess = 0.0f;

			foreach (ItemTemplate i in _dropItems){
				maxSuccess += i.SpawnRate;
			}

			if (roll <= maxSuccess){

				// Get Dropped Item
				var r = 0.0f;
				ItemTemplate droppedItem = new ItemTemplate();
				foreach ( ItemTemplate i in _dropItems ){
					r += i.SpawnRate; 
					if ( roll <= r ){
						droppedItem = i;
						break;
					}
				}

				// Get Quantity
				var quantityRand = UnityEngine.Random.Range(0.0f,1f);
				var quantityCurve = quantityRand*quantityRand;
				var itemDelta = droppedItem.MaxDropQuantity-droppedItem.MinDropQuantity;
				var q = (int)(droppedItem.MinDropQuantity + Mathf.Floor( quantityCurve*itemDelta ) );

				// create item
				var inventoryItem = ScriptableObject.Instantiate( droppedItem.Item );

				// return item
				inventoryItem.SetCount( q );
				rolledItem = inventoryItem;
			}

			return rolledItem;
		}
		private void CreateDropItem ( InventoryItem item  ) {
			
			var drop = Instantiate( _dropItemPrefab );
			drop.transform.position = transform.position;
			drop.transform.rotation = transform.rotation;
			drop.SetItem( item );
		}

		[System.Serializable]
		public struct ItemTemplate {

			public float SpawnRate;
			public float MinDropQuantity;
			public float MaxDropQuantity;
			public InventoryItem Item;
		}

	}
}