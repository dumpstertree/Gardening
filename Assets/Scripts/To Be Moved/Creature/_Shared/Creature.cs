using System.Collections;
using UnityEngine;
using Interactable.OptionalComponent;
using Dumpster.Core.BuiltInModules.Effects;

public abstract class Creature : Interactable.InteractableObject { 
	
	// this class should be reanamed to something that represents 
	// an object that performs actions but is not alive for example a turret

	// **************************

	public QuickSlotInventory QuickslotInventory{ 
		get{ return _quickslotInventory; } 
	}
	public Inventory Inventory { 
		get{ return _inventory; }  
	}
	public Transform GunProjector { 
		get{ return _projectileSpawner; } 
	}
	public Animator Animator { 
		get{ return _animator;} 
	}
	public Brain Brain { 
		get{ return _brain; } 
	}
	public Interactor Interactor {
		get{return _interactor; } 
	}
	public Dumpster.Physics.Controller Physics {
		get{ return _physics;}
	}
	public abstract Animations AnimationsData{ get; }

	// **************************

	public virtual void Init () {

		_health.OnHealthChanged += currentHealth => {

			if ( currentHealth <= 0 && !_dead ) { Faint(); }
			if ( currentHealth >= 1 &&  _dead ) { WakeUp(); }
		};
	}

	
	// **************************

	[Header( "Creature Properties" )]
	[SerializeField] protected Interactor _interactor;
	[SerializeField] protected Transform _projectileSpawner;
	[SerializeField] protected Animator _animator;
	[SerializeField] protected Health _health;
	[SerializeField] protected Brain _brain;
	[SerializeField] protected Dumpster.Physics.Controller _physics;

	protected QuickSlotInventory _quickslotInventory;
	protected Inventory _inventory;


	// **************************

	protected virtual void Update () {

		var currentAnimState = _animator.GetCurrentAnimatorStateInfo( 0 );
		
		if ( !currentAnimState.IsTag( RESRICTED_INPUT_TAG ) && !_dead ){
			_brain.Think();
		}
	}
	protected virtual void Start () {
	}
	protected virtual void Faint () {

		_dead = true;
		_animator.SetTrigger( FAINT_TRIGGER );

		EdensGarden.Instance.Effects.OneShot( ParticleType.Faint, transform.position, transform.rotation );
	}
	protected virtual void WakeUp () {

		_dead = false;
		_animator.SetTrigger( WAKE_UP_TRIGGER );

		EdensGarden.Instance.Effects.OneShot( ParticleType.WakeUp, transform.position, transform.rotation );
	}


	// **************************

	private const string FAINT_TRIGGER = "Dead";
	private const string WAKE_UP_TRIGGER = "WakeUp";
	private const string RESRICTED_INPUT_TAG = "InputRestricted";

	private const float FACE_INTERACTABLE_LENGTH = 0.5f;
	private const float FACE_INTERACTABLE_FORCE_DISTANCE = 1.5f;
	private const float FACE_INTERACTABLE_MIN_DISTANC = 0.5f;
	private const float FACE_INTERACTABLE_MAX_DISTANC = 1.6f;

	private bool _dead;


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

	public enum AnimationTrigger {
		None,
		Emote
	}
	public struct Animations {

		public Animation None;
		public Animation Emote;

		public Animations ( Animation none, Animation emote ) {
			None = none;
			Emote = emote;
		}
	}
	public struct Animation {

		public string Trigger { get; }
		public float Length { get; }
		public float ActionPercentage { get; }

		public Animation ( string trigger, float length, float actionPercentage ) {
			Trigger = trigger;
			Length = length;
			ActionPercentage = actionPercentage;
		}
	}
}
