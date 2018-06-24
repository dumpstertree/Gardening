using UnityEngine;

public class Slash : MonoBehaviour {

	public void Set ( Eden.Life.BlackBox attacker , HitData hitData ) {
		
		_attacker = attacker;
		_hitData = hitData;

		EdensGarden.Instance.Async.WaitForSeconds( KILL_TIME, () => { if ( gameObject != null ) { Destroy( gameObject ); } } );
	}



	[SerializeField] private LayerMask _layermask;

	private const float KILL_TIME = 0.25f;

	private Eden.Life.BlackBox _attacker;
	private HitData _hitData;


	private void OnTriggerEnter( Collider collision ) {


  		if( _layermask == (_layermask | (1 << collision.gameObject.layer) ) ) {

  			if ( _attacker.Colliders.Contains( collision ) ) {
  				return;
  			}

			// if ( _attacker.Colliders. )
			var interactable = collision.GetComponentInChildren<Eden.Interactable.InteractableObject>();

			if ( interactable && interactable.Hitable ){
				interactable.HitDelegate.Hit( _attacker, _hitData );
			}
  		}
	}
	private void Update () {

		transform.position = _attacker.MeleeSpawner.position;
		transform.rotation = _attacker.MeleeSpawner.rotation;
	}
}
