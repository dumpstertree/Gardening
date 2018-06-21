using System.Collections.Generic;

namespace Eden.Controller.Dialog {

	public class Sequence {

		public Model.Dialog.Dialog Next () {
			_index = _index + 1;
			return _dialogs[ _index ];
		}
		public Model.Dialog.Dialog Back () {
			_index = _index - 1;
			return _dialogs[ _index ];
		}
		public bool isDone {
			get{ return ( _index == _dialogs.Count - 1 ); }
		}

		private int _index;
		private List<Model.Dialog.Dialog> _dialogs;

		public Sequence( List<Model.Dialog.Dialog> dialogs ) {

			_dialogs = dialogs;
			_index = -1;
		}
	}
}