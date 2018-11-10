using UnityEngine;

namespace Eden.Model.Building.Parts {
	
	[System.Serializable]
	public class Gun: Part<Eden.Model.Building.Stats.Gun> {

		public Gun ( char[,] blocks, Eden.Model.Building.Stats.Gun stats, string prefabID ) : base( blocks, stats, prefabID ) {

			var r = Random.Range( 0, 3);

			if( r == 0 ) {
				_blocks = new char[3,3] {
					
					{ 'o','x','⇢' },
					{ 'x',' ',' ' },
					{ '∩',' ',' ' }
				};
			}
			if( r == 1 ) {
				_blocks = new char[3,3] {
					
					{ '⊃','o','⇢' },
					{ ' ',' ',' '},
					{ ' ',' ',' ' }
				};
			}
			if( r == 2 ) {
				_blocks = new char[3,3] {
					
					{ 'x','∪','x' },
					{ '⇣','o','⇣' },
					{ ' ',' ',' ' }
				};
			}
		}
	}
}