using System;

namespace Dumpster.Core {

	public static class ArrayMath {

		static public T[] ShiftArrayLeft<T> ( T[] source ) {

			var destination = new T[ source.Length ]; 
			Array.Copy( source, 1, destination, 0, source.Length - 1);
			return destination;
		}

		static public T[] ShiftArrayRight<T> ( T[] source ) {

			var destination = new T[ source.Length ]; 
			Array.Copy( source, 0, destination, 1, source.Length - 1);
			return destination;
		}

		static public void Shuffle<T> ( T[] source  ) {
			
			for ( int i=0; i<source.Length; i++ ) {

				var targetIndex = UnityEngine.Random.Range( 0, source.Length );
				
				var ref1 = source[ i ];
				var ref2 = source[ targetIndex ];

				source[ i ] = ref2;
				source[ targetIndex ] = ref1;
			}
		}
		static public T[] Shuffled<T> ( T[] source  ) {
			
			Shuffle( source );
			return source;
		}
	}
}