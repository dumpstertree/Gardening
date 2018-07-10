using UnityEngine;
using Eden.Model;

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
		[SerializeField] private Transform _reticle;

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

		private Item _item;

		private void OnQuickslotIndexChanged( int index ) {

			if ( _item != null && _item.IsShootable ) {
				_item.AsShootableItem.OnAvailableBulletsChange -= HandleAvailableBulletsChanged;
				_item.AsShootableItem.OnReloadTimeChanged -= HandleReloadTimeChanged;
			}

			_item = _blackBox.EquipedItems.GetInventoryItem( index );

			if ( _item != null && _item.IsShootable ) {
				_item.AsShootableItem.OnAvailableBulletsChange += HandleAvailableBulletsChanged;
				_item.AsShootableItem.OnReloadTimeChanged += HandleReloadTimeChanged;
			} else {
				_energyFill.transform.localScale = Vector3.zero;
			}
		}
		private void HandleAvailableBulletsChanged ( int numOfBullets ) {

			_energyFill.transform.localScale =  new Vector3( 1, (float)numOfBullets / (float)_item.AsShootableItem.Gun.Stats.ClipSize, 1 );
		}
		private void HandleReloadTimeChanged ( float currentReloadTime, float maxReloadTime ) {

			_energyFill.transform.localScale =  new Vector3( 1, currentReloadTime / maxReloadTime, 1 );
		}

		[SerializeField] private LayerMask _layerMask;
		private void Update () {

			_reticle.gameObject.SetActive( _item != null && _item.IsShootable && _blackBox.QuickslotChip.ItemIsEquiped );

			if ( _item != null && _item.IsShootable ) {
				
				RaycastHit hit;
				if (Physics.Raycast( _blackBox.ProjectileSpawner.position, _blackBox.ProjectileSpawner.forward, out hit, Mathf.Infinity, _layerMask )) {
        		
        			// _reticle.position = Vector3.Lerp( _reticle.position, Camera.main.WorldToScreenPoint( hit.point ), 0.2f );
        			_reticle.position = new Vector3( Screen.width/2f, Screen.height/2f, 0 );
        		}
			}
        	
		}
	}
}