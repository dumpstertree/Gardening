using UnityEngine;

namespace Model.Dialog {

	public class Dialog : ScriptableObject {

		public string Text { 
			get { return _text; }
		}
		public PresentEffect Effect { 
			get { return _effect; }
		}
		public PortraitEmotion Emotion { 
			get { return _emotion; }
		} 
		public SpeakerInfo Speaker { 
			get { return _speaker;} 
		}


		[Header( "Saying" )]
		[TextAreaAttribute]
		[SerializeField] private string _text;	
		[SerializeField] private PresentEffect _effect;
		[SerializeField] private PortraitEmotion _emotion;

		[Header( "Talking" )]
		[SerializeField] private SpeakerInfo _speaker;


		public Dialog( SpeakerInfo speaker, string text, PresentEffect effect = PresentEffect.None, PortraitEmotion emotion = PortraitEmotion.None ) {
			
			_text = text;
			_effect = effect;
			_emotion = emotion;
			_speaker = _speaker;
		}
	}
}