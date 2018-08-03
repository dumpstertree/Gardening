using UnityEngine;

namespace Eden.Model.Building.Grid {
	
	[System.Serializable]
	public class SaveData {

		[SerializeField] public Part[] Parts;

		public SaveData( Part[] parts ) {
			
			Parts = parts;
		}
	}

	[System.Serializable]
	public class Part {

		[SerializeField] public int Row;
		[SerializeField] public int Collumn;
		[SerializeField] public Vector3 Rotation;
		[SerializeField] public string PartPrefabID;

		public Part( int row, int collumn, Vector3 rotation, string partPrefabID ) {
			
			Row = row;
			Collumn = collumn;
			Rotation = rotation;
			PartPrefabID = partPrefabID;
		}
	}
}