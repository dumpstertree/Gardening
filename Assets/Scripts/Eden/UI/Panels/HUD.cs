using Dumpster.Core;
using Dumpster.BuiltInModules;
using UnityEngine;
using Dumpster.Characteristics;

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

		[Header( "Zen" )]
		[SerializeField] private Transform _zenBar;
		[SerializeField] private Transform _zenBarFill;

		private Actor _actor {
			get{ return Game.GetModule<Navigation>()?.CurrentArea.LoadedPlayer.GetComponent<Actor>(); }
		}


		private void Update () {

			var zen = Game.GetModule<Eden.Modules.Zen>();
			if ( zen != null ) {
				SizeZenBar( zen.CurrentZen, zen.MaxZen );
			}

			var health = _actor.GetCharacteristic<Health>( true );
			if ( health != null ) {
				SizeHealthBar ( health.Current, health.Max );
			}

			var playerLogic = _actor.GetCharacteristic<PlayerLogic>( true );
			if ( playerLogic != null ) {
				
				if ( playerLogic.CurrentItemInHand.IsShootable ) {

					SetReticleVisible( true );
					
					
					// Is reloading
					if ( playerLogic.CurrentItemInHand.AsShootableItem.IsReloading ) {

						SizeReloadingBar( 0f, 0f );

					
					// Is Not Reloading
					} else {
							
						SetEnergyBarVisible( true );
						SizeEnergyBar( 
							playerLogic.CurrentItemInHand.AsShootableItem.AvailableBullets, 
							playerLogic.CurrentItemInHand.AsShootableItem.ClipSize 
						);
					}

			
				// Not Shootable
				} else {

					SetReticleVisible( false );
					SetEnergyBarVisible( false );
				}

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
		private void SizeZenBar ( int currentZen, int maxZen ) {
			
			var scale = (float)currentZen / (float)maxZen;
			_zenBarFill.transform.localScale = new Vector3( 1f, scale, 1f );
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