using UnityEngine;
using Application;

public class Game : MonoBehaviour {

	// ************ STATIC ****************

	public static Game Instance {
		set { _instance = value;  _instance.OnSetInstance(); }
		get { return _instance; }
	}	
	public static Effects Effects {
		get{ return Game.Instance._effects; } 
	}
	public static UIController UIController{
		get{ return Game.Instance._uiController; }
	}
	public static GunControl GunControl {
		get{ return Game.Instance._gunControl; }
	}
	public static InputStack Input {
		get{ return Game.Instance._input; } 
	}

	// ************ PRIVATE ****************

	private static Game _instance;
	private Effects _effects;
	private UIController _uiController;
	private GunControl _gunControl;
	private InputStack _input;

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
		
		// reset all input	
		_input.Reset();
	}

	// ******************************

	private void BuildGame () {

		// Effects
		_effects = gameObject.AddComponent<Effects>();

		// UIController
		_uiController = gameObject.AddComponent<UIController>();

		// Gun Control
		_gunControl = gameObject.AddComponent<GunControl>();

		// Input
		_input = gameObject.AddComponent<InputStack>();
	}
	private void InitGame () {
		
		// Gun Control
		_gunControl.Init();

		// Input
		_input.Init();

		// Async

		// Effects

		// UIController
		_uiController.Init();
	}
	private void PlayGame () {
	}
}











