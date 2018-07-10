using UnityEngine;

namespace Eden.Model.Building.Parts {
	
	[System.Serializable]
	public class Gun: Part<Eden.Model.Building.Stats.Gun> {

		public Gun ( char[,] blocks, Eden.Model.Building.Stats.Gun stats ) : base( blocks, stats ) {

			var r = Random.Range( 0, 3);

			if( r == 0 ) {
				Blocks = new char[3,3] {
					
					{ 'o','x','⇢' },
					{ 'x',' ',' ' },
					{ '∩',' ',' ' }
				};
			}
			if( r == 1 ) {
				Blocks = new char[3,3] {
					
					{ '⊃','o','⇢' },
					{ ' ',' ',' '},
					{ ' ',' ',' ' }
				};
			}
			if( r == 2 ) {
				Blocks = new char[3,3] {
					
					{ 'x','∪','x' },
					{ '⇣','o','⇣' },
					{ ' ',' ',' ' }
				};
			}
		}
	}
}