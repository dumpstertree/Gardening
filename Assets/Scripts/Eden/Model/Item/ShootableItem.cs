using UnityEngine;
using System;

namespace Eden.Model {
	
	public abstract class ShootableItem : Item {

		protected ShootableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) : base (prefabID, displayName, maxCount, expendable, sprite) {}
		
		
		// ************ Properties *****************

		public abstract Eden.Model.Building.RangedWeapon Gun { 
			get; 
		}

		public int AvailableBullets {
			get{ return _availableBullets; }
		}
		public int ClipSize {
			get{ return _clipSize; }
		}
		public bool IsReloading {
			get{ return _reloading; }
		}



		// ************ Methods *****************

		public void Reload () {

			if ( !_reloading ) {

				_reloading = true;

				EdensGarden.Instance.Async.WaitForSeconds( _reloadSpeed, () => {
					_availableBullets = _clipSize;
					_reloading = false; 
				});
			}
		}
		public void Fire (  Eden.Life.Chips.InteractorChip interactor ) {
			
			// if trying to fire and no bullets reload
			if ( _availableBullets <= 0 ) {
				
				Reload();
				return;
			}

			// if not already firing start
			if ( !_firing ) {

				var fireRate = _rateOfFire;
				
				// create all the bullets
				Action onStart = () => {
					_firing = true;
					for ( int i=0; i<_numOfBullets; i++ ) {  CreateBullet( interactor );  }
				};
				
				// end firing				
				Action onComplete = () => {
					_firing = false;
				};

				EdensGarden.Instance.Async.WaitForSeconds( fireRate, onStart, null, onComplete );
			}
		}
	
		
		// ************ Protected **************
		
		protected override void OnUse ( Eden.Life.Chips.InteractorChip interactor, Action onComplete  ) {

			Fire( interactor );
			onComplete();
		}

		
		// ************ Private **************

		private bool _firing;
		private bool _reloading;
		private int _availableBullets;

		private float _rateOfFire {
			get { return EdensGarden.Instance.StatsForLevel.RateOfFire( Gun.Stats.RateOfFire ); }
		}
		private float _reloadSpeed {
			get { return EdensGarden.Instance.StatsForLevel.ReloadSpeed( Gun.Stats.ReloadSpeed ); }
		}
		private float _accuracy {
			get { return EdensGarden.Instance.StatsForLevel.Accuracy( Gun.Stats.Accuracy ); }
		}
		private int _numOfBullets {
			get { return EdensGarden.Instance.StatsForLevel.NumOfBullets( Gun.Stats.NumOfBullets ); }
		}
		private int _clipSize {
			get { return EdensGarden.Instance.StatsForLevel.ClipSize( Gun.Stats.ClipSize ); }
		}
		private float _bulletSpeed {
			get { return EdensGarden.Instance.StatsForLevel.BulletSpeed( Gun.Stats.BulletSpeed ); }
		}
		private float _bulletSize  {
			get { return EdensGarden.Instance.StatsForLevel.BulletSize( Gun.Stats.BulletSize ); }
		}


		private void CreateBullet (  Eden.Life.Chips.InteractorChip interactor ) {

			var bullet = GameObject.Instantiate( Gun.BulletPrefab );
			var hitData = new HitData();
			hitData.Power = 1;

			bullet.SetBullet( interactor.RangedWeaponChip, hitData, _bulletSize, _bulletSpeed, _accuracy );

			_availableBullets--;
		}	
	}
}