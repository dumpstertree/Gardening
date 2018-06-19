using UnityEngine;
namespace Eden {
	
	public class PS4InputDelegate : IInput {

		private const string FACE_UP_ID 	= "Face_Up";
		private const string FACE_DOWN_ID 	= "Face_Down";
		private const string FACE_LEFT_ID 	= "Face_Left";
		private const string FACE_RIGHT_ID 	= "Face_Right";

		private const string DPAD_HORIZONTAL_ID = "DPad_Horizontal";
		private const string DPAD_VERTICAL_ID 	= "DPad_Vertical";

		private const string ANALOG_RIGHT_HORIZONTAL_ID = "Analog_Right_Horizontal";
		private const string ANALOG_RIGHT_VERTICAL_ID = "Analog_Right_Vertical";
		
		private const string ANALOG_LEFT_HORIZONTAL_ID = "Analog_Left_Horizontal";
		private const string ANALOG_LEFT_VERTICAL_ID = "Analog_Left_Vertical";

		private const string BACK_LEFT_BUMPER_ID = "Back_Left_Bumper";
		private const string BACK_RIGHT_BUMPER_ID = "Back_Right_Bumper";

		Input.Package IInput.GetPackage () {

			return new Input.Package( 
				GetFaceButtons(),
				GetDpadButtons(),
				GetLeftAnalog(),
				GetRightAnalog(),
				GetLeftBackAnalog(),
				GetRightBackAnalog()
			 );
		}

		private Input.Package.Buttons GetFaceButtons () {

			return new Input.Package.Buttons( 
				
				UnityEngine.Input.GetButton( FACE_UP_ID ),
				UnityEngine.Input.GetButtonDown( FACE_UP_ID ),
				UnityEngine.Input.GetButtonUp( FACE_UP_ID ),
				
				UnityEngine.Input.GetButton( FACE_DOWN_ID ),
				UnityEngine.Input.GetButtonDown( FACE_DOWN_ID ),
				UnityEngine.Input.GetButtonUp( FACE_DOWN_ID ),
				
				UnityEngine.Input.GetButton( FACE_LEFT_ID ),
				UnityEngine.Input.GetButtonDown( FACE_LEFT_ID ),
				UnityEngine.Input.GetButtonUp( FACE_LEFT_ID ),

				UnityEngine.Input.GetButton( FACE_RIGHT_ID ),
				UnityEngine.Input.GetButtonDown( FACE_RIGHT_ID ),
				UnityEngine.Input.GetButtonUp( FACE_RIGHT_ID )
			);
		}

		private bool _lastUp;
		private bool _lastDown;
		private bool _lastLeft;
		private bool _lastRight;

		private Input.Package.Buttons GetDpadButtons () {

			var horizontal = UnityEngine.Input.GetAxis( DPAD_HORIZONTAL_ID );
			var vertical = UnityEngine.Input.GetAxis( DPAD_VERTICAL_ID );
			
			var up = Mathf.RoundToInt( vertical ) == 1;
			var down = Mathf.RoundToInt( vertical ) == -1;
			var left = Mathf.RoundToInt( horizontal ) == -1;
			var right = Mathf.RoundToInt( horizontal ) == 1;

			var buttons =  new Input.Package.Buttons(

				up,
				up && !_lastUp,
				!up && _lastUp,
				
				down,
				down && !_lastDown,
				!down && _lastDown,
				
				left,
				left && !_lastLeft,
				!left && _lastLeft,

				right,
				right && !_lastRight,
				!right && _lastRight
			);

			_lastUp = up;
			_lastDown = down;
			_lastLeft = left;
			_lastRight = right;

			return buttons;
		}
		private Input.Package.Analog GetLeftAnalog () {
			
			return new Input.Package.Analog(

				UnityEngine.Input.GetAxis( ANALOG_LEFT_HORIZONTAL_ID ),
				UnityEngine.Input.GetAxis( ANALOG_LEFT_VERTICAL_ID )
			);
		}
		private Input.Package.Analog GetRightAnalog () {
			
			return new Input.Package.Analog(

				UnityEngine.Input.GetAxis( ANALOG_RIGHT_HORIZONTAL_ID ),
				UnityEngine.Input.GetAxis( ANALOG_RIGHT_VERTICAL_ID )
			);
		}
		private Input.Package.Back GetLeftBackAnalog () {

			return new Input.Package.Back ( 
				0f,
				UnityEngine.Input.GetButton( BACK_LEFT_BUMPER_ID ),
				UnityEngine.Input.GetButtonDown( BACK_LEFT_BUMPER_ID ),
				UnityEngine.Input.GetButtonUp( BACK_LEFT_BUMPER_ID )
			 );
		}
		private Input.Package.Back GetRightBackAnalog () {

			return new Input.Package.Back ( 
				0f,
				UnityEngine.Input.GetButton( BACK_RIGHT_BUMPER_ID ),
				UnityEngine.Input.GetButtonDown( BACK_RIGHT_BUMPER_ID ),
				UnityEngine.Input.GetButtonUp( BACK_RIGHT_BUMPER_ID )
			 );
		}
	}
}