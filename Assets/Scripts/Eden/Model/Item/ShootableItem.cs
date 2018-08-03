using UnityEngine;
using System;

namespace Eden.Model {
	
	public abstract class ShootableItem : Item {

		protected ShootableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) : base (prefabID, displayName, maxCount, expendable, sprite) {}

		
		// ************ Events *****************

		public delegate void AvailableBulletsChangeEvent( int availableBullets );
		public AvailableBulletsChangeEvent OnAvailableBulletsChange;

		public delegate void ReloadTimeChangedEvent( float currentReloadTime, float maxReloadTime );
		public ReloadTimeChangedEvent OnReloadTimeChanged;
			
		
		// ************ Properties *****************

		public abstract Eden.Model.Building.RangedWeapon Gun { 
			get; 
		}

		public int AvailableBullets {
			get{ return _availableBullets; }
			set{ HandleOnAvailableBulletsChanged( value ); }
		}


		// ************ Methods *****************

		public void Reload () {

			if ( !_reloading ) {

				// start reloading.
				Action onStart = () => {
					_reloading = true;
				};
				
				// alert others that you are reloading.
				Action<float> onWait = t => {

					HandleOnReloadTimeChanged( t, _reloadSpeed );
				};
				
				// tell others you are no longer loading. finish reloading.
				Action onComplete = () => {
					HandleOnReloadTimeChanged( _reloadSpeed, _reloadSpeed );
					AvailableBullets = _clipSize;
					_reloading = false;
				};
					
				EdensGarden.Instance.Async.WaitForSeconds( _reloadSpeed, onStart, onWait, onComplete );
			}
		}
		public void Fire ( Eden.Life.BlackBox user ) {
			
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
					for ( int i=0; i<_numOfBullets; i++ ) {  CreateBullet( user );  }
				};
				
				// end firing				
				Action onComplete = () => {
					_firing = false;
				};

				EdensGarden.Instance.Async.WaitForSeconds( fireRate, onStart, null, onComplete );
			}
		}
	
		
		// ************ Protected **************
		
		protected override void OnUse ( Eden.Life.BlackBox user, Eden.Interactable.InteractableObject interactable, Action onComplete  ) {

			Fire( user );
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


		private void CreateBullet ( Eden.Life.BlackBox user ) {

			var bullet = GameObject.Instantiate( Gun.BulletPrefab );
			var hitData = new HitData();
			hitData.Power = 1;

			bullet.SetBullet( user, hitData, _bulletSize, _bulletSpeed, _accuracy );

			AvailableBullets--;
		}		
		private void HandleOnAvailableBulletsChanged ( int availableBullets ) {
			
			_availableBullets = availableBullets;
			
			if ( OnAvailableBulletsChange != null ) {
				OnAvailableBulletsChange( _availableBullets );
			}
		}
		private void HandleOnReloadTimeChanged ( float currentReloadTime, float maxReloadTime ) {
			
			if ( OnReloadTimeChanged != null ) {
				OnReloadTimeChanged( currentReloadTime, maxReloadTime );
			}
		}
	}
}