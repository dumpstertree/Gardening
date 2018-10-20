using Eden.Model.Interactable;
using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour {

	public void Set ( Eden.Life.BlackBox attacker , Hit hitData ) {
		
		// _attacker = attacker;
		_hitData = hitData;
	}


	[SerializeField] private LayerMask _layermask;
	[SerializeField] private float _size;
	[SerializeField] private float _duration;

	// private Eden.Life.BlackBox _attacker;
	private Hit _hitData;

	private void Awake () {

		transform.localScale = Vector3.zero;
		StartCoroutine( Expand() );
	}
	private IEnumerator Expand () {

		for( float t=0f; t<_duration; t+=Time.deltaTime ){

			var frac = t/_duration;
			transform.localScale = new Vector3( frac * _size, frac * _size, frac * _size );
			yield return null;
		}

		yield return new WaitForSeconds( .25f );

		Destroy( gameObject );
	}
	private void OnTriggerEnter( Collider collision ) {

  		if( _layermask == (_layermask | (1 << collision.gameObject.layer) ) ) {
			
			var interactable = collision.GetComponentInChildren<Eden.Interactable.InteractableObject>();

			if ( interactable && interactable.Hitable ){
				interactable.HitDelegate.Hit( _hitData );
			}
  		}
	}
}
