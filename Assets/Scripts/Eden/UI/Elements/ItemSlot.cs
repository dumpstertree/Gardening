using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Eden.UI.Animators;

namespace Eden.UI.Elements {

	public class ItemSlot : MonoBehaviour,  ISelectHandler {

		public enum State {
			Inavalid,
			Default,
			Selected,
			DragStart
		}


		// ***************** Public **********************


		public Item ItemInSlot {
			get; set;
		}
		public bool HasItemInSlot {
			get{ return ItemInSlot != null; }
		}


		public delegate void SelectEvent ();
		public SelectEvent OnSelect;

		public delegate void ClickEvent ();
		public SelectEvent OnClick;


		public void SetSlotState ( State state ) {

			if ( state == _slotState ) {
				return;
			}
				
			switch ( state ) {
			
				case State.Default : 
					_animator.SetState( ItemSlotAnimator.DefaultTag );
					break;
			
				case State.Selected : 
					_animator.SetState( ItemSlotAnimator.SelectedTag );
					break;
			
				case State.DragStart : 
					_animator.SetState( ItemSlotAnimator.DragStartTag );
					break;
			}

			_slotState = state;
		}


		// ***************** Private **********************


		[Header ( "References" ) ]
		[SerializeField] private ItemSlotAnimator _animator;
		[SerializeField] private Button _button;

		[Header ( "Animations" ) ]
		[SerializeField] private Vector3 _itemScale = Vector3.one;
		[SerializeField] private Vector3 _itemOffset = Vector3.zero;
		[SerializeField] private float _lerpSpeed = 0.5f;

		private State _slotState;


		private void Awake () {
			
			_button.onClick.AddListener( FireOnClickEvent );			
			_animator.SetState( ItemSlotAnimator.DefaultTag );
		}
		private void Update () {

			if ( ItemInSlot != null ) {
				
				var startPos = ItemInSlot.transform.position;
				var targetPos = transform.position + _itemOffset;
				ItemInSlot.transform.position = Vector3.Lerp( startPos, targetPos, _lerpSpeed );

				var startScale = ItemInSlot.transform.localScale;
				var targetScale = _itemScale;
				ItemInSlot.transform.localScale = Vector3.Lerp( startScale, targetScale, _lerpSpeed );
			}
		}


		private void FireOnSelectEvent () {

			if ( OnSelect != null ){
				OnSelect ();
			}
		}
		private void FireOnClickEvent () {

			if ( OnClick != null ){
				OnClick ();
			}
		}


		// **************** ISelectHandler ******************


		void ISelectHandler.OnSelect( BaseEventData eventData ) {
		
			FireOnSelectEvent ();
		}
	}
}