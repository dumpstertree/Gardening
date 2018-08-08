using UnityEngine;
using System.Collections.Generic;

namespace Dumpster.Core.BuiltInModules {
	
	public class Camera : Module {

		public enum Priority {
			Low,
			Medium,
			High
		}

	
		protected override void OnInit () {

			_lowPriorityControllerStack = new List<CameraController>();
			_mediumPriorityControllerStack = new List<CameraController>();
			_highPriorityControllerStack = new List<CameraController>();

			// create camera instance 
			_cameraInstance = Instantiate( Resources.Load( CAMERA_PATH ) ) as GameObject;
			_cameraTarget = new GameObject( "Camera Target" ).transform;
			
			_cameraInstance.transform.SetParent( transform, false );
			_cameraTarget.SetParent( transform, false );
			
		}
		protected override void OnRun () {

			FindCameraControllers ();
		}
		protected override void OnReload () {

			_lowPriorityControllerStack = new List<CameraController>();
			_mediumPriorityControllerStack = new List<CameraController>();
			_highPriorityControllerStack = new List<CameraController>();
			_defaultController = null;

			FindCameraControllers ();
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

			switch ( controller.Priority ) {
				
				case Priority.Low :
					if ( !_lowPriorityControllerStack.Contains( controller ) ) {
						_lowPriorityControllerStack.Add( controller );
					}
					break;
				
				case Priority.Medium :
					if ( !_mediumPriorityControllerStack.Contains( controller ) ) {
						_mediumPriorityControllerStack.Add( controller );
					}
					break;
				
				case Priority.High :
					if ( !_highPriorityControllerStack.Contains( controller ) ) {
						_highPriorityControllerStack.Add( controller );
					}
					break;
			} 
		}
		public void RelinquishControl ( CameraController controller ) {

			if ( _lowPriorityControllerStack.Contains( controller ) ) {
				_lowPriorityControllerStack.Remove( controller );
			}
			if ( _mediumPriorityControllerStack.Contains( controller ) ) {
				_mediumPriorityControllerStack.Remove( controller );
			}
			if ( _highPriorityControllerStack.Contains( controller ) ) {
				_highPriorityControllerStack.Remove( controller );
			}
		}


		// ******************** Private *************************

		private List<CameraController> _highPriorityControllerStack;
		private List<CameraController> _mediumPriorityControllerStack;
		private List<CameraController> _lowPriorityControllerStack;
		private CameraController _defaultController;
		
		private CameraController _lastController;
		private CameraController _controller {
			get{ 
				if ( _highPriorityControllerStack .Count > 0 ) {
					return _highPriorityControllerStack[ _highPriorityControllerStack.Count -1 ];
				} else if ( _mediumPriorityControllerStack .Count > 0 ) {
					return _mediumPriorityControllerStack[ _mediumPriorityControllerStack.Count -1 ];
				} else if ( _lowPriorityControllerStack .Count > 0 ) {
					return _lowPriorityControllerStack[ _lowPriorityControllerStack.Count -1 ];
				} else {
					return _defaultController;
				}
			}
		}
		private void FindCameraControllers () {
			
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
		private void LateUpdate () {
			
			if ( _controller != _lastController ) {

				if ( _controller != null ) {
					_controller.WillGainControl ();
				}
				if( _lastController != null ) {
					_lastController.WillLoseControl ();
				}
			}

			if ( _controller != null ) {
				_controller.Control ( _cameraTarget, _cameraFocus );
			}
			
			_cameraInstance.transform.position = _cameraTarget.position;
			_cameraInstance.transform.rotation = _cameraTarget.rotation;

			_lastController = _controller;
		}		
	}
}
