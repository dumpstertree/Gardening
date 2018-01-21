using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

	// ***************** PUBLIC *******************

	public Transform CameraTarget;
	public Transform CameraFocus;
	public Rigidbody Rigidbody;

	public QuickSlotInventory QuickslotInventory{ get{ return _quickslotInventory; } }
	public Inventory Inventory { get{ return _inventory; }  }
	public Model.PartInventory GunParts { get { return _gunParts; }}

	[SerializeField] private QuickSlot _quickslot;
	public QuickSlot QuickSlot { get{ return _quickslot; }  }

	[SerializeField] private Interactor _interactor;
	public Interactor Interactor { get{return _interactor; } }
	
	[SerializeField] private Animator _animator;
	public Animator Animator { get{ return _animator;} }
	public PlayerRecipes PlayerRecipes;

	public void FaceInteractableObject( Vector3 position ){
			
		// rotation
		var targetRot = Quaternion.LookRotation( new Vector3( position.x, transform.position.y, position.z) - transform.position );
				
		// position
		var targetPos = transform.position;
		var p1 = transform.position;
		var p2 = new Vector3( position.x, transform.position.y, position.z);
		var d = Vector3.Distance( p1, p2);
		
		if ( d < FACE_INTERACTABLE_MIN_DISTANC || d > FACE_INTERACTABLE_MAX_DISTANC ) { 
			var angle = - Mathf.Atan2(p2.x-p1.x, p2.z-p1.z) * Mathf.Rad2Deg -90;
	     	var x = FACE_INTERACTABLE_FORCE_DISTANCE * Mathf.Cos(angle * Mathf.Deg2Rad);
	     	var y = 0;
	     	var z = FACE_INTERACTABLE_FORCE_DISTANCE * Mathf.Sin(angle * Mathf.Deg2Rad);
	     	targetPos = p2 + new Vector3( x, y, z );
		}

		StartCoroutine( LerpFaceInteractableObject( targetPos, targetRot ) );
	}

	// ***************** PRIVATE *******************

	private Inventory _inventory;
	private QuickSlotInventory _quickslotInventory;
	private Model.PartInventory _gunParts = new Model.PartInventory( 99 );

	private const float FACE_INTERACTABLE_LENGTH = 0.5f;
	private const float FACE_INTERACTABLE_FORCE_DISTANCE = 1.5f;
	private const float FACE_INTERACTABLE_MIN_DISTANC = 0.5f;
	private const float FACE_INTERACTABLE_MAX_DISTANC = 1.6f;

	// *********************************************
	public void Init () {

		CreateCameraTarget();
		CameraFocus = Animator.transform;

		// load data
		_quickslotInventory = DataController.LoadQuickSlotInventory();
		_inventory = DataController.LoadInventory();

		// save when changes are made
		_quickslotInventory.OnInventoryItemChanged += (index, item) => { 
			DataController.SaveQuickSlotInventory( _quickslotInventory ); 
		};
		_inventory.OnInventoryItemChanged += (index, item) => {
			DataController.SaveInventory( _inventory );
		};
	}
	private void CreateCameraTarget(){
		CameraTarget = new GameObject( "CameraTarget" ).transform;
	}

	// *********************************************

	private IEnumerator LerpFaceInteractableObject ( Vector3 targetPos, Quaternion targetRot ) {

		var startPos = transform.position;
		var startRot = transform.rotation;

		for ( float t = 0; t<FACE_INTERACTABLE_LENGTH; t += Time.deltaTime ){
			
			var frac = t/FACE_INTERACTABLE_LENGTH;
			
			transform.position = Vector3.Lerp( startPos, targetPos, frac);
			transform.rotation = Quaternion.Slerp( startRot, targetRot, frac);
			
			yield return null;
		}
	}
}
