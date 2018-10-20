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
	}
}