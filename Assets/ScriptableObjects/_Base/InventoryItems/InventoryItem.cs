using UnityEngine;
using System;


[System.Serializable]
public partial class InventoryItem : ScriptableObject {

	// ***************** PUBLIC *******************

	public delegate void OnCountChangedEvent();
	public OnCountChangedEvent OnCountChanged;

	// ********************************************
	
	[SerializeField] private string _id;
	public string ID { 
		get{ return _id; } 
	}

	[SerializeField] private int _count;
	public int Count{ 
		get{ return _count; } 
	}

	// ********************************************

	[SerializeField] private string _displayName;
	public string DisplayName{ 
		get{ return _displayName; } 
	}

	[SerializeField] private int _maxCount;
	public int MaxCount{ 
		get{ return _maxCount; } 
	}

	[SerializeField] private Sprite _sprite;
	public Sprite Sprite { 
		get{ return _sprite; } 
	}

	[SerializeField] private GameObject _holdItem;
	public GameObject HoldItem { 
		get{ return _holdItem; } 
	}

	[SerializeField] private bool _expendable;
	public bool Expendable { 
		get{ return _expendable; } 
	}

	// ********************************************
	
	private bool _hasBeenInit;
	public void Init ( string id = "" ) {

		_id = id != "" ? id : Guid.NewGuid().ToString();
		
		InitCanShoot();
	}


	public void Use ( Player player, Action onComplete ) {

		var interactableObject = player.Interactor.InteractableObject;

		// place
		if ( _canPlace ) { 
			Use( player, _interactData, () => { Place( player.Interactor );  }, onComplete );
		}

		// shoot
		if ( _canShoot ) {
			Shoot( player, _shootData, onComplete );
		}

		// hit
		if ( _canHit && player.Interactor.InteractableObject.Hitable ) { 
			Use( player, _interactData, () => { interactableObject.HitDelegate.Hit( player, _hitData );  }, onComplete );
		}

		// plant
		if ( _canPlant && player.Interactor.InteractableObject.Plantable ) { 
		}

		// feed
		if ( _canFeed && player.Interactor.InteractableObject.Feedable ) { 
		}

		// interact
		if ( _canInteract && player.Interactor.InteractableObject.Interactable) { 
			Use( player, _interactData, () => { interactableObject.InteractDelegate.Interact( player, this ); }, onComplete );
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