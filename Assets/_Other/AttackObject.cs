using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackObject : MonoBehaviour {

	[SerializeField] private Transform _attack;
	[SerializeField] private List<AttackPoint> _attackPoints;

	private int _attackPointIndex = -1;

	
	private void Start() {
		
		_attack.transform.localPosition = _attackPoints[0].Transform.localPosition;
		_attack.transform.rotation = _attackPoints[0].Transform.localRotation;

		MoveNext();
	}
	private void MoveNext () {
		
		_attackPointIndex++;

		if ( _attackPointIndex < _attackPoints.Count ){
			_attackPoints[ _attackPointIndex ].Move( this , _attack, MoveNext );
		}
		else{
			Destroy();
		}
	}
	private void Destroy () {

		Destroy( transform.parent.gameObject );
	}

	private void OnTriggerEnter(Collider other) {
		
		var interactable = other.GetComponent<Interactable.InteractableObject>();
		
		if ( interactable && interactable.Hitable ) {
			var hitData = new HitData();
			hitData.Power = 1;
			interactable.HitDelegate.Hit( null, hitData );
		}
	}

	private void OnDrawGizmos () {

		for ( int i = 0; i < _attackPoints.Count; i++ ){

			var p1 = _attackPoints[i];	

			if ( p1.Transform ) {
				
				Gizmos.color = Color.red;
				Gizmos.DrawRay( p1.Transform.position, p1.Transform.forward );
				Gizmos.DrawWireSphere( p1.Transform.position, 0.1f );
			}
		}

		for ( int i = 0; i < _attackPoints.Count-1; i++ ){

			var p1 = _attackPoints[i];
			var p2 = _attackPoints[i+1];

			if ( p1.Transform && p2.Transform ) {
				
				Gizmos.color = Color.green;
				Gizmos.DrawLine( p1.Transform.position, p2.Transform.position );
			}
		}
	}
	

	[System.Serializable]
	private class AttackPoint {
		
		[SerializeField] public Transform Transform;
		[SerializeField] public float TimeToNext;
		[SerializeField] public float WaitTime;
		[SerializeField] public Application.Effects.Type Effect;

		public void Move ( AttackObject attack, Transform attackObject, Action onComplete ) {
			
			attack.StartCoroutine( Move ( attackObject, onComplete ) );
		} 

		private IEnumerator Move ( Transform attackObject, Action onComplete ) {

			var startPos = attackObject.localPosition;
			var startRot = attackObject.localRotation;

			for ( float t = 0f; t< TimeToNext; t += Time.deltaTime ) {
				
				var frac = t/TimeToNext;

				attackObject.transform.localPosition = Vector3.Lerp( startPos, Transform.localPosition, frac );
				attackObject.transform.localRotation = Quaternion.Slerp( startRot, Transform.localRotation, frac );
				yield return null;
			}

			yield return new WaitForSeconds ( WaitTime );

			if ( Effect != Application.Effects.Type.None ) {
				Game.Effects.OneShot( Effect, Transform.position, Transform.rotation );
			}

			onComplete();
		}
	}
}


