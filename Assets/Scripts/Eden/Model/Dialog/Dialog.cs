using UnityEngine;

namespace Eden.Model.Dialog {

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
	}
}