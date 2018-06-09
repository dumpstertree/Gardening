using UnityEngine;
using Application;

public class Game : MonoBehaviour {

	// ************ STATIC ****************

	public static Game Instance {
		set { _instance = value;  _instance.OnSetInstance(); }
		get { return _instance; }
	}	
	public static Async Async {
		get{ return Game.Instance._async; } 
	}
	public static Effects Effects {
		get{ return Game.Instance._effects; } 
	}
	public static AreaController AreaController {
		get{ return Game.Instance._areaController; }
	}
	public static UIController UIController{
		get{ return Game.Instance._uiController; }
	}
	public static Area Area{
		get{ return Game.Instance._area; }
	}
	public static GunControl GunControl {
		get{ return Game.Instance._gunControl; }
	}
	public static InputStack Input {
		get{ return Game.Instance._input; } 
	}

	// ************ PRIVATE ****************

	private static Game _instance;
	private Async _async;
	private Effects _effects;
	private AreaController _areaController;
	private UIController _uiController;
	private Area _area;
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

		// load new area
		_area = FindObjectOfType<Area>();
		_area.Init();
		_area.EnterArea( doorID );
	}

	// ******************************

	private void BuildGame () {
		
		// Area
		_area = FindObjectOfType<Area>();

		// Async
		_async = gameObject.AddComponent<Async>();

		// Effects
		_effects = gameObject.AddComponent<Effects>();

		// Area Controller
		_areaController = gameObject.AddComponent<AreaController>();

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

		// Area
		//_area.Init();

		// Async

		// Effects

		// Area Controller
		_areaController.Init();

		// UIController
		_uiController.Init();
	}
	private void PlayGame () {
		
		_areaController.OnChangeArea += OnChangeArea;
		_area.EnterArea( 0 );
	}
}











