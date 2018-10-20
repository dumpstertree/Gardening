using UnityEngine;
using UnityEngine.UI;

namespace Eden.UI.Animators {
	
	public class ItemSlotAnimator : UIAnimator {

		public const string DefaultTag = "Default";
		public const string SelectedTag = "Selected";
		public const string DragStartTag = "DragStart";

		protected override void OnEnterState ( string state ) {
			
			switch ( state ) {
			
				case DefaultTag : OnEnterDefault(); break;
				case SelectedTag : OnEnterSelected(); break;
				case DragStartTag : OnEnterDragStart(); break;
			}
		}
		protected override void OnExitState ( string state ) {

			switch ( state ) {
			
				case DefaultTag : OnExitDefault(); break;
				case SelectedTag : OnExitSelected(); break;
				case DragStartTag : OnExitDragStart(); break;
			}
		}


		[Header( "References" )]
		[SerializeField] private Image _background;
		[SerializeField] private Image _outline;

		
		[Header( "Default Properties" )]
		[SerializeField] private Color _defaultBackgroundColor;
		[SerializeField] private Color _defaultOutlineColor;


		[Header( "Selected Properties" )]
		[SerializeField] private Color _selectedBackgroundColor;
		[SerializeField] private Color _selectedOutlineColor;


		[Header( "Drag Start Properties" )]
		[SerializeField] private Color _dragStartBackgroundColor;
		[SerializeField] private Color _dragStartOutlineColor;



		private void OnEnterDefault () {

			_background.color = _defaultBackgroundColor;
			_outline.color = _defaultOutlineColor;
		}
		private void OnExitDefault () {}


		private void OnEnterSelected () {

			_background.color = _selectedBackgroundColor;
			_outline.color = _selectedOutlineColor;
		}
		private void OnExitSelected () {}


		private void OnEnterDragStart () {

			_background.color = _dragStartBackgroundColor;
			_outline.color = _dragStartOutlineColor;
		}
		private void OnExitDragStart () {}
	}
}