using UnityEngine;

namespace Eden.Model.Dialog {

	public class SpeakerInfo : ScriptableObject {

		public string Name { 
			get{ return _name; } 
		}
		public Sprite Portrait { 
			get { return _portrait; }
		}
		public ColorPalette Color { 
			get { return _color; }
		}
		public PortraitAlignment Alignment { 
			get { return _alignment; }
		}

		[SerializeField] private string _name;
		[SerializeField] private Sprite _portrait;
		[SerializeField] private ColorPalette _color;
		[SerializeField] private PortraitAlignment _alignment;
	}
}