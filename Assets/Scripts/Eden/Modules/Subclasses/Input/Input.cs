namespace Eden {
	
	public class Input : Dumpster.Core.BuiltInModules.Input.Controller<Input.Package> {

		private Package _lastPackage;
		private IInput _ps4Controller = new PS4InputDelegate();
		private IInput _keyboardController = new KeyboardInputDelegate(); 

		private bool _hasController {
			get{ return UnityEngine.Input.GetJoystickNames().Length > 0; }
		}

		protected override Package PollPackage () {
			
			if ( _hasController ) {
				return _ps4Controller.GetPackage();
			} else {
				return _keyboardController.GetPackage();
			}
		} 	
		protected override Package GetEmptyPackage () {
			
			return new Package ();
		}


		private void Update () {

			var package = PollPackage();

			if ( _lastPackage == null || ShouldUpdate( package, _lastPackage ) ) {
				
				PushInputPackage( package );
				_lastPackage = package;
			}
		}
		private bool ShouldUpdate ( Package package1, Package package2 ) {

			if ( !Package.Buttons.Equal( package1.Face, package2.Face) ) return true;
			if ( !Package.Buttons.Equal( package1.Dpad, package2.Dpad) ) return true;

			if ( !Package.Analog.Equal( package1.LeftAnalog, package2.LeftAnalog) ) return true;
			if ( !Package.Analog.Equal( package1.RightAnalog, package2.RightAnalog) ) return true;

			if ( !Package.Back.Equal( package1.BackLeft, package2.BackLeft) ) return true;
			if ( !Package.Back.Equal( package1.BackRight, package2.BackRight) ) return true;
			
			if ( package2.Dpad.Up ) return true;
			if ( package2.Dpad.Down ) return true;
			if ( package2.Dpad.Left ) return true;
			if ( package2.Dpad.Right ) return true;

			if ( package2.Face.Up ) return true;
			if ( package2.Face.Down ) return true;
			if ( package2.Face.Left ) return true;
			if ( package2.Face.Right ) return true;

			if ( package2.BackLeft.Bumper ) return true;
			if ( package2.BackRight.Bumper ) return true;


			return false;
		}

		public class Package {
			
			public Buttons Face { get; } 
			public Buttons Dpad { get; } 
			public Analog LeftAnalog { get; }
			public Analog RightAnalog { get; }
			public Back BackLeft { get; }
			public Back BackRight { get; }

			public class Back {
				
				public float Trigger { get; } 

				public bool Bumper { get; }
				public bool Bumper_Down { get; }
				public bool Bumper_Up { get; }

				public Back () {
					
					Trigger = 0f;
					
					Bumper = false;
					Bumper_Down = false;
					Bumper_Up = false;
				}
				public Back ( float trigger, bool bumper, bool bumperDown, bool bumperUp ) {

					Trigger = trigger;
					
					Bumper = bumper;
					Bumper_Down = bumperDown;
					Bumper_Up = bumperUp;
				}
				public static bool Equal ( Back back1, Back back2 ) {

					if (back1.Bumper != back2.Bumper) return false;
					if (back1.Bumper_Down != back2.Bumper_Down) return false;
					if (back1.Bumper_Up != back2.Bumper_Up) return false;
					if (back1.Trigger != back2.Trigger ) return false;
					
					return true;
				}
			}
			public class Buttons {

				public bool Up { get; }
				public bool Up_Down { get; }
				public bool Up_Up { get; }

				public bool Down { get; }
				public bool Down_Down { get; }
				public bool Down_Up { get; }

				public bool Left { get; }
				public bool Left_Down { get; }
				public bool Left_Up { get; }

				public bool Right { get; }
				public bool Right_Down { get; }
				public bool Right_Up { get; }

				public Buttons () {

					Up = false;
					Up_Down = false;
					Up_Up = false;

					Down = false;
					Down_Down = false;
					Down_Up = false;
					
					Left = false;
					Left_Down = false;
					Left_Up = false;
					
					Right = false;
					Right_Down = false;
					Right_Up = false;
				}
				public Buttons ( bool up, bool up_down, bool up_up, bool down, bool down_down, bool down_up, bool left, bool left_down, bool left_up, bool right, bool right_down, bool right_up ) {
					
					Up = up;
					Up_Down = up_down;
					Up_Up = up_up;

					Down = down;
					Down_Down = down_down;
					Down_Up = down_up;

					Left = left;
					Left_Down = left_down;
					Left_Up = left_up;

					Right = right;
					Right_Down = right_down;
					Right_Up = right_up;
				}

				public static bool Equal ( Buttons buttons1, Buttons buttons2 ) {

					if (buttons1.Up != buttons2.Up) return false;
					if (buttons1.Up_Down != buttons2.Up_Down) return false;
					if (buttons1.Up_Up != buttons2.Up_Up) return false;

					if (buttons1.Down != buttons2.Down) return false;
					if (buttons1.Down_Down != buttons2.Down_Down) return false;
					if (buttons1.Down_Up != buttons2.Down_Up) return false;

					if (buttons1.Left != buttons2.Left) return false;
					if (buttons1.Left_Down != buttons2.Left_Down) return false;
					if (buttons1.Left_Up != buttons2.Left_Up) return false;

					if (buttons1.Right != buttons2.Up) return false;
					if (buttons1.Right_Down != buttons2.Right_Down) return false;
					if (buttons1.Right_Up != buttons2.Right_Up) return false;

					return true;
				}
			}
			public class Analog {
				
				public float Horizontal { get; }
				public float Vertical { get; }

				public Analog () {

					Horizontal = 0f;
					Vertical = 0f;  
				}
				public Analog ( float horizontal, float vertical ) {
					
					Horizontal = horizontal;
					Vertical = vertical;
				}
			
				public static bool Equal ( Analog analog1, Analog analog2 ) {

					if ( analog1.Horizontal != analog2.Horizontal ) return false;
					if ( analog1.Vertical != analog2.Vertical ) return false;

					return true;
				}
			}

			public Package () {

				Face = new Buttons ();
				Dpad = new Buttons ();
				LeftAnalog = new Analog ();
				RightAnalog = new Analog ();
				BackLeft = new Back ();
				BackRight = new Back ();
			}
			public Package ( Buttons face, Buttons dpad, Analog leftAnalog, Analog rightAnalog, Back backLeft, Back backRight )  {

				Face = face;
				Dpad = dpad;
				LeftAnalog = leftAnalog;
				RightAnalog = rightAnalog;
				BackLeft = backLeft;
				BackRight = backRight;
			}
		}
	}
}