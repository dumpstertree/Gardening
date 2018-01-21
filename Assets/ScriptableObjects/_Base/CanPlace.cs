using UnityEngine;

partial class InventoryItem {

	public bool CanPlace { get{ return _canPlace; } }

	[HeaderAttribute("Place")]
	[SerializeField] private bool _canPlace;
	[SerializeField] private PlaceData _placeData;

	private void Place ( Interactor interactor ) {

		RaycastHit hit;
		if (Physics.Raycast( interactor.transform.position, Vector3.down, out hit )){
		
			var go = Instantiate( _placeData.Prefab );
			go.transform.position = hit.point;
			go.transform.rotation = interactor.transform.rotation;
		}
	}
}
