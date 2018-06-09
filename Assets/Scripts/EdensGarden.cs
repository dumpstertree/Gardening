using System.Collections;
using System.Collections.Generic;
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

	[SerializeField] private TransitionCreator _transitionCreator;
	
	protected override void BuildGame () {

		Async  = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Async ) ) as Dumpster.Core.BuiltInModules.Async;
		Camera = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Camera ) ) as Dumpster.Core.BuiltInModules.Camera;
		Rooms  = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Rooms.Controller ) ) as Dumpster.Core.BuiltInModules.Rooms.Controller;

		var t = Instantiate( Instance._transitionCreator );
		t.transform.SetParent( Instance.transform, false );
	}
	protected override void InitGame () {

		Async.Init ();
		Camera.Init ();
		Rooms.Init ();
	}
	protected override void PlayGame () {

		Rooms.OnChangeArea += FireOnSceneChangedEvent;

		Rooms.Run();
		Async.Run ();
		Camera.Run ();
	}
}

