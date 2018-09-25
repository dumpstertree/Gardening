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
				

			// Set Texture 
			_rend.material.mainTexture = Item.Sprite.texture;


			// Set count
			if ( Item.Count > 1 ){
			
				_count.SetActive( true );
				_countText.text = Item.Count.ToString() + "x";
			
		
			// Set Count Not Active
			} else{
			
				_count.SetActive( false );
			}
		}
	}

	private void Update () {

		_rend.transform.forward = Camera.main.transform.forward;
	}
}
