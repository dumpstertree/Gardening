using UnityEngine;

public class EdensGarden : Dumpster.Core.Game {

	// game
	public static EdensGarden Instance {
		get{ return _instance as EdensGarden; }
	}

	// modules
	public Dumpster.Core.BuiltInModules.Async Async {
		get; private set;
	}
	public Dumpster.Core.BuiltInModules.Camera Camera {
		get; private set;
	}
	public Dumpster.Core.BuiltInModules.Rooms.Controller Rooms {
		get; private set;
	}
	public Dumpster.Core.BuiltInModules.Effects.Controller Effects {
		get; private set;
	}

	public Eden.Input Input {
		get; private set;
	}

	[SerializeField] private TransitionCreator _transitionCreator;
	
	protected override void BuildGame () {

		// Default Modules
		Async   = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Async ) ) as Dumpster.Core.BuiltInModules.Async;
		Camera  = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Camera ) ) as Dumpster.Core.BuiltInModules.Camera;
		Rooms   = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Rooms.Controller ) ) as Dumpster.Core.BuiltInModules.Rooms.Controller;
		Effects = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Effects.Controller ) ) as Dumpster.Core.BuiltInModules.Effects.Controller;

		// Subclass Modules
		Input  = Dumpster.Core.Module.Install( this, typeof( Eden.Input )) as Eden.Input;

		// Custom Modules
		var t = Instantiate( Instance._transitionCreator );
		t.transform.SetParent( Instance.transform, false );
	}
	protected override void InitGame () {

		Async.Init ();
		Camera.Init ();
		Rooms.Init ();
		Input.Init ();
		Effects.Init (); 
	}
	protected override void PlayGame () {

		Rooms.OnChangeArea += FireOnSceneChangedEvent;

		Input.RequestInput( Constants.InputLayers.Player );

		Rooms.Run ();
		Async.Run ();
		Camera.Run ();
		Input.Run ();
		Effects.Run (); 

	}
	private void Start () {

		Game.UIController.ChangeContext( UIController.UiContext.Identifier.Farm );
	}

	public struct Constants {
		
		public struct InputLayers {

			public static string Player = "PLAYER";
			public static string Dialog = "DIALOG";
		}

		public struct UILayers {

		}
	}
}

