using UnityEngine;
using System;

[System.Serializable]
public partial class InventoryItem : ScriptableObject {

	
	// ***************** PUBLIC *******************

	public delegate void OnCountChangedEvent();
	public OnCountChangedEvent OnCountChanged;

	
	// ********************************************
	
	public string ID { 
		get{ return _id; } 
	}
	public int Count{ 
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


	// ********************************************

	[SerializeField] private string _id;
	[SerializeField] private int _count;
	[SerializeField] private string _displayName;
	[SerializeField] private int _maxCount;
	[SerializeField] private Sprite _sprite;
	[SerializeField] private GameObject _holdItem;
	[SerializeField] private bool _expendable;

	private bool _hasBeenInit;


	// ********************************************
	
	public void Init ( string id = "" ) {

		_id = id != "" ? id : Guid.NewGuid().ToString();
		
		InitCanShoot();
	}

	public void Use ( Creature user, Action onComplete ) {

		var interactableObject = user.Interactor.InteractableObject;

		// place
		if ( _canPlace ) { 
			Place( user.Interactor );
		}

		// shoot
		if ( _canShoot ) {
			Shoot( user, _shootData, onComplete );
		}


		// hit
		if ( _canHit && user.Interactor.InteractableObject.Hitable ) { 
			Use( user, _interactData, () => { interactableObject.HitDelegate.Hit( user, _hitData );  }, onComplete );
		}

		// plant
		if ( _canPlant && user.Interactor.InteractableObject.Plantable ) { 
		}

		// feed
		if ( _canFeed && user.Interactor.InteractableObject.Feedable ) { 
		}

		// interact
		if ( _canInteract && user.Interactor.InteractableObject.Interactable) { 
			Use( user, _interactData, () => { interactableObject.InteractDelegate.Interact( user, this ); }, onComplete );
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

	
	// *********************************************

	public static Serialized Serialize ( InventoryItem inventoryItem ) {

		return new InventoryItem.Serialized( inventoryItem );
	}
	public static InventoryItem Deserialize ( Serialized serializedData ) {
				
		var inst = Game.ItemManager.RequestItem( serializedData.ItemName, serializedData.ID );

		inst._count = serializedData.Count;

		return inst;
	}

	[System.Serializable]
	public class Serialized {

		[SerializeField] public string ID;
		[SerializeField] public int Count;
		[SerializeField] public string ItemName;

		public Serialized ( InventoryItem item ) {

			ID = item.ID;
			Count = item.Count;
			ItemName = item.name;
		}
	}
}
