using UnityEngine;

public class DropItem : MonoBehaviour {

	[SerializeField] private MeshRenderer _rend;
	[SerializeField] private TextMesh _countText;
	[SerializeField] private GameObject _count;

	public InventoryItem Item {
		get{ return _item; }
	}
	private InventoryItem _item;

	public void SetItem ( InventoryItem item ) {
		
		_item = item;
		_rend.material.mainTexture = item.Sprite.texture;

		if ( item.Count > 1 ){
			_count.SetActive( true );
			_countText.text = item.Count.ToString() + "x";
		} else{
			_count.SetActive( false );
		}
	}

	private void Update () {

		_rend.transform.forward = Camera.main.transform.forward;
	}
}
