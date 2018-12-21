using Dumpster.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Dumpster.Tweening {

	public class Tween {

		// ***************** Static *********************

		public static Tween Float (  Action<float> setter, float startValue, float targetValue, float time ) {

			var t = new Tween();
			t._tweenValue = Game.Instance.StartCoroutine( t.TweenValue( time, v => t.OnFloatValueChange( setter, startValue, targetValue, v ) ) );

			return t;
		}
		public static Tween Vector3 ( Action<Vector3> setter, Vector3 startValue, Vector3 targetValue, float time ) {

			var t = new Tween();
			t._tweenValue = Game.Instance.StartCoroutine( t.TweenValue( time, v => t.OnVector3ValueChange( setter, startValue, targetValue, v ) ) );

			return t;
		}
		public static Tween Color ( Action<Color> setter, Color startValue, Color targetValue, float time  ) {

			var t = new Tween();
			t._tweenValue = Game.Instance.StartCoroutine( t.TweenValue( time, v => t.OnColorValueChange( setter, startValue, targetValue, v ) ) );

			return t;
		}

		
		// ***************** Public *********************

	
		public void Kill () {

			Game.Instance.StopCoroutine( _tweenValue );
		}
		public Tween OnComplete( Action onComplete ) {

			_onComplete = onComplete;
			return this;
		}


		// ***************** Private *********************


		private Action _onComplete;
		private Coroutine _tweenValue;

		private void OnFloatValueChange ( Action<float> setter, float startPos, float targetPos, float progress ) {

			var val = UnityEngine.Mathf.Lerp( startPos, targetPos, progress );
			setter( val );
		}
		private void OnVector3ValueChange ( Action<Vector3> setter, Vector3 startPos, Vector3 targetPos, float progress ) {

			var val = UnityEngine.Vector3.Lerp( startPos, targetPos, progress );
			setter( val );
		}
		private void OnColorValueChange ( Action<Color> setter, Color startColor, Color targetColor, float progress ) {

			var val = UnityEngine.Color.Lerp( startColor, targetColor, progress );
			setter( val );
		}
		private IEnumerator TweenValue ( float time, Action<float> onValueChange ) {


			for( float t=0;t<time; t+=Time.deltaTime ){
				onValueChange( t/time );
				yield return null;
			}


			onValueChange( 1f );


			if ( _onComplete != null ) {
				_onComplete ();
			}
		}
	}
}