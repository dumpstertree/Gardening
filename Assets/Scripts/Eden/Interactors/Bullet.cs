using UnityEngine;
using Dumpster.Core.BuiltInModules.Effects;

public class Bullet : MonoBehaviour {

	// ********** PUBLIC **************

	public void SetBullet ( Eden.Life.BlackBox user , HitData hitData ) {
		
		_shooter = user;
		_hitData = hitData;

		CreateCasing();

		EdensGarden.Instance.Async.WaitForSeconds( BULLET_KILL_TIME, () => { var go = gameObject; if ( go != null ) Destroy( go ); } );
	}

	// *********** PRIVATE ************

	[SerializeField] private float _speed;
	[SerializeField] private GameObject _casingPrefab;
	[SerializeField] private LayerMask _layermask;

	private const float CASING_KILL_TIME = 60.0f;
	private const float BULLET_KILL_TIME = 10.0f;
	private const float CONTACT_OFFSET = 0.01f;

	private Eden.Life.BlackBox _shooter;
	private HitData _hitData;
	
	// ***********************

	private void Update () {

		transform.Translate( Vector3.forward * ( _speed * Time.deltaTime ) );
	}
	private void OnTriggerEnter( Collider collision ) {

  		if( _layermask == (_layermask | (1 << collision.gameObject.layer) ) ) {

  			if ( _shooter.Colliders.Contains( collision ) ) {
  				return;
  			}

			var interactable = collision.GetComponentInChildren<Eden.Interactable.InteractableObject>();
			if ( interactable && interactable.Hitable ){
				interactable.HitDelegate.Hit( _shooter, _hitData );
			}

			EdensGarden.Instance.Effects.OneShot( ParticleType.Fireworks, transform.position, transform.rotation );

			// Destroy
			Destroy( gameObject );
  		}
	}


	// ***********************

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
}
