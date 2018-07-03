using UnityEngine;

namespace Eden.Model.Building.Parts {
	
	public class Gun: Part<Eden.Model.Building.Stats.Gun> {

		public Gun ( string name, char[,] blocks, Eden.Model.Building.Stats.Gun stats ) : base( name, blocks, stats ) {

			BuilderStats = new Eden.Model.Building.Stats.Gun( 
							   Random.Range( 1, 10 ),
							   Random.Range( 0, 2 ),
							   Random.Range( 0, 2 ),
							   Random.Range( 0, 2 ),
							   Random.Range( 0, 2 ),
							   Random.Range( 0, 2 ),
							   Random.Range( 0, 2 ));
			

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