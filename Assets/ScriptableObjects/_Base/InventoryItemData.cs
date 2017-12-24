using UnityEngine;


public abstract class InventoryItemData{
	
	public InventoryItem.UseAnimation Animation;
}


[System.Serializable] public class HitData : InventoryItemData {

	public int Power;
	public HitType Type;
	
	public enum HitType {
		Axe,
		Bullet
	}
}
[System.Serializable] public class PlantData : InventoryItemData {

	public GameObject Prefab;
}
[System.Serializable] public class FeedData : InventoryItemData {
}
[System.Serializable] public class InteractData : InventoryItemData {
}
[System.Serializable] public class PlaceData : InventoryItemData {
	
	public GameObject Prefab;
}