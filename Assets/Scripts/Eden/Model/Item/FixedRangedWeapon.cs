using UnityEngine;

namespace Eden.Model {
	
	public class FixedRangedWeapon : ShootableItem {
			
		// ************* Constructor ****************
		
		public FixedRangedWeapon( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite, Eden.Model.Building.RangedWeapon rangedWeapon )  : base ( prefabID, displayName, maxCount, expendable, sprite ) {
			
			_rangedWeapon = rangedWeapon;
		}
		

		// ************* Public ****************

		public override Eden.Model.Building.RangedWeapon Gun {
			get{ return _rangedWeapon; }
		}		
		
		
		// ************* Private ****************

		private Eden.Model.Building.RangedWeapon _rangedWeapon;
	}
}
