using System.Collections;
using UnityEngine;

namespace Dumpster.Events {

	public class ChangeColor : SmartEvent {

		[Header( "Settings")]
		[SerializeField] private MeshRenderer _renderer;
		[SerializeField] private Color _targetColor;
		[SerializeField] private float _lerpTime;

		[Header( "Events" )]
		[SerializeField] private SmartEvent[] _onColorChange;


		//***************** Public *******************
		
		public override void EventTriggered() {

			StartCoroutine( LerpCoroutine( FireOnColorChange ) );
		}

		private void FireOnColorChange () {

			foreach ( SmartEvent e in _onColorChange ) {
				e.EventTriggered();
			}
		}

		private IEnumerator LerpCoroutine ( System.Action onComplete ) {

			var startColor = _renderer.material.color;
			for ( float t=0f; t<_lerpTime; t+=Time.deltaTime ) {

				var progress = t/_lerpTime;
				_renderer.material.color = Color.Lerp( startColor, _targetColor, progress  );
				yield return null;
			}


			_renderer.material.color = _targetColor;

			if ( onComplete != null ) {
				onComplete();
			}
		}	
	}
}