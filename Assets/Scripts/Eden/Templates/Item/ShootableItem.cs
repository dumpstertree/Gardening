using System.Collections.Generic;
using UnityEngine;

namespace Eden.Templates {
	
	public class ShootableItem : Item {
		
		public override Eden.Model.Item CreateInstance () {

			var gun = new Gun2( new List<Part2>(), _startingStats );
			return new Eden.Model.FixedShootableItem( _id, _displayName, _maxCount, _expendable, _sprite, gun );
		}

		[Header( "Stats" )]
		[SerializeField] private Eden.Model.Building.Stats.Gun _startingStats;
	}
}