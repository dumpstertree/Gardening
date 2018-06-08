using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdensGarden : Dumpster.Core.Game {

	public Dumpster.Core.BuiltInModules.Async Async {
		get; private set;
	}
	public Dumpster.Core.BuiltInModules.Camera Camera {
		get; private set;
	}

	protected override void BuildGame () {

		Async  = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Async ) ) as Dumpster.Core.BuiltInModules.Async;
		Camera = Dumpster.Core.Module.Install( this, typeof( Dumpster.Core.BuiltInModules.Camera ) ) as Dumpster.Core.BuiltInModules.Camera;
	}
	protected override void InitGame () {

		Async.Init ();
		Camera.Init ();
	}
	protected override void PlayGame () {

		Async.Run ();
		Camera.Run ();
	}
}

