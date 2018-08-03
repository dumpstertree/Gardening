using UnityEngine;
using System.Collections.Generic;
using Eden.Interactors;

namespace Eden.Model.Building {
	
	[System.Serializable]	
	public class RangedWeapon {

		public RangedWeapon () {
			
			StartingStats = new Stats.Gun();
			ModularParts = new List<Parts.Gun>();
		}
		

		public Bullet BulletPrefab {
			get{ return _bulletPrefab; }
			set{ _bulletPrefab = value; }
		}
		public Stats.Gun StartingStats {
			get{ return _startingStats; }
			set{ _startingStats = value; }
		}
		public List<Parts.Gun> ModularParts {
			get{ return new List<Parts.Gun>( _modularParts ); }
			set{ _modularParts = value.ToArray(); }
		}
		public Stats.Gun Stats {
			get {
				var s = _startingStats;
				foreach( Parts.Gun p in ModularParts ) { s.Add( p.Stats ); }
				return s;
			}
		}

		[SerializeField] private Stats.Gun _startingStats;
		[SerializeField] private Parts.Gun[] _modularParts;

		private Bullet _bulletPrefab;
	}
}