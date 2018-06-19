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
		Model.Template.ItemAnimation animation,
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
		_animation = animation;
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
	private Model.Template.ItemAnimation _animation;

	public Controller.Item.ShootData _shootData;
	public Controller.Item.InteractData _interactor;

	private bool _canShoot;
	private bool _canInteract;
	private bool _canPlace;
	private bool _canHit;
	private bool _canPlant;


	// ********************************************

	public void Use ( Eden.Life.BlackBox user, Eden.Interactable.InteractableObject interactable , Action onComplete ) {

		// Play animation
		// switch ( _animation.Trigger ) {
			
		// 	case Creature.AnimationTrigger.None: 
		// 		user.Body.Animator.SetTrigger( "" );
		// 		break;
			
		// 	case Creature.AnimationTrigger.Emote: 
		// 		user.Body.Animator.SetTrigger( "" );
		// 		break;
		// }

		// Use
		if ( _canInteract && interactable != null && interactable.Interactable ) { 
			Interact( user, onComplete ); 
			return; 
		}
		if ( _canShoot) {
			Shoot( user, onComplete ); 
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
	
	private void Interact ( Eden.Life.BlackBox user, Action onComplete ) {

		// _interactor.Interact( user, this );
		// if ( onComplete != null ) { onComplete(); }
	}
	private void Shoot ( Eden.Life.BlackBox user, Action onComplete ) {
		
		_shootData.Fire( user );
 		if ( onComplete != null ) { onComplete(); }
	}
	private void Swing ( Eden.Life.BlackBox user, Action onComplete ) {
	}
	private void Place ( Eden.Life.BlackBox user, Action onComplete ) {
	}
	private void Plant ( Eden.Life.BlackBox user, Action onComplete  ) {
	}

	
	// *********************************************

	public static InventoryItem Deserialize ( InventoryItem serializedData ) {
		
		var id = serializedData.ID;
		var template = Model.Template.InventoryItemTemplate.GetTemplate( id );
		var count = serializedData.Count;

		return template.GetInstance( count );
	}
}
