using UnityEngine;
using System.Collections.Generic;

namespace Eden.Model.Dialog {

	public class Sequence : ScriptableObject {

		[SerializeField] private List<Eden.Model.Dialog.Dialog> _dialogs;

		public Eden.Controller.Dialog.Sequence GetController () {
			return new Eden.Controller.Dialog.Sequence( _dialogs );
		}
	}

	
	public enum PresentEffect {
		None,
		Emphasis
	}
	public enum ColorPalette {
		Gray,
		Blue,
		Pink,
	}
	public enum PortraitAlignment {
		None,
		Left,
		Right
	}
	public enum PortraitEmotion {
		None,
		Embarrassed,
		Shocked,
		Sad
	}
}
