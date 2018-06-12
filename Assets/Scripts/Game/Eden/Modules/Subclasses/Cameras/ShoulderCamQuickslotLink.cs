namespace Eden.Cameras {

	public class ShoulderCamQuickslotLink : Dumpster.Core.BuiltInModules.ShoulderCameraController {

		private Player _player {
			get { return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Player>(); }
		}

		private InventoryItem _item;
		private void Start () {
			
			_player.QuickSlot.OnInputChanged += id => {
				
				var index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( id );
				var item = _player.QuickslotInventory.GetInventoryItem( index );

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