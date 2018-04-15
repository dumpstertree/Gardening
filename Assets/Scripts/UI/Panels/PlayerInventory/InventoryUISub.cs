using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels {
	
	public class InventoryUISub : InventoryUI {

		private Player _player {
			get{ return Game.Area.LoadedPlayer; }
		}
		[SerializeField] private GridLayoutGroup _layoutGroup;

		private const int COLLUMNS = 3;
		private const float PADDING = 25;

		protected override Inventory GetInventory () {
			return Game.Area.LoadedPlayer.Inventory;
		}
		protected override ItemBubbleUI[] GetItemBubbles () {

			var itemBubbles = new ItemBubbleUI[ _player.Inventory.InventoryCount ];
			for ( int i = 0; i<itemBubbles.Length; i++  ){

				var itemBubble = Instantiate( _itemBubblePrefab );
				itemBubble.transform.SetParent( _layoutGroup.transform, false );
				itemBubble.Index = i;

				itemBubbles[ i ] = itemBubble;
			}

			return itemBubbles;
		}

		protected void Awake() {

			_layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			_layoutGroup.constraintCount = COLLUMNS;
			_layoutGroup.cellSize = _itemBubblePrefab.GetComponent<RectTransform>().sizeDelta;
			_layoutGroup.spacing = new Vector2( PADDING, PADDING );
		}
	}
}