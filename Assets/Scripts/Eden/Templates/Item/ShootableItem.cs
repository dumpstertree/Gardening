using UnityEngine;

namespace Eden.Templates {
	
	public class ShootableItem : Item {
		
		public Eden.Model.FixedShootableItem CreateInstance () {
			
			return new Eden.Model.FixedShootableItem( _id, _displayName, _maxCount, _expendable, _sprite, _startingStats );
		}

		[Header( "Stats" )]
		[SerializeField] private Eden.Model.Building.Stats.Gun _startingStats;
	}
}