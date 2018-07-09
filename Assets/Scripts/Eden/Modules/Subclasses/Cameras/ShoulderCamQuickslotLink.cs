using UnityEngine;
using Eden.Model;

namespace Eden.Cameras {

	public class ShoulderCamQuickslotLink : Dumpster.Core.BuiltInModules.ShoulderCameraController {

		[SerializeField] private Eden.Life.BlackBox _blackBox;

		private Item _item;
		private void Start () {
			
			_blackBox.QuickslotChip.OnInputChanged += index => {
				
				var item = _blackBox.EquipedItems.GetInventoryItem( index );

				if ( item != null ) {
					if ( _item == null ) {
						if( item.IsShootable ) { FireRequestEvent (); }
					} else {
						if ( item.IsShootable && !_item.IsShootable ) { FireRequestEvent (); }
						if ( !item.IsShootable && _item.IsShootable ) { FireRelinquishEvent(); }
					}
				} else {
					if ( _item != null && _item.IsShootable ) { FireRelinquishEvent (); }
				}

				_item = item;
			};
		}
	}
}