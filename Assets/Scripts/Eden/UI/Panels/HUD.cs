using UnityEngine;

namespace Eden.UI.Panels {

	public class HUD : Dumpster.Core.BuiltInModules.UI.Panel {

		private Eden.Life.BlackBox _blackBox {
			get{ return EdensGarden.Instance.Rooms.CurrentArea.LoadedPlayer.GetComponent<Eden.Life.BlackBox>(); }
		}
		private Interactable.Stats _stats {
			get { return _blackBox.GetComponentInChildren<Interactable.Stats>(); }
		}

		[SerializeField] private Transform _healthFill;
		[SerializeField] private Transform _energyFill;

		protected override void OnInit () {

			_stats.OnHealthChanged += HandleHealthChange;
			HandleHealthChange( _stats.CurrentHealth );

			_blackBox.QuickslotChip.OnIndexChanged += OnQuickslotIndexChanged;
			OnQuickslotIndexChanged( _blackBox.QuickslotChip.Index );
		}

		
		// ******************** Health ****************

		private void HandleHealthChange( int currentHealth ) {

			_healthFill.transform.localScale = new Vector3( 1, (float)currentHealth/ (float)_stats.MaxHealth, 1 );
		}


		// ******************** Energy ****************

		private InventoryItem _item;

		private void OnQuickslotIndexChanged( int index ) {

			if ( _item != null ) {
				_item._shootData.OnAvailableBulletsChange -= HandleAvailableBulletsChanged;
				_item._shootData.OnReloadTimeChanged -= HandleReloadTimeChanged;
			}

			_item = _blackBox.EquipedItems.GetInventoryItem( index );

			if ( _item != null && _item.CanShoot ) {
				_item._shootData.OnAvailableBulletsChange += HandleAvailableBulletsChanged;
				_item._shootData.OnReloadTimeChanged += HandleReloadTimeChanged;
			} else {
				_energyFill.transform.localScale = Vector3.zero;
			}
		}
		private void HandleAvailableBulletsChanged ( int numOfBullets ) {

			_energyFill.transform.localScale =  new Vector3( 1, (float)numOfBullets / (float)_item._shootData.Gun.WeaponStats.ClipSize, 1 );
		}
		private void HandleReloadTimeChanged ( float currentReloadTime, float maxReloadTime ) {

			_energyFill.transform.localScale =  new Vector3( 1, currentReloadTime / maxReloadTime, 1 );
		}
	}
}