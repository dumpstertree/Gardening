using UnityEngine;
using Dumpster.Core;

public class Path : MonoBehaviour {

	[Range( 0, 1)]
	[SerializeField] private float _progress;


	public Point[] Points {
		get { return _points; }
	}

	public Line GetLine ( float progress ) {

		_progress = progress;

		var point1 = _points[ 0 ];
		var point2 = _points[ 1 ];

		var location1Position = Vector3.Lerp( 
			point1.Location1, 
			point2.Location1, 
			progress 
		);

		var location2Position = Vector3.Lerp( 
			point1.Location2, 
			point2.Location2, 
			progress 
		);

		return new Line( location1Position, location2Position );
	}

	private void OnDrawGizmos () {

		if ( Points.Length < 2 ) {
			return;
		}

		Gizmos.color = Color.red;
		
		var lastP1Location = Points[ 0 ].Location1;
		foreach ( Point p in Points ) {

			var location = p.Location1;
			Gizmos.DrawLine( lastP1Location, location );
			lastP1Location = p.Location1;
		}

		Gizmos.color = Color.blue;
		
		var lastP2Location = Points[ 0 ].Location2;
		foreach ( Point p in Points ) {

			var location = p.Location2;
			Gizmos.DrawLine( lastP2Location, location );
			lastP2Location = p.Location2;
		}


		Gizmos.color = Color.gray;
		
		var line = GetLine( _progress );
		Gizmos.DrawRay( line.Point1, -line.Direction * line.Length );
		Gizmos.DrawRay( line.Point2, line.Direction * line.Length );
	}


	[SerializeField] private Point[] _points;
}


[System.Serializable]
public class Point {

	public Vector3 Location1 {
		get{ return _location1.position; }
	}
	public Vector3 Location2 {
		get{ return _location2.position; }
	}

	[SerializeField] private Transform _location1;
	[SerializeField] private Transform _location2;
}
