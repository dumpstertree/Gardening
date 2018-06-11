using UnityEngine;

namespace Eden {
	
	public class Input : Dumpster.Core.BuiltInModules.Input.Controller<Input.Package> {

		private KeyCode _confirm = KeyCode.RightShift;
		private KeyCode _back = KeyCode.LeftShift;
		private KeyCode _start = KeyCode.S;

		protected override Package PollPackage () {
			return new Package( this );
		} 	
		protected override Package GetEmptyPackage () {
			return new Package ();
		}

		private Package _lastPackage;

		private void Update () {

			var package = new Package( this );

			if ( _lastPackage == null || ShouldUpdate( package, _lastPackage ) ) {
				
				PushInputPackage( GetEmptyPackage() );
				_lastPackage = package;
			}
		}
		private bool ShouldUpdate ( Package package1, Package package2 ) {

			if ( package1.Confirm != package2.Confirm ) { return true; }
			if ( package1.ConfirmDown != package2.ConfirmDown ) { return true; }
			if ( package1.ConfirmUp != package2.ConfirmUp ) { return true; }
			if ( package1.Back != package2.Back ) { return true; }
			if ( package1.BackDown != package2.BackDown ) { return true; }
			if ( package1.BackUp != package2.BackUp ) { return true; }
			if ( package1.Start != package2.Start ) { return true; }
			if ( package1.StartDown != package2.StartDown ) { return true; }
			if ( package1.StartUp != package2.StartUp ) { return true; }
			if ( package1.Horizontal != package2.Horizontal ) { return true; }
			if ( package1.Vertical != package2.Vertical ) { return true; }

			return false;
		}

		public class Package {
			
			public bool Confirm { get; }
			public bool ConfirmDown { get; }
			public bool ConfirmUp { get; }

			public bool Back { get; }
			public bool BackDown { get; }
			public bool BackUp { get; }

			public bool Start { get; }
			public bool StartDown { get; }
			public bool StartUp { get; }

			public float Horizontal { get; }
			public float Vertical { get; }

			public Package ( Input input ) {

				Confirm = UnityEngine.Input.GetKey( input._confirm );
				ConfirmDown = UnityEngine.Input.GetKeyDown( input._confirm );
				ConfirmUp = UnityEngine.Input.GetKeyUp( input._confirm );

				Back = UnityEngine.Input.GetKey( input._back );
				BackDown = UnityEngine.Input.GetKeyDown( input._back );
				BackUp = UnityEngine.Input.GetKeyUp( input._back );

				Start = UnityEngine.Input.GetKey( input._start );
				StartDown = UnityEngine.Input.GetKeyDown( input._start );
				StartUp = UnityEngine.Input.GetKeyUp( input._start );

				Horizontal = UnityEngine.Input.GetAxis( "Horizontal" );
				Vertical = UnityEngine.Input.GetAxis( "Vertical" );
			}
			public Package () {

				Confirm = false;
				ConfirmDown = false;
				ConfirmUp = false;

				Back = false;
				BackDown = false;
				BackUp = false;

				Start = false;
				StartDown = false;
				StartUp = false;

				Horizontal = 0f;
				Vertical = 0f;
			}
		}
	}
}