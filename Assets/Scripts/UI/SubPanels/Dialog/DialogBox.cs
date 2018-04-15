using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace UI.Subpanels.Dialog {
	
	public class DialogBox : MonoBehaviour {

		// *************** Public ******************

		public void Present ( Model.Dialog.Sequence.Dialog dialog, bool hasNext ) {

			_dialog = dialog;
			
			SetName( dialog.Name );
			SetColor( dialog.Color );
			SetPortrait( dialog.Portrait );
			SetHasNextIndicator( hasNext );

			_presentingAnimation = StartCoroutine( Present( dialog.Text ) );
		}
		public void SkipPresenting () {
			
			StopCoroutine( _presentingAnimation );
			_presentingAnimation = null;

			_text.text = _dialog.Text;
		}
		public bool IsPresenting {
			get{ return !(_presentingAnimation == null); }
		}
		
		// ************** Private *********************

		[Header( "Reference" )]
		[SerializeField] private Text _name;
		[SerializeField] private Text _text;
		[SerializeField] private Image _portrait;
		[SerializeField] private GameObject _hasNextIndicator;

		[Header( "Variables" )]
		[SerializeField] private float TIME_PER_CHAR = 0.01f;


		private Coroutine _presentingAnimation;
		private Model.Dialog.Sequence.Dialog _dialog;


		private void SetName( string name ) {

			_name.text = name;
		}
		private void SetPortrait( Sprite portrait ) {

			_portrait.sprite = portrait;
		}
		private void SetColor( Model.Dialog.ColorPalette color ) {
		}
		private void SetHasNextIndicator( bool hasNext ) {

			_hasNextIndicator.gameObject.SetActive( hasNext );
		}
		private IEnumerator Present( string text ) {
			
			_text.text = "";

			for ( int i = 0; i < text.Length; i++ ) {
			
				for( float t=0; t<TIME_PER_CHAR; t+=Time.deltaTime ) {
					yield return null;
				}

				_text.text +=text[ i ];
			}

			_presentingAnimation = null;
		}
	}
}