using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels {
	
	public class QuickSlot: InventoryUI {

		[SerializeField] private Image _selection;
		[SerializeField] private ItemBubbleUI _topItem;
		[SerializeField] private ItemBubbleUI _rightItem;
		[SerializeField] private ItemBubbleUI _bottomItem;
		[SerializeField] private ItemBubbleUI _leftItem;
		[SerializeField] private ItemBubbleUI _centerItem;

		private QuickSlotInventory.ID _id;

		private Player _player {
			get{ return Game.Area.LoadedPlayer; }
		}
		
		// ********************************

		protected override void OnPresent () {
			
			base.OnPresent();
			
			Game.Area.LoadedPlayer.QuickSlot.OnInputChanged += ( newID ) => {
				_id = newID;
			};
		}

		// ********************************

		protected override Inventory GetInventory () {

			return Game.Area.LoadedPlayer.QuickslotInventory;	
		}
		protected override ItemBubbleUI[] GetItemBubbles () {

			var itemBubbles = new ItemBubbleUI[ _player.QuickslotInventory.InventoryCount ];

			_topItem.Index    = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Top );
			_rightItem.Index  = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Right );
			_bottomItem.Index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Bottom );
			_leftItem.Index   = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Left );
			_centerItem.Index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center );

			itemBubbles[ _topItem.Index ]    = _topItem;
			itemBubbles[ _rightItem.Index ]  = _rightItem;
			itemBubbles[ _bottomItem.Index ] = _bottomItem;
			itemBubbles[ _leftItem.Index ]   = _leftItem;
			itemBubbles[ _centerItem.Index ] = _centerItem;

			return itemBubbles;
		}

		// ********************************

		private void Update () {
			
			switch( _id ) {
				
				case QuickSlotInventory.ID.Top:
					_selection.transform.position = Vector3.Lerp( _selection.transform.position, _topItem.transform.position, 0.5f );
					break;
				case QuickSlotInventory.ID.Right:
					_selection.transform.position = Vector3.Lerp( _selection.transform.position, _rightItem.transform.position, 0.5f );
					break;
				case QuickSlotInventory.ID.Bottom:
					_selection.transform.position = Vector3.Lerp( _selection.transform.position, _bottomItem.transform.position, 0.5f );
					break;
				case QuickSlotInventory.ID.Left:
					_selection.transform.position = Vector3.Lerp( _selection.transform.position, _leftItem.transform.position, 0.5f );
					break;
				case QuickSlotInventory.ID.Center:
					_selection.transform.position = Vector3.Lerp( _selection.transform.position, _centerItem.transform.position, 0.5f );
					break;
			}
		}
	}
}
