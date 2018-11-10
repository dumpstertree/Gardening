using System;
using System.Collections;
using UnityEngine;

namespace Dumpster.Core.BuiltInModules {
	
	[CreateAssetMenu(menuName = "Dumpster/Modules/Async")]
	public class Async : Module {

		public void WaitForSeconds( float seconds, Action onComplete ) {
			
			_game.StartCoroutine( WaitForSecondsCoroutine( seconds, onComplete ) );
		}
		public void WaitForEndOfFrame( Action onComplete ){
			
			_game.StartCoroutine( WaitForEndOfFrameCoroutine( onComplete ) );
		}

		IEnumerator WaitForSecondsCoroutine( float seconds, Action onComplete ) {
			yield return new WaitForSeconds( seconds );
			onComplete();
		}
		IEnumerator WaitForEndOfFrameCoroutine( Action onComplete ) {
	   		yield return new WaitForEndOfFrame();
			onComplete();
		}
	}
}