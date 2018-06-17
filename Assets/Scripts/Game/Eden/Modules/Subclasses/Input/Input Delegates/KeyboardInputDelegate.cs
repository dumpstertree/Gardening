using UnityEngine;

namespace Eden {

	public class KeyboardInputDelegate : IInput {

		private KeyCode FACE_UP = KeyCode.W;
		private KeyCode FACE_DOWN = KeyCode.Space;
		private KeyCode FACE_LEFT = KeyCode.A;
		private KeyCode FACE_RIGHT = KeyCode.D;

		private KeyCode DPAD_UP = KeyCode.I;
		private KeyCode DPAD_DOWN = KeyCode.K;
		private KeyCode DPAD_LEFT = KeyCode.J;
		private KeyCode DPAD_RIGHT = KeyCode.L;

		Input.Package IInput.GetPackage () {

			return new Input.Package( 
				GetFaceButtons(),
				GetDpadButtons(),
				GetLeftAnalog(),
				GetRightAnalog()
			 );
		}

		private Input.Package.Buttons GetFaceButtons () {
			
			return new Input.Package.Buttons(

				UnityEngine.Input.GetKey( FACE_UP ),
				UnityEngine.Input.GetKeyDown( FACE_UP ),
				UnityEngine.Input.GetKeyUp( FACE_UP ),

				UnityEngine.Input.GetKey( FACE_DOWN ),
				UnityEngine.Input.GetKeyDown( FACE_DOWN ),
				UnityEngine.Input.GetKeyUp( FACE_DOWN ),
				
				UnityEngine.Input.GetKey( FACE_LEFT ),
				UnityEngine.Input.GetKeyDown( FACE_LEFT ),
				UnityEngine.Input.GetKeyUp( FACE_LEFT ),
				
				UnityEngine.Input.GetKey( FACE_RIGHT ),
				UnityEngine.Input.GetKeyDown( FACE_RIGHT ),
				UnityEngine.Input.GetKeyUp( FACE_RIGHT )
			);
		}
		private Input.Package.Buttons GetDpadButtons () {
			
			return new Input.Package.Buttons(

				UnityEngine.Input.GetKey( DPAD_UP ),
				UnityEngine.Input.GetKeyDown( DPAD_UP ),
				UnityEngine.Input.GetKeyUp( DPAD_UP ),

				UnityEngine.Input.GetKey( DPAD_DOWN ),
				UnityEngine.Input.GetKeyDown( DPAD_DOWN ),
				UnityEngine.Input.GetKeyUp( DPAD_DOWN ),
				
				UnityEngine.Input.GetKey( DPAD_LEFT ),
				UnityEngine.Input.GetKeyDown( DPAD_LEFT ),
				UnityEngine.Input.GetKeyUp( DPAD_LEFT ),
				
				UnityEngine.Input.GetKey( DPAD_RIGHT ),
				UnityEngine.Input.GetKeyDown( DPAD_RIGHT ),
				UnityEngine.Input.GetKeyUp( DPAD_RIGHT )
			);
		}
		private Input.Package.Analog GetLeftAnalog () {
			
			return new Input.Package.Analog( 
			
				UnityEngine.Input.GetAxis( "Horizontal" ),
				UnityEngine.Input.GetAxis( "Vertical" ) );
		}
		private Input.Package.Analog GetRightAnalog () {
			
			return new Input.Package.Analog( 
			
				UnityEngine.Input.GetAxis( "Horizontal" ),
				UnityEngine.Input.GetAxis( "Vertical" ) );
		}
	}
}