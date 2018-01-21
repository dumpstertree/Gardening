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
		set{ _id = value; }
	}

	[SerializeField] private string _path;
	public string Path { 
		get{ return _path; } 
	}

	[SerializeField] private string _name;
	public string Name{ 
		get{ return _name; } 
	}

	[SerializeField] private int _count;
	public int Count{ 
		get{ return _count; } 
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
		
		var obj = Resources.Load( serializedData.ItemPath ) as InventoryItem;
		var inst = Instantiate( obj );

		inst._count = serializedData.Count;
		inst._id = serializedData.ID;

		return inst;
	}

	[System.Serializable]
	public class Serialized {

		[SerializeField] public string ID;
		[SerializeField] public int Count;
		[SerializeField] public string ItemPath;

		public Serialized ( InventoryItem item ) {

			ID = item.ID;
			Count = item.Count;
			ItemPath = item.Path;
		}
	}
}
