using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels {

	public class Ammo : UiPanel {

		[SerializeField] private Text _availableAmmo;
		[SerializeField] private Text _maxAmmo;
		[SerializeField] private Image _reloadTime;

		private InventoryItem _item;
		private GunRef _gunRef;

		private Player _player {
			get{ return Game.Area.LoadedPlayer; }
		}


		protected override void OnInit () {

			_player.QuickSlot.OnInputChanged += newId => { 

				var index = _player.QuickslotInventory.ConvertQuickSlotIDToIndex( newId );
				var item = _player.QuickslotInventory.GetInventoryItem( index );

				// remove all old connectionss
				if ( _item != null ) {

					var id = _item.ID;
					var gunRef = Game.GunControl.GetGun( id );

					gunRef.OnAvailableBulletsChange -= newCount => { UpdateAvailableAmmo( newCount ); };
					gunRef.OnReloadTimeChanged -= (currentReloadTime, reloadTime ) => { UpdateReloadTime( currentReloadTime, reloadTime ); };
				}


				_item = null;
				_gunRef = null;

				if ( item != null && item.CanShoot ) {
					
					var id = item.ID;
					var gunRef = Game.GunControl.GetGun( id );

					_item = item;
					_gunRef = gunRef;


					// update available ammo
					UpdateAvailableAmmo( gunRef.AvailableBullets );
					gunRef.OnAvailableBulletsChange += newCount => { UpdateAvailableAmmo( newCount ); };
					gunRef.OnReloadTimeChanged += (currentReloadTime, reloadTime ) => { UpdateReloadTime( currentReloadTime, reloadTime ); };

					// update max ammo
					var gunStats = new GunDataController().LoadGun( _item.ID ).WeaponStats;
					UpdateMaxAmmo( gunStats.ClipSize );
				} 
				else {

					// hide all ammo
					Hide();
				}
			};
		}
		
		private void UpdateMaxAmmo ( int amount ) {

			var gunStats = new GunDataController().LoadGun( _item.ID ).WeaponStats;

			_maxAmmo.color = new Color( 1,1,1,1 );
			_maxAmmo.text = gunStats.ClipSize.ToString();
		}
		private void UpdateAvailableAmmo ( int amount ) {
			
			_availableAmmo.color = new Color( 1,1,1,1 );
			_availableAmmo.text = _gunRef.AvailableBullets.ToString();
		}
		private void UpdateReloadTime ( float currentReloadTime, float reloadTime ) {

			_reloadTime.fillAmount = currentReloadTime/reloadTime;
		}
		private void Hide(){
			
			_availableAmmo.color = new Color( 1,1,1,0 );
			_maxAmmo.color = new Color( 1,1,1,0 );
		}
	}
}
