using System.Collections.Generic;
using UnityEngine;

public class Gun2 : MonoBehaviour {

	public List<Part2> Parts;
	public Eden.Model.Building.Stats.Gun Stats;

	public Gun2( List<Part2> parts, Eden.Model.Building.Stats.Gun stats ) {
		
		Parts = parts;
		Stats = stats;
	}
}

public class Part2 {

	public Eden.Model.Building.Parts.Gun Part;
	public int Row;
	public int Collumn;
	public Vector3 Rotation;

	public Part2( Eden.Model.Building.Parts.Gun part, int row, int collumn, Vector3 rotation ) {

		Part = part;
		Row = row;
		Collumn = collumn;
		Rotation = rotation;
	}
}


