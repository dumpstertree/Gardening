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
	public QuickSlot QuickSlot { get{ return _quickslot; }  }
	public Interactor Interactor { get{return _interactor; } }
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

	private Inventory _inventory = new Inventory( 15 );
	private Model.PartInventory _gunParts = new Model.PartInventory( 99 );
	private QuickSlotInventory _quickslotInventory = new QuickSlotInventory( 5 );

	[SerializeField] private Animator _animator;
	[SerializeField] private QuickSlot _quickslot;
	[SerializeField] private Interactor _interactor;

	[SerializeField] private InventoryItem _topSlot;
	[SerializeField] private InventoryItem _rightSlot;
	[SerializeField] private InventoryItem _bottomSlot;
	[SerializeField] private InventoryItem _leftSlot;
	[SerializeField] private InventoryItem _centerSlot;

	private const float FACE_INTERACTABLE_LENGTH = 0.5f;
	private const float FACE_INTERACTABLE_FORCE_DISTANCE = 1.5f;
	private const float FACE_INTERACTABLE_MIN_DISTANC = 0.5f;
	private const float FACE_INTERACTABLE_MAX_DISTANC = 1.6f;

	// *********************************************
	public void Init () {

		CreateCameraTarget();
		CameraFocus = Animator.transform;

		_quickslotInventory.SetInventoryItem( _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Top),    ScriptableObject.Instantiate( _topSlot ) );
		_quickslotInventory.SetInventoryItem( _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Right),  ScriptableObject.Instantiate( _rightSlot ) );
		_quickslotInventory.SetInventoryItem( _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Bottom), ScriptableObject.Instantiate( _bottomSlot ) );
		_quickslotInventory.SetInventoryItem( _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Left),   ScriptableObject.Instantiate( _leftSlot ) );
		_quickslotInventory.SetInventoryItem( _quickslotInventory.ConvertQuickSlotIDToIndex( QuickSlotInventory.ID.Center), ScriptableObject.Instantiate( _centerSlot ) );
		
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
