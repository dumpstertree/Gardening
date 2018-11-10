using Eden.Interactors.Ranged;
using UnityEngine;

namespace Eden.Templates {
	
	public class FixedRangedWeapon : Item {
		
		public override Eden.Model.Item CreateInstance () {

			var rangedWeapon = new Eden.Model.Building.RangedWeapon();
			rangedWeapon.StartingStats = _rangedWeaponStats;
			rangedWeapon.BulletPrefab = _bulletPrefab;
			
			return new Eden.Model.FixedRangedWeapon( _id, _displayName, _maxCount, _expendable, _sprite, rangedWeapon );
		}

		[Header( "Stats" )]
		[SerializeField] private Eden.Model.Building.Stats.Gun _rangedWeaponStats;

		[Header( "Bullets" )]
		[SerializeField] private Bullet _bulletPrefab;
	}
}