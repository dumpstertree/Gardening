using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Core.BuiltInModules {
	
	public class Camera : Module {

		protected override void OnInstall () {}
		protected override void OnInit () {

			// create camera instance 
			foreach ( CameraController c in FindObjectsOfType<CameraController>() ) {

				var cameraController = c;

				cameraController.OnRequestControl += () => RequestControl( cameraController );
				cameraController.OnRelinquishControl += () => RelinquishControl( cameraController );

				if ( cameraController.IsDefaultController ) {
					if ( _defaultController == null ) { _defaultController = cameraController; } 
					else { Debug.LogWarning( "More than one Default Controller in Scene" ); }
				}
			}
		}
		protected override void OnRun () {

			_cameraInstance = Instantiate( Resources.Load( CAMERA_PATH ) ) as GameObject;
			_cameraTarget = new GameObject( "Camera Target" ).transform;
			
			_cameraInstance.transform.SetParent( transform, false );
			_cameraTarget.SetParent( transform, false );
			_cameraFocus.SetParent( transform, false );


			if ( _defaultController != null ) {

				// set to default state
			}
		}



		// ******************** Public *************************

		private GameObject _cameraInstance;
		private Transform _cameraTarget;
		private Transform _cameraFocus;

		private const string CAMERA_PATH = "Camera";

		public void SetFocus ( Transform focus ) {

			_cameraFocus = focus;
		}
		public void RequestControl ( CameraController controller ) {

			_controllerStack.Add( controller );
		}
		public void RelinquishControl ( CameraController controller ) {

			if ( _controllerStack.Contains( controller ) ) {
				_controllerStack.Remove( controller );
			}
		}


		// ******************** Private *************************

		private CameraController _defaultController;
		private List<CameraController> _controllerStack = new List<CameraController>();

		private CameraController _controller {
			get{  return ( _controllerStack.Count > 0 ) ? _controllerStack[ _controllerStack.Count - 1 ] : _defaultController; }
		}

		private void Update () {
			
			if ( _controller != null ) {
				_controller.Control ( _cameraTarget, _cameraFocus );
			}

			_cameraInstance.transform.position = _cameraTarget.position;
			_cameraInstance.transform.rotation = _cameraTarget.rotation;
		}		
	}
}
