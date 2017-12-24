using System.Collections;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace Application {
	
	public class Async : MonoBehaviour {

		public void WaitForSeconds( float seconds, Action onComplete ) {
			StartCoroutine( WaitForSecondsCoroutine( seconds, onComplete ) );
		}
		public void WaitForAreaToLoad( string areaName, Action onComplete ){
			StartCoroutine( WaitForAreaToLoadCoroutine( areaName, onComplete ) );
		}

		IEnumerator WaitForSecondsCoroutine( float seconds, Action onComplete ) {

			yield return new WaitForSeconds( seconds );
			onComplete();
		}
		IEnumerator WaitForAreaToLoadCoroutine( string areaName, Action onComplete ) {

       		yield return SceneManager.LoadSceneAsync( areaName );
			onComplete();
		}

	}
}