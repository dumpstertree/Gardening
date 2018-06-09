using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour {

	// ********** PUBLIC **************

	public void SetBullet ( Creature user, HitData hitData ) {
		
		_shooter = user;
		_hitData = hitData;

		CreateCasing();
		EdensGarden.Instance.Async.WaitForSeconds( BULLET_KILL_TIME, () => { Destroy( false ); } );
	}

	// *********** PRIVATE ************

	[SerializeField] private float _speed;
	[SerializeField] private GameObject _casingPrefab;
	[SerializeField] private GameObject _contactPrefab;

	private const float CASING_KILL_TIME = 60.0f;
	private const float BULLET_KILL_TIME = 10.0f;
	private const float CONTACT_OFFSET = 0.01f;
	private const float CONTACT_MIN_SCALE = 0.20f;
	private const float CONTACT_MAX_SCALE = 0.05f;

	private Creature _shooter;
	private HitData _hitData;
	private bool _beingDestroyed;
	
	// ***********************

	private void Update () {

		transform.Translate( Vector3.forward * ( _speed * Time.deltaTime ) );
	}
	private void OnTriggerEnter( Collider collision ) {
	
		var interactable = collision.GetComponent<Interactable.InteractableObject>();
		if ( interactable && interactable.Hitable ){
			interactable.HitDelegate.Hit( _shooter, _hitData );
		}

		Destroy( true );
	}

	// ***********************

	private void Destroy ( bool createContact ) {

		if( !_beingDestroyed ) {
		
			_beingDestroyed = true;

			if ( createContact ) {

				CreateContact();
			}
		
			// Effects
			Game.Effects.OneShot( Application.Effects.Type.Fireworks, transform.position, transform.rotation );

			// Destroy
			Destroy( gameObject );
		}
	}
	private void CreateCasing () {
		
		var inst = Instantiate( _casingPrefab );
		inst.transform.position = transform.position;
		inst.transform.rotation = transform.rotation * Quaternion.AngleAxis( 90, Vector3.right );

		var velocity = new Vector3();
		velocity.x = Random.Range( -3, 1);
		velocity.y = Random.Range(  1, 3);
		velocity.z = Random.Range( -3, 3);

		inst.GetComponent<Rigidbody>().velocity = velocity;

		EdensGarden.Instance.Async.WaitForSeconds( CASING_KILL_TIME, () => { Destroy( inst ); } );
	}
	private void CreateContact () {

		RaycastHit hit;

        if ( Physics.Raycast( transform.position, transform.forward, out hit ) ) {
			
			var inst = Instantiate( _contactPrefab );

			// rotation
			var rotation = Random.Range( 0, 360);
			inst.transform.rotation = Quaternion.LookRotation( -hit.normal );
			inst.transform.Rotate( inst.transform.forward, rotation );

			// position
			inst.transform.position = hit.point;
			inst.transform.Translate( Vector3.back * CONTACT_OFFSET );

			// scale 
			var scale = Random.Range( CONTACT_MIN_SCALE, CONTACT_MAX_SCALE); 
			inst.transform.localScale = new Vector3( scale, scale, scale );
        }
	}


	private struct Setter {

		public Damage DamageType;
		public List<Element> ElementTypes;

		public float Speed;
		public float AimAssist;
	}

	private enum Damage {

		Slashing,
		Bludgeoning,
		Piercing
	}
	private enum Element {

		None,
		Fire,
		Cold,
		Lightning,
		Acid,
		Psychic,
		Necrotic,
		Radiant
	}
}
