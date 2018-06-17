using UnityEngine;
using System.Collections.Generic;

namespace Model.Dialog {

	public class Sequence {

		// ************* Init *****************

		public Sequence ( List<Dialog> dialogs ) {

			_dialogs = dialogs;
			_index = -1;
		}


		// ************* Public *****************
		
		public Dialog Next () {
			_index = _index + 1;
			return _dialogs[ _index ];
		}
		public Dialog Back () {
			_index = _index - 1;
			return _dialogs[ _index ];
		}
		public bool IsLast {
			get{ return ( _index == _dialogs.Count - 2 ); }
		}
		public bool isDone {
			get{ return ( _index == _dialogs.Count - 1 ); }
		}
		

		// ************* Private *****************
		
		private List<Dialog> _dialogs;
		private int _index;
		

		// ************* Data *****************
		
		public struct Dialog {

			public string Text { get; }
			public string Name { get; }
			public Sprite Portrait { get; }
			public PresentEffect Effect { get; }
			public PortraitEmotion Emotion { get; }
			public ColorPalette Color { get; }
			public PortraitAlignment Alignment { get; }

			public Dialog( SpeakerInfo speaker, string text, PresentEffect effect = PresentEffect.None, PortraitEmotion emotion = PortraitEmotion.None ) {
				
				Text = text;
				Name = speaker.Name;
				Portrait = speaker.Portrait;
				Color = speaker.Color;
				Alignment = speaker.Alignment;
				Effect = effect;
				Emotion = emotion;
			}
		}
	}

	public struct SpeakerInfo {

		public string Name { get; }
		public Sprite Portrait { get; }
		public ColorPalette Color { get; }
		public PortraitAlignment Alignment { get; }

		public SpeakerInfo ( string name, Sprite portrait, ColorPalette color, PortraitAlignment alignment ) {

			Name = name;
			Portrait = portrait;
			Color = color;
			Alignment = alignment;
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

