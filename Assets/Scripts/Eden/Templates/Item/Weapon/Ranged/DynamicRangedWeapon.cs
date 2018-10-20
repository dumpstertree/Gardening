using UnityEngine;

namespace Eden.Templates {
	
	public class DynamicRangedWeapon : Item {
		
		public override Eden.Model.Item CreateInstance () {

			var rangedWeapon = new Eden.Model.Building.RangedWeapon();
			rangedWeapon.StartingStats = _startingStats;

			return new Eden.Model.DynamicRangedWeapon( _id, _displayName, _maxCount, _expendable, _sprite, rangedWeapon );
		}

		[Header( "Stats" )]
		[SerializeField] private Eden.Model.Building.Stats.Gun _startingStats;
	}
}