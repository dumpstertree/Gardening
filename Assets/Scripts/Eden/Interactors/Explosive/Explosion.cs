using System.Collections;
using UnityEngine;
using Dumpster.Core;
using Dumpster.Characteristics;

public class Explosion : MonoBehaviour {

	public void Set () {
		
	}


	[SerializeField] private LayerMask _layermask;
	[SerializeField] private float _size;
	[SerializeField] private float _duration;


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
			
			var actor = collision.GetComponent<Actor>();
			if ( actor != null ) {
				
				actor.GetCharacteristic<Damageable>()?.Damage();
			}
  		}
	}
}
