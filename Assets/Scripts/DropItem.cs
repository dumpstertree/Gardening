using UnityEngine;
using Eden.Model;

public class DropItem : MonoBehaviour {

	[SerializeField] private MeshRenderer _rend;
	[SerializeField] private TextMesh _countText;
	[SerializeField] private GameObject _count;

	public Item Item {
		get; private set;
	}

	public void SetItem ( Item item ) {
		
		Item = item;
		
		if ( Item != null ) {
			
			_rend.material.mainTexture = item.Sprite.texture;

			if ( item.Count > 1 ){
				_count.SetActive( true );
				_countText.text = item.Count.ToString() + "x";
			} else{
				_count.SetActive( false );
			}
		}
	}

	private void Update () {

		_rend.transform.forward = Camera.main.transform.forward;
	}
}
