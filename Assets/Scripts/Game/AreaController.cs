using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaController : MonoBehaviour {

	
	// ********************************
	
	public delegate void OnChangeAreaEvent( int doorID );
	public OnChangeAreaEvent OnChangeArea;

	public void Init () {

		var prefab = Resources.Load( SCREEN_WIPE_PREFAB_PATH ) as GameObject;
		_screenWipeInstance = Instantiate( prefab ).GetComponent<Image>();
		_screenWipeInstance.transform.SetParent( transform );
	}
	public void ChangeArea ( Area.Identifier id, int doorID ) {
		
		Time.timeScale = 0;

		StartCoroutine( FadeOut( () => {

			var areaName = GetAreaIdentifierName( id );
			SceneManager.LoadScene( areaName );
		
			Game.Async.WaitForAreaToLoad( areaName, () => { 
				
				StartCoroutine( FadeIn() );
				Time.timeScale = 1;

				if( OnChangeArea != null ) {
					OnChangeArea( doorID );
				}
			});
		}));
	}

	// ********************************

	private const string SCREEN_WIPE_PREFAB_PATH = "ScreenWipe";
	private const float TRANSITION_DURATION = 0.2f;
	private const string PROPERTY_NAME = "_Fade";

	private Image _screenWipeInstance;

	// ********************************

	private string GetAreaIdentifierName( Area.Identifier id ){

		switch( id ){

			case Area.Identifier.Farm:
				return "Farm";

			case Area.Identifier.Dungeon:
				return "Dungeon";

			case Area.Identifier.Town:
				return "Town";

			case Area.Identifier.CraftingArea:
				return "CraftingArea";

			case Area.Identifier.SmallHome:
				return "SmallHome";

			case Area.Identifier.GunShop:
				return "Gun Shop";

			case Area.Identifier.GardeningShop:
				return "Gardening Shop";

			case Area.Identifier.GeneralStore:
				return "General Store";

			case Area.Identifier.Home:
				return "Home";

			case Area.Identifier.Housing:
				return "Housing";
				
			default:
				return "Farm";
		}
	}

	// ********************************

	private IEnumerator FadeOut ( Action onComplete ) {

		for( float i = 0; i<TRANSITION_DURATION; i+= Time.fixedDeltaTime ){
			var frac = i/TRANSITION_DURATION;
			_screenWipeInstance.material.SetFloat( PROPERTY_NAME, frac );
			yield return null;
		}
		_screenWipeInstance.material.SetFloat( PROPERTY_NAME, 1f );

		onComplete();
	}
	private IEnumerator FadeIn (){

		for( float i = 0; i<TRANSITION_DURATION; i+= Time.fixedDeltaTime ){
			var frac = 1 - i/TRANSITION_DURATION;
			_screenWipeInstance.material.SetFloat( PROPERTY_NAME, frac );
			yield return null;
		}

		_screenWipeInstance.material.SetFloat( PROPERTY_NAME, 0f );
	}
}
