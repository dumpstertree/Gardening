using UnityEngine;

public class EdensGarden : Dumpster.Core.Game {

	public struct Constants {
		
		public struct InputLayers {

			public const string Player = "PLAYER";
			public const string Dialog = "DIALOG";
			public const string InventoryUI = "INVENTORY_UI";
			public const string ForegroundUI = "FOREGROUND";
			public const string MidgroundUI = "MIDGROUND";
			public const string BackgroundUI = "BACKGROUND";
		}
		
		public struct UIPanels {

			public const string Ammo = "AMMO";
			public const string MenuButtons = "MENU_BUTTONS";
			public const string Quicklot = "QUICKSLOT";
		}
		
		public struct UIContexts {

			public const string None = "NONE";
			public const string Player = "PLAYER";
			public const string Dialog = "DIALOG";
			public const string Inventory = "INVENTORY";
		}
		
		public struct UILayers {

			public const string Foreground = "FOREGROUND";
			public const string Midground = "MIDGROUND";
			public const string Background = "BACKGROUND";
		}
		public struct NewUILayers {

			public const int Foreground = 2;
			public const int Midground  = 1;
			public const int Background = 0;
		}
	}


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
	public Dumpster.Core.BuiltInModules.UI.Controller UI {
		get; private set;
	}

	public Eden.Input Input {
		get; private set;
	}
	public Eden.Targeting Targeting {
		get; private set;
	}

	[SerializeField] private TransitionCreator _transitionCreator;
	[SerializeField] private Eden.UI.ContextDelegate _contextDelegate;
	
	protected override void BuildGame () {

		// Default Modules
		Async   = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Async ) ) as Dumpster.Core.BuiltInModules.Async;
		Camera  = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Camera ) ) as Dumpster.Core.BuiltInModules.Camera;
		Rooms   = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Rooms.Controller ) ) as Dumpster.Core.BuiltInModules.Rooms.Controller;
		Effects = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Effects.Controller ) ) as Dumpster.Core.BuiltInModules.Effects.Controller;
		UI 		= Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.UI.Controller ) ) as Dumpster.Core.BuiltInModules.UI.Controller;

		// Subclass Modules
		Input     = Dumpster.Core.Module.Install( this, typeof( Eden.Input )) as Eden.Input;
		Targeting = Dumpster.Core.Module.Install( this, typeof( Eden.Targeting )) as Eden.Targeting;

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
		UI.Init ();
		Targeting.Init ();
	}
	protected override void PlayGame () {

		UI.ContextDelgate = _contextDelegate;

		Rooms.OnChangeArea += FireOnSceneChangedEvent;

		Input.RequestInput( Constants.InputLayers.Player );

		Rooms.Run ();
		Async.Run ();
		Camera.Run ();
		Input.Run ();
		Effects.Run (); 
		UI.Run ();
		Targeting.Run ();


		UI.Present( Constants.NewUILayers.Midground, Constants.UIContexts.Player );
	}
}

