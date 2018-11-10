using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dumpster.Core.BuiltInModules.Input {

	public interface IInputReciever<T> {

		void RecieveInput ( T package );
		void EnteredInputFocus ();
		void ExitInputFocus ();
	}
}