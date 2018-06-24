using UnityEngine;

public class Game : MonoBehaviour {

	// ************ STATIC ****************

	public static Game Instance {
		set { _instance = value;  _instance.OnSetInstance(); }
		get { return _instance; }
	}	
	public static GunControl GunControl {
		get{ return Game.Instance._gunControl; }
	}

	// ************ PRIVATE ****************

	private static Game _instance;
	private GunControl _gunControl;

	// ******************************
	
	private void Awake () {

		if ( Game.Instance == null ){ Game.Instance = this; }
		else{ Destroy( gameObject ); }
	}

	// ******************************

	private void OnSetInstance () {
		
		// DontDestroyOnLoad( gameObject );

		BuildGame();
		InitGame();
		PlayGame();
	}
	private void OnChangeArea ( int doorID ) {
		
	}

	// ******************************

	private void BuildGame () {

		// Gun Control
		_gunControl = gameObject.AddComponent<GunControl>();
	}
	private void InitGame () {
		
		// Gun Control
		_gunControl.Init();

	}
	private void PlayGame () {
	}
}











