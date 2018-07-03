using UnityEngine;
using UnityEngine.UI;

public class ButtonActions : MonoBehaviour {

	public string FaceUpActionText {
		get{ return _faceUpAction.text; }
		set{ SetActionText( _faceUpAction, value ); }
	}
	public string FaceDownActionText {
		get{ return _faceDownAction.text; }
		set{ SetActionText( _faceDownAction, value ); }
	}
	public string FaceLeftActionText {
		get{ return _faceLeftAction.text; }
		set{ SetActionText( _faceLeftAction, value ); }
	}
	public string FaceRightActionText {
		get{ return _faceRightAction.text; }
		set{ SetActionText( _faceRightAction, value ); }
	}

	[SerializeField] private Text _faceUpAction;
	[SerializeField] private Text _faceDownAction;
	[SerializeField] private Text _faceLeftAction;
	[SerializeField] private Text _faceRightAction;

	private void SetActionText ( Text text, string newText ) {
		
		text.transform.parent.gameObject.SetActive( newText != "" );
		text.text = newText;
	}
	private void Awake () {

		FaceUpActionText = "";
		FaceDownActionText = "";
		FaceLeftActionText = "";
		FaceRightActionText = "";
	}
}
