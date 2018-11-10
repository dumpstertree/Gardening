using UnityEngine;

namespace Dumpster.Core {
	
	public struct Line {

		public Vector3 Point1 { get; }
		public Vector3 Point2 { get; }
		public Vector3 Direction { get; }
		public float Length { get; }

		public Line ( Vector3 point1, Vector3 point2 ) {

			Point1 = point1;
			Point2 = point2;
			Direction = (point1 - point2).normalized;
			Length = Vector3.Distance( point1, point2 );
		}
	}
}