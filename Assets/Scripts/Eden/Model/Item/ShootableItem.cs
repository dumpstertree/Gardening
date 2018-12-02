using Dumpster.Core;
using Dumpster.Core.BuiltInModules;
using Eden.Characteristics;
using Eden.Modules;
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

				Game.GetModule<Async>()?.WaitForSeconds( _reloadSpeed, () => {
					_availableBullets = _clipSize;
					_reloading = false; 
				});
			}
		}
		public void Fire ( Actor actor ) {
			
			// if trying to fire and no bullets reload
			if ( _availableBullets <= 0 ) {
				
				Reload();
				return;
			}

			// if not already firing start
			if ( !_firing ) {
				
				_firing = true;
				
				actor.GetCharacteristic<CanUseRangedWeapons>().Animate( () => {
					
					// create all the bullets
					foreach( CanUseRangedWeapons r in actor.GetCharacteristics<CanUseRangedWeapons>() ){
						for ( int i=0; i<_numOfBullets; i++ ) {  CreateBullet( r );  }
					}
					
					Game.GetModule<Async>()?.WaitForSeconds( _rateOfFire, () => _firing = false );
				});
			}
		}
	
		
		// ************ Protected **************
		
		protected override void OnUse ( Actor actor, Action onComplete  ) {

			var ranged = actor.GetCharacteristic<CanUseRangedWeapons>( true );
			
			if ( ranged != null ) {
				Fire( actor );
				onComplete();
			} else {
				onComplete();
			}
		}

		
		// ************ Private **************

		private bool _firing;
		private bool _reloading;
		private int _availableBullets;

		private float _rateOfFire {
			get { return Game.GetModule<Eden.Modules.Constants>().RangedWeapons.RateOfFire( Gun.Stats.RateOfFire ); }
		}
		private float _reloadSpeed {
			get { return Game.GetModule<Eden.Modules.Constants>().RangedWeapons.ReloadSpeed( Gun.Stats.ReloadSpeed ); }
		}
		private float _accuracy {
			get { return Game.GetModule<Eden.Modules.Constants>().RangedWeapons.Accuracy( Gun.Stats.Accuracy ); }
		}
		private int _numOfBullets {
			get { return Game.GetModule<Eden.Modules.Constants>().RangedWeapons.NumOfBullets( Gun.Stats.NumOfBullets ); }
		}
		private int _clipSize {
			get { return Game.GetModule<Eden.Modules.Constants>().RangedWeapons.ClipSize( Gun.Stats.ClipSize ); }
		}
		private float _bulletSpeed {
			get { return Game.GetModule<Eden.Modules.Constants>().RangedWeapons.BulletSpeed( Gun.Stats.BulletSpeed ); }
		}
		private float _bulletSize  {
			get { return  Game.GetModule<Eden.Modules.Constants>().RangedWeapons.BulletSize( Gun.Stats.BulletSize ); }
		}


		private void CreateBullet ( CanUseRangedWeapons ranged ) {

			var bullet = GameObject.Instantiate( Gun.BulletPrefab );

			bullet.SetBullet( ranged, _bulletSize, _bulletSpeed, _accuracy );

			_availableBullets--;
		}	
	}
}