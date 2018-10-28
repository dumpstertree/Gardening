using Dumpster.Core;
using Dumpster.BuiltInModules;
using UnityEngine;
using Eden.Life;

namespace Eden.UI.Panels {

	public class HUD : Dumpster.BuiltInModules.Panel {

		[Header( "Energy" )]
		[SerializeField] private Transform _energyBar;
		[SerializeField] private Transform _energyBarFill;
		
		[Header( "Health" )]
		[SerializeField] private Transform _healthBar;
		[SerializeField] private Transform _healthBarFill;

		[Header( "Reticle" )]
		[SerializeField] private Transform _reticle;

		
		private BlackBox _blackBox {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<BlackBox>(); }
		}


		private void Update () {

			var visual = _blackBox.Visual;

			SizeHealthBar( visual.CurrentHealth, visual.MaxHealth );


			// Is Shootable
			if ( visual.CurrentItemInHand.IsShootable ) {

				SetReticleVisible( true );
				
				
				// Is reloading
				if ( visual.CurrentItemInHand.AsShootableItem.IsReloading ) {

					SizeReloadingBar( 0f, 0f );

				
				// Is Not Reloading
				} else {
						
					SetEnergyBarVisible( true );
					SizeEnergyBar( 
						visual.CurrentItemInHand.AsShootableItem.AvailableBullets, 
						visual.CurrentItemInHand.AsShootableItem.ClipSize 
					);
				}

		
			// Not Shootable
			} else {

				SetReticleVisible( false );
				SetEnergyBarVisible( false );
			}
		}


		private void SizeHealthBar ( int currentHealth, int maxHealth ) {
			
			var scale = (float)currentHealth / (float)maxHealth;
			_healthBarFill.transform.localScale = new Vector3( 1f, scale, 1f );
		}
		private void SizeEnergyBar ( int currentEnergy, int maxEnergy ) {

			var scale = (float)currentEnergy / (float)maxEnergy;
			_energyBarFill.transform.localScale = new Vector3( 1f, scale, 1f );
		}
		private void SizeReloadingBar ( float currentReload, float maxReload ) {
		}


		private void SetHealthBarVisible ( bool visible ) {

			_healthBar.gameObject.SetActive( visible );
		}
		private void SetEnergyBarVisible ( bool visible ) {

			_energyBar.gameObject.SetActive( visible );
		}
		private void SetReticleVisible ( bool visible ) {

			_reticle.gameObject.SetActive( visible );
		}
	}
}