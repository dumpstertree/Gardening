using UnityEngine;
using System;

[System.Serializable]
public class InventoryItem {

	// ***************** PUBLIC *******************

	public InventoryItem () {}
	public InventoryItem ( 
		string id,
		int count,
		string displayName,
		int maxCount,
		Sprite sprite,
		GameObject holdItem,
		bool expendable,
		bool canInteract,
		Controller.Item.InteractData interactData,
		bool canShoot,
		Controller.Item.ShootData shootData ) {

		_id = id;
		_count = count;
		_displayName = displayName;
		_maxCount = maxCount;
		_sprite = sprite;
		_holdItem = holdItem;
		_expendable = expendable;

		_canInteract = canInteract;
		_interactor = interactData;
		
		_canShoot = canShoot;
		_shootData = shootData;
	}

	// ********************************************

	public delegate void OnCountChangedEvent();
	public OnCountChangedEvent OnCountChanged;

	
	// ********************************************
	
	public string ID { 
		get{ return _id; } 
	}
	public int Count { 
		get{ return _count; } 
	}

	
	// ********************************************

	public string DisplayName { 
		get{ return _displayName; } 
	}
	public int MaxCount { 
		get{ return _maxCount; } 
	}
	public Sprite Sprite { 
		get{ return _sprite; } 
	}
	public GameObject HoldItem { 
		get{ return _holdItem; } 
	}
	public bool Expendable { 
		get{ return _expendable; } 
	}
	public bool CanShoot {
		get{ return _canShoot; }
	}
	public bool CanInteract {
		get { return _canInteract; }
	}
	public bool CanPlace {
		get { return _canPlace; }
	}
	public bool CanHit {
		get { return _canHit; }
	}
	public bool CanPlant {
		get { return _canPlant; }
	}


	// ********************************************

	[SerializeField] private string _id;
	[SerializeField] private int _count;

	private string _displayName;
	private int _maxCount;
	private Sprite _sprite;
	private GameObject _holdItem;
	private bool _expendable;

	public Controller.Item.ShootData _shootData;
	public Controller.Item.InteractData _interactor;

	private bool _canShoot;
	private bool _canInteract;
	private bool _canPlace;
	private bool _canHit;
	private bool _canPlant;


	// ********************************************

	public void Use ( Creature user, Action onComplete ) {

		var objectToInteractWith = user.Interactor.InteractableObject;

		if ( _canInteract && objectToInteractWith != null && objectToInteractWith.Interactable ) { 
			Interact( user, onComplete ); 
			return; 
		}
		if ( _canShoot) {
			Shoot( user, onComplete ); 
			return; 
		}

		if ( _expendable ){
			ReduceCount( 1 );
		}
	}
	public void AddCount ( int more ) {

		_count += more;

		if ( OnCountChanged != null ){
			OnCountChanged();
		}
	}
	public void ReduceCount ( int less ) {

		_count -= less;

		if ( OnCountChanged != null ){
			OnCountChanged();
		}
	}
	public void SetCount ( int count ) {
		
		_count = count;

		if ( OnCountChanged != null ){
			OnCountChanged();
		}
	}


	// ******************* Private  *************************
	
	private void Interact ( Creature interactor, Action onComplete ) {

		_interactor.Interact( interactor, this );
		if ( onComplete != null ) { onComplete(); }
	}
	private void Shoot ( Creature interactor, Action onComplete ) {
		
		_shootData.Fire( interactor );
 		if ( onComplete != null ) { onComplete(); }
	}
	private void Swing ( Creature interactor, Action onComplete ) {
	}
	private void Place ( Creature interactor, Action onComplete ) {
		
		// RaycastHit hit;
		// if (Physics.Raycast( interactor.transform.position, Vector3.down, out hit )){
		
		// 	var go = GameObject.Instantiate( _placeData.Prefab );
		// 	go.transform.position = hit.point;
		// 	go.transform.rotation = interactor.transform.rotation;
		// }
	}
	private void Plant ( Creature interactor, Action onComplete  ) {
	}

	
	// *********************************************

	public static InventoryItem Deserialize ( InventoryItem serializedData ) {
		
		var id = serializedData.ID;
		var template = Model.Template.InventoryItemTemplate.GetTemplate( id );
		var count = serializedData.Count;

		return template.GetInstance( count );
	}
}
