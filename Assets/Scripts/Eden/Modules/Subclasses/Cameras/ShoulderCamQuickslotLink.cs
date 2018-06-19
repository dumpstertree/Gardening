using UnityEngine;
namespace Eden.Cameras {

	public class ShoulderCamQuickslotLink : Dumpster.Core.BuiltInModules.ShoulderCameraController {

		[SerializeField] private Eden.Life.BlackBox _blackBox;

		private InventoryItem _item;
		private void Start () {
			
			_blackBox.QuickslotChip.OnInputChanged += index => {
				
				var item = _blackBox.EquipedItems.GetInventoryItem( index );

				if ( item != null ) {
					if ( _item == null ) {
						if( item.CanShoot ) { FireRequestEvent (); }
					} else {
						if ( item.CanShoot && !_item.CanShoot ) { FireRequestEvent (); }
						if ( !item.CanShoot && _item.CanShoot ) { FireRelinquishEvent(); }
					}
				} else {
					if ( _item != null && _item.CanShoot ) { FireRelinquishEvent (); }
				}

				_item = item;
			};
		}
	}
}