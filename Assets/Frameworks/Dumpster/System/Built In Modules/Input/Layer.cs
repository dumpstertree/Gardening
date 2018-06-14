using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Core.BuiltInModules.Input {

	public class Layer<T> {

		private List< IInputReciever<T>> _recievers = new List< IInputReciever<T>>();

		public void AddToLayer (  IInputReciever<T> reciever ) {

			_recievers.Add( reciever );
		}
		public void RecieveInput ( T package ){
			
			foreach( IInputReciever<T> r in _recievers ){
				r.RecieveInput( package );
			}
		}
		public void EnterFocus () {

			foreach( IInputReciever<T> r in _recievers ){
				r.EnteredInputFocus ();
			}
		}
		public void ExitFocus () {

			foreach( IInputReciever<T> r in _recievers ){
				r.ExitInputFocus ();
			}
		}
	}
}