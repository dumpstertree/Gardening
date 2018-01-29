using System.Collections;
using UnityEngine;
using Interactable.OptionalComponent;

[RequireComponent( typeof(Health) )]
public abstract class Creature : Interactable.InteractableObject {

	// **************************

	public QuickSlotInventory QuickslotInventory{ 
		get{ return _quickslotInventory; } 
	}
	public Inventory Inventory { 
		get{ return _inventory; }  
	}
	public Transform GunProjector { 
		get{ return _gunProjector; } 
	}
	public Animator Animator { 
		get{ return _animator;} 
	}
	public Rigidbody Rigidbody { 
		get { return _rigidBody; } 
	}
	public Brain Brain { 
		get{ return _brain; } 
	}
	public Interactor Interactor {
		get{return _interactor; } 
	}

	// **************************

	public virtual void Init () {

		_health = GetComponent<Health>();
		_brain = GetComponent<Brain>();
		_rigidBody = GetComponent<Rigidbody>();

		_health.OnHealthChanged += currentHealth => {

			if ( currentHealth <= 0 && !_dead ) {
				Faint();
			}
			if ( currentHealth >= 1 && _dead ) {
				WakeUp();
			}
		};
	}

	
	// **************************



	[SerializeField] protected Interactor _interactor;
	[SerializeField] protected Transform _gunProjector;
	[SerializeField] protected Animator _animator;

	protected Health _health;
	protected Brain _brain;
	protected Rigidbody _rigidBody;
	protected QuickSlotInventory _quickslotInventory;
	protected Inventory _inventory;

	private bool _dead;


	
	// **************************

	protected virtual void Update () {

		if ( !_animator.GetCurrentAnimatorStateInfo(0).IsTag("InputRestricted") && !_dead ){
			_brain.Think();
		}
	}
	protected virtual void Start () {
	}
	protected virtual void Faint () {

		_dead = true;
		_animator.SetTrigger( "Dead" );
		_rigidBody.isKinematic = true;

		Game.Effects.OneShot( Application.Effects.Type.Faint, transform.position, transform.rotation );
	}
	protected virtual void WakeUp () {

		_dead = false;
		_animator.SetTrigger( "WakeUp" );
		_rigidBody.isKinematic = false;

		Game.Effects.OneShot( Application.Effects.Type.WakeUp, transform.position, transform.rotation );
	}


	// **************************

	private const float FACE_INTERACTABLE_LENGTH = 0.5f;
	private const float FACE_INTERACTABLE_FORCE_DISTANCE = 1.5f;
	private const float FACE_INTERACTABLE_MIN_DISTANC = 0.5f;
	private const float FACE_INTERACTABLE_MAX_DISTANC = 1.6f;


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
