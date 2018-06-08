using UnityEngine;

public class DynamicCamera : CameraSystem {

	[SerializeField] private CameraMovement _camera;
	protected override void OnInit () {

		// var camera = Instantiate( _camera );
		// camera.SetupCamera( Game.Area.LoadedPlayer.CameraTarget, Game.Area.LoadedPlayer.CameraFocus );
		// camera.transform.SetParent( transform, false );
	}
}
