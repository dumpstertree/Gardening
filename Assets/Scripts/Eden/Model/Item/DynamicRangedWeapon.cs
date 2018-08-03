using UnityEngine;

namespace Eden.Model {

	public class DynamicRangedWeapon : ShootableItem {

		
		// ************* Constructor ****************
		
		public DynamicRangedWeapon( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite, Eden.Model.Building.RangedWeapon rangedWeapon ) : base ( prefabID, displayName, maxCount, expendable, sprite )  {

			SaveRangedWeapon( rangedWeapon );
		}
		

		// ************* Public ****************

		public override Eden.Model.Building.RangedWeapon Gun {
			get{ return LoadRangedWeapon(); }
		}


		// ************* Private ****************
		
		private void SaveRangedWeapon ( Eden.Model.Building.RangedWeapon rangedWeapon ) {
			
			var path = Data.Path.RangedWeapon;
			var fileName = UniqueID;

			EdensGarden.Instance.Data.Save( path, fileName, rangedWeapon );
		}
		private Eden.Model.Building.RangedWeapon LoadRangedWeapon () {

			var path = Data.Path.RangedWeapon;
			var fileName = UniqueID;
			var loadedRangedWeapon = EdensGarden.Instance.Data.Load<Eden.Model.Building.RangedWeapon>( path, fileName ); 

			return loadedRangedWeapon;
		}
	}
}