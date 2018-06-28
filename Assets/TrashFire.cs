using UnityEngine;
using System;
using System.Collections.Generic;

public class TrashFire : MonoBehaviour {

	private bool _hasBeenFound;

	private List<string> _memory = new List<string>();

	private void Update () {

		if ( _hasBeenFound ){
			return;
		}
		
		var methodName = "";
		while ( _memory.Contains( methodName ) ) {
		
			methodName = "";
			var numOfLetters = UnityEngine.Random.Range( 0, 10 );
			for ( int i=0; i<numOfLetters; i++) {
				methodName += (char)('A' + UnityEngine.Random.Range (0,26));
			}
		}
		_memory.Add( methodName );


		try {
			Invoke( methodName, 0 );
		}  catch {
			Debug.LogWarning( "No function titled " + methodName + " found. I'll try again" );
		}
	}
	private void FOUNDME () {

		_hasBeenFound = true;
		Debug.Log( "You Found Me! Took you :" + Time.timeSinceLevelLoad );
	}
}