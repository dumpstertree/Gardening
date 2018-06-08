using System;
using System.Collections;
using UnityEngine;

namespace Dumpster.Core.BuiltInModules {
	
	public class Async : Module {

		protected override void OnRun () {}
		protected override void OnInstall () {}

		// public void WaitForSeconds( float seconds, Action onStart, Action<float> onWait, Action onComplete ) {
		// 	StartCoroutine( WaitForSecondsCoroutine( seconds, onStart, onWait, onComplete ) );
		// }
		// public void WaitForSeconds( float seconds, Action onComplete ) {
		// 	StartCoroutine( WaitForSecondsCoroutine( seconds, onComplete ) );
		// }
		// public void WaitForAreaToLoad( string areaName, Action onComplete ){
		// 	StartCoroutine( WaitForAreaToLoadCoroutine( areaName, onComplete ) );
		// }
		// public void WaitForEndOfFrame( Action onComplete ){
		// 	StartCoroutine( WaitForEndOfFrameCoroutine( onComplete ) );
		// }

		// IEnumerator WaitForSecondsCoroutine( float seconds, Action onStart, Action<float> onWait, Action onComplete  ) {

		// 	if ( onStart != null ) { onStart(); }
			
		// 	for (float t = 0f; t<seconds; t+=Time.deltaTime ) {
		// 		if ( onWait != null ) { onWait( t ); }
		// 		yield return null;
		// 	}

		// 	if ( onComplete != null ) onComplete();
		// }
		// IEnumerator WaitForSecondsCoroutine( float seconds, Action onComplete ) {
		// 	yield return new WaitForSeconds( seconds );
		// 	onComplete();
		// }
		// IEnumerator WaitForAreaToLoadCoroutine( string areaName, Action onComplete ) {
	 //   		// yield return SceneManager.LoadSceneAsync( areaName );
		// 	onComplete();
		// }
		// IEnumerator WaitForEndOfFrameCoroutine( Action onComplete ) {
	 //   		yield return new WaitForEndOfFrame();
		// 	onComplete();
		// }

	}
}