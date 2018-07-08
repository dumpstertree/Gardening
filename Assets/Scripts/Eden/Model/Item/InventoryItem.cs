using UnityEngine;
using System;

[System.Serializable]
public class InventoryItem {

	// ***************** Constructor *******************

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
		Controller.Item.ShootData shootData,
		bool canHit ) {

		_id = id;
		_count = count;
		_displayName = displayName;
		_maxCount = maxCount;
		_sprite = sprite;
		_holdItem = holdItem;
		_expendable = expendable;

		_canAction = canInteract;
		_interactor = interactData;
		
		_canShoot = canShoot;
		_shootData = shootData;

		_canHit = canHit;
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
		get { return _canAction; }
	}
	public bool CanHit {
		get { return _canHit; }
	}
	public bool CanPlant {
		get { return false; }
	}
	public bool HasGunBuilding {
		get { return true; }
	}


	// ******************* Public **********************

	public void Use ( Eden.Life.BlackBox user, Eden.Interactable.InteractableObject interactable , Action onComplete ) {

		
		// Use
		if ( _canAction ) { 
			Interact( user, onComplete ); 
			return; 
		}
		if ( _canShoot) {
			Shoot( user, onComplete ); 
			return; 
		}
		if ( _canHit) {
			Hit( user, onComplete ); 
			return; 
		}


		if ( _expendable ) {
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

	[SerializeField] private string _id;
	[SerializeField] private int _count;

	private string _displayName;
	private int _maxCount;
	private Sprite _sprite;
	private GameObject _holdItem;
	private bool _expendable;

	public Controller.Item.ShootData _shootData;
	public Controller.Item.InteractData _interactor;
	public Controller.Item.HitData2 _hitData = new Controller.Item.HitData2();
	public Eden.Model.Building.Stats.Gun _gunStats = new Eden.Model.Building.Stats.Gun();

	private bool _canShoot;
	private bool _canAction;
	private bool _canHit;

	
	private void Interact ( Eden.Life.BlackBox user, Action onComplete ) {

		_interactor.Interact( user, this, onComplete );
	}
	private void Shoot ( Eden.Life.BlackBox user, Action onComplete ) {
		
		_shootData.Fire( user, _gunStats );
 		if ( onComplete != null ) { onComplete(); }
	}
	private void Hit ( Eden.Life.BlackBox user, Action onComplete ) {

		_hitData.Hit( user );
		if ( onComplete != null ) { onComplete(); }
	}
	
	
	
	// *********************************************

	public static InventoryItem Deserialize ( InventoryItem serializedData ) {
		
		var id = serializedData.ID;
		var template = Eden.Model.Template.InventoryItemTemplate.GetTemplate( id );
		var count = serializedData.Count;

		return template.GetInstance( count );
	}
}
