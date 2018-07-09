﻿using UnityEngine;

public class HoldItem : MonoBehaviour {

	[SerializeField] private Eden.Life.BlackBox _player;
	[SerializeField] private GameObject _defaultItem;
	[SerializeField] private LayerMask _holdItemLayerMask;

	private GameObject _holdItem;

	private void Awake () {

		_player.QuickslotChip.OnInputChanged += LoadNewItem;
	}

	private void LoadNewItem ( int index ) {

		// //get item
		// var currentItem = _player.EquipedItems.GetInventoryItem( index );

		// // destroy old
		// if ( _holdItem != null ) {
		// 	var obj = _holdItem;
		// 	_holdItem.GetComponent<Animator>().SetTrigger("Destroy");
		// 	EdensGarden.Instance.Async.WaitForSeconds( 1.0f, () => { Destroy( obj ); } );
		// }

		// // create new
		// if ( currentItem != null && currentItem.HoldItem != null ) {
		// 	_holdItem = Instantiate( currentItem.HoldItem );
		// } else{
		// 	_holdItem = Instantiate( _defaultItem );
		// }

		// // set transform
		// _holdItem.transform.SetParent( transform, false );
		// _holdItem.transform.localPosition = Vector3.zero;
		// _holdItem.transform.localRotation = Quaternion.identity;

		// foreach( Transform t in _holdItem.transform.GetComponent<Transform>() ){
		// 	t.gameObject.layer = gameObject.layer;
		// }
	}
}
