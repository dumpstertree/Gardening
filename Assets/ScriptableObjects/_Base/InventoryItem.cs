using UnityEngine;
using System;
using System.Collections;


public partial class InventoryItem : ScriptableObject {

	// ***************** PUBLIC *******************

	public delegate void OnCountChangedEvent();
	public OnCountChangedEvent OnCountChanged;
	
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

	// ********************************************
	
	public string Name{ get{ return _name; } }
	public int Count{ get{ return _count; } }
	public int MaxCount{ get{ return _maxCount; } }
	public bool CanHit { get{ return _canHit; } }
	public bool CanPlant { get{ return _canPlant; } }
	public bool CanFeed { get{ return _canFeed; } }
	public bool CanInteract { get{ return _canInteract; } }
	public bool CanPlace { get{ return _canPlace; } }
	public bool CanShoot { get{ return _canShoot; } }
	public Sprite Sprite { get{ return _sprite; } }
	public GameObject HoldItem { get{ return _holdItem; } }

	// ***************** PRIVATE *******************

	[HeaderAttribute(" Object Properties ")]
	[SerializeField] private string _name;
	[SerializeField] private Sprite _sprite;
	[SerializeField] private int _count;
	[SerializeField] private int _maxCount;
	[SerializeField] private bool _expendable;
	[SerializeField] private GameObject _holdItem;

	[HeaderAttribute("Hit")]
	[SerializeField] private bool _canHit;
	[SerializeField] private HitData _hitData;

	[HeaderAttribute("Plant")]
	[SerializeField] private bool _canPlant;
	[SerializeField] private PlantData _plantData;

	[HeaderAttribute("Feed")]
	[SerializeField] private bool _canFeed;
	[SerializeField] private FeedData _feedData;

	[HeaderAttribute("Interact")]
	[SerializeField] private bool _canInteract;
	[SerializeField] private InteractData _interactData;

	[HeaderAttribute("Place")]
	[SerializeField] private bool _canPlace;
	[SerializeField] private PlaceData _placeData;

	[HeaderAttribute("Shoot")]
	[SerializeField] private bool _canShoot;
	[SerializeField] public ShootData _shootData;

	// *********************************************

	private void Place ( Interactor interactor ) {

		RaycastHit hit;
		if (Physics.Raycast( interactor.transform.position, Vector3.down, out hit )){
		
			var go = Instantiate( _placeData.Prefab );
			go.transform.position = hit.point;
			go.transform.rotation = interactor.transform.rotation;
		}
	}
	private void Shoot ( Player shooter, ShootData shootData, Action onComplete ) {

		var gunStats = shootData.CraftedGun.GunStats;
		for ( int i = 0; i < gunStats.NumberOfBullets; i++ ){
			
			var go = Instantiate( shootData.BulletPrefab );
			go.transform.position = Game.Area.LoadedPlayer.transform.position;
			go.transform.rotation = Game.Area.LoadedPlayer.transform.rotation;
			go.GetComponent<Bullet>().SetBullet( shooter, shootData._hitData );

			go.transform.rotation  = go.transform.rotation * Quaternion.AngleAxis( UnityEngine.Random.Range( -5f, 5f), go.transform.right );
			go.transform.rotation  = go.transform.rotation * Quaternion.AngleAxis( UnityEngine.Random.Range( -5f, 5f), go.transform.up );
		}

		Game.Async.WaitForSeconds( 1.0f/gunStats.FireRate, onComplete );
	}
	private void Use ( Player player, InventoryItemData data, Action action, Action onComplete ) {
		
		// get use data
		var useData = GetUseAnimationData( data.Animation );
		var interactableObject = player.Interactor.InteractableObject;

		// force player to look at interactable
		if ( interactableObject ){
			player.FaceInteractableObject( interactableObject.transform.position );
		}
		
		// play animation
		player.Animator.SetTrigger( useData.AnimationTrigger );

		// use action
		Game.Async.WaitForSeconds( useData.AnimationUseFraction * useData.AnimationLength, () => {
			action();
		});

		// run onComplete
		Game.Async.WaitForSeconds( useData.AnimationLength, () => { 
			onComplete();
		});

		// reduce count
		if ( _expendable ){
			ReduceCount( 1 );
		}
	}
}