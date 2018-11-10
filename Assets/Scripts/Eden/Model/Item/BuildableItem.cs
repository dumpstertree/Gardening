using UnityEngine;

namespace Eden.Model {
	
	public class GunBuildableItem : Item {
		
		public Eden.Model.Building.Parts.Gun Part {
			get; private set;
		}

		public GunBuildableItem( string prefabID, string displayName, int maxCount, bool expendable, Sprite sprite, Eden.Model.Building.Parts.Gun part ) : base (prefabID, displayName, maxCount, expendable, sprite) {
			Part = part;
		}
	}
}