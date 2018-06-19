using System;
using UnityEngine;

namespace Controller.Item {

	public class ShootData {
		
		// ************* EVENTS ****************

		public delegate void AvailableBulletsChangeEvent( int availableBullets );
		public AvailableBulletsChangeEvent OnAvailableBulletsChange;

		public delegate void ReloadTimeChangedEvent( float currentReloadTime, float maxReloadTime );
		public ReloadTimeChangedEvent OnReloadTimeChanged;
		
		
		// ************ PUBLIC *****************

		public Model.Gun Gun {
			get{ return Game.GunControl.GetGun( _gunGUIDToLookup ); }
		}
		public int AvailableBullets {
			get{ return _availableBullets; }
			set{ HandleOnAvailableBulletsChanged( value ); }
		}

		public void SetGunGUIDToLookup ( string guid ) {
			_gunGUIDToLookup = guid;
		}
		public void SetBulletPrefab ( GameObject prefab ) {
			_bulletPrefab = prefab;
		}

		public void Reload () {
			
			Debug.Log( "reload" );

			if ( !_reloading ) {

				var reloadTime = Gun.WeaponStats.ReloadTime;

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
					AvailableBullets = Gun.WeaponStats.ClipSize;
					_reloading = false;
				};

				Debug.Log( "waiting for " + reloadTime );
				EdensGarden.Instance.Async.WaitForSeconds( reloadTime, onStart, onWait, onComplete );
			}
		}
		public void Fire ( Eden.Life.BlackBox user ) {

			Debug.Log( "try to fire" );
			// if trying to fire and no bullets reload
			if ( _availableBullets <= 0 ) {
				
				Reload();
				return;
			}

			// if not already firing start
			if ( !_firing ) {

				Debug.Log( "fire" );

				var fireRate = 1f / Gun.WeaponStats.FireRate;
				var numOfBullets = Gun.WeaponStats.NumberOfBullets;
				
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


		// ************ PRIVATE **************

		private string _gunGUIDToLookup; // curently unused
		private bool _firing;
		private bool _reloading;
		private int _availableBullets;

		private GameObject _bulletPrefab; // this should be moved to be pulled from the gun

		private void CreateBullet ( Eden.Life.BlackBox user ) {

			var go = GameObject.Instantiate( _bulletPrefab );
			var bulletSpread = 0;
			var spreadR = UnityEngine.Random.Range( -bulletSpread, bulletSpread);
			var spreadU = UnityEngine.Random.Range( -bulletSpread, bulletSpread);
		
			go.transform.position = user.ProjectileSpawner.transform.position;
			go.transform.rotation = user.ProjectileSpawner.transform.rotation;
			go.transform.rotation = go.transform.rotation * Quaternion.AngleAxis( spreadR, go.transform.right );
			go.transform.rotation = go.transform.rotation * Quaternion.AngleAxis( spreadU, go.transform.up );

			var hitData = new HitData();
			hitData.Power = 1;
			go.GetComponent<Bullet>().SetBullet( user, hitData);

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