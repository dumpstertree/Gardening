using UnityEngine;
using UnityEngine.UI;
	
namespace Eden.UI.Elements.Building {
	
	public class PartCell : MonoBehaviour {

		public Button Button {
			get{ return _button; }
		}

		public bool IsSelected {
			get { return _selected; }
			set { if ( _selected != value ) {
					if ( value ) {
						SetSelected();
					} else {
						SetUnselected();
					}
				} 
			}
		}
		
		[SerializeField] private Button _button;
		[SerializeField] private Text _name;
		[SerializeField] private Image _image;

		public void SetPart ( Eden.Model.Building.Parts.Gun part ) {
			_name.text = "";
		}
		public void Remove () {
			Destroy( gameObject );
		}

		private bool _selected;

		private void Awake () {

			SetUnselected();
		}
		private void SetSelected () {

			_selected = true;
			_image.color = new Color( 1f, 1f, 1f, 1.0f );
			transform.localScale = Vector3.one;

		}
		private void SetUnselected () {

			_selected = false;
			_image.color = new Color( 1f, 1f, 1f, 0.5f );
			transform.localScale = new Vector3( 0.8f, 0.8f, 0.8f );
		}
	}
}