using UnityEngine;
using System;
using Eden.Model.Building.Stats;

namespace Eden.Model {
	
	public abstract class ShootableItem : Item {

		protected ShootableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite ) : base (prefabID, displayName, maxCount, expendable, sprite) {}

		
		// ************ Public *****************

		public delegate void AvailableBulletsChangeEvent( int availableBullets );
		public AvailableBulletsChangeEvent OnAvailableBulletsChange;

		public delegate void ReloadTimeChangedEvent( float currentReloadTime, float maxReloadTime );
		public ReloadTimeChangedEvent OnReloadTimeChanged;
		
		public int AvailableBullets {
			get{ return _availableBullets; }
			set{ HandleOnAvailableBulletsChanged( value ); }
		}

		public void Reload () {

			if ( !_reloading ) {

				var reloadTime = Stats.ReloadSpeed;

				// start reloading.
				Action onStart = () => {
					_reloading = true;
				};
				
				// alert others that you are reloading.
				Action<float> onWait = t => {
					HandleOnReloadTimeChanged( t, reloadTime );
				};
				
				// tell others you are no longer loading. finish reloading.
				Action onComplete = () => {
					HandleOnReloadTimeChanged( reloadTime, reloadTime );
					AvailableBullets = 5;
					_reloading = false;
				};
					
				EdensGarden.Instance.Async.WaitForSeconds( reloadTime, onStart, onWait, onComplete );
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

				var fireRate = 1f / Stats.RateOfFire;
				var numOfBullets = 1;
				
				// create all the bullets
				Action onStart = () => {
					_firing = true;
					for ( int i=0; i<numOfBullets; i++ ) { 
						CreateBullet( user ); 
					}
				};
				
				// end firing				
				Action onComplete = () => {
					_firing = false;
				};

				EdensGarden.Instance.Async.WaitForSeconds( fireRate, onStart, null, onComplete );
			}
		}
		public abstract Gun Stats {
			get; set;
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

		private GameObject _bulletPrefab {
			get { 
				return Resources.Load<GameObject>( "Bullet" ); 
			}
		}
		private void CreateBullet ( Eden.Life.BlackBox user ) {

			var go = GameObject.Instantiate( _bulletPrefab );
		
			var hitData = new HitData();
			hitData.Power = 1;
			go.GetComponent<Bullet>().SetBullet( user, hitData );

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

	public class FixedShootableItem : ShootableItem {

		// ************* Constructor ****************
		
		public FixedShootableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite, Gun stats ) : base (prefabID, displayName, maxCount, expendable, sprite)  {

			_stats = stats;
		}

		public override Gun Stats {
			get{ return _stats; }
			set{ _stats = value; }
		}

		// ************* Private ****************

		private Gun _stats;

	
	}
}