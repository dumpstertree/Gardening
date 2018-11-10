using Eden.Interactors.Melee;
using UnityEngine;

namespace Eden.Templates {
	
	public class MeleeWeapon : Item {
		
		public override Eden.Model.Item CreateInstance () {

			return new Eden.Model.MeleeWeapon( _id, _displayName, _maxCount, _expendable, _sprite, _swingPrefabs );
		}

		[Header( "Prefab" )]
		[SerializeField] private Slash[] _swingPrefabs;
	}
}