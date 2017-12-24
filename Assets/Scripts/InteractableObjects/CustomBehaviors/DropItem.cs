using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactable.Component;

[RequireComponent(typeof(Pickup))]
public class DropItem : MonoBehaviour {

	[SerializeField] private MeshRenderer _rend;
	[SerializeField] private TextMesh _countText;
	[SerializeField] private GameObject _count;

	private Pickup _pickup;

	public void SetItem ( InventoryItem item ) {
		
		_pickup.Item = item;
		_rend.material.mainTexture = item.Sprite.texture;

		if ( item.Count > 1 ){
			_count.SetActive( true );
			_countText.text = item.Count.ToString() + "x";
		}
		else{
			_count.SetActive( false );
		}
	}

	private void Update () {
		transform.GetChild(0).LookAt( Camera.main.transform );
	}
	private void Awake () {

		_pickup = GetComponent<Pickup>();
	}
}
