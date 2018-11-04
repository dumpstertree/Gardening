using UnityEngine;
using System.Collections;

namespace Dumpster.Events {

	public class Rotate : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private Transform _objectToRotate;
		[SerializeField] private Vector3 _targetRotation;
		[SerializeField] private float _rotationTime;

		[Header( "Events" )]
		[SerializeField] private SmartEvent[] _onRotate;

	
		//***************** Public *******************
		
		public override void EventTriggered() {

			StartCoroutine( RotateCoroutine( FireOnRotate ) );
		}

		private void FireOnRotate () {

			foreach ( SmartEvent e in _onRotate ) {
				e.EventTriggered();
			}
		}
		private IEnumerator RotateCoroutine ( System.Action onComplete ) {

			var startRotation = _objectToRotate.rotation;
			for ( float t=0f; t<_rotationTime; t+=Time.deltaTime ) {

				_objectToRotate.rotation = Quaternion.Slerp( startRotation, Quaternion.Euler(_targetRotation), t/_rotationTime );
				yield return null;
			}

			_objectToRotate.rotation = Quaternion.Euler(_targetRotation);

			if ( onComplete != null ) {
				onComplete();
			}
		}	
	}
}