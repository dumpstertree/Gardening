using UnityEngine;

namespace Model.Dialog {

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

		public SpeakerInfo ( string name, Sprite portrait, ColorPalette color, PortraitAlignment alignment ) {

			_name = name;
			_portrait = portrait;
			_color = color;
			_alignment = alignment;
		}

		public static SpeakerInfo Generic () {
			return new SpeakerInfo( "", null, ColorPalette.Gray, PortraitAlignment.None );
		}
		public static SpeakerInfo Player () {
			var s = Resources.Load<Sprite>( "Player" );
			return new SpeakerInfo( "You", s, ColorPalette.Blue, PortraitAlignment.Left );
		}
		public static SpeakerInfo ZeroTwo () {
			var s = Resources.Load<Sprite>( "Other" );
			return new SpeakerInfo( "Zero Two", s, ColorPalette.Pink, PortraitAlignment.Right );
		}
	}
}