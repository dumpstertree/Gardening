using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemBubbleUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	// ********* PUBLIC ******************

	public bool Indestuctable {
		get{ return _indestuctable; }
	}
	public int Index {
		get { return _index; }
		set { _index = value; }
	}
 	public void SetItem ( InventoryItem item ) {
 		_item = item;
		if (item == null){
			SetUnfilledSlot();
		}
		else{
			SetFilledSlot( item );
		}
	}
	public void SetAnimationState( State state ){

		switch( state ){

		case State.Default:
			_animator.SetTrigger( DEFAULT_STATE_TRIGGER );
			break;
		case State.Hover:
			_animator.SetTrigger( HOVER_STATE_TRIGGER );
			break;
		}
	}

	public delegate void OnPointerEvent ();
	public OnPointerEvent PointerEnter;
	public OnPointerEvent PointerExit;
	public OnPointerEvent PointerDown;
	public OnPointerEvent PointerUp;

	public void OnPointerEnter(PointerEventData eventData) {
		_pointerOver = true;
		if (PointerEnter != null){
			PointerEnter();
		}
	}
	public void OnPointerExit(PointerEventData eventData) {
		_pointerOver = false;
		if (PointerExit != null){
			PointerExit();
		}
	}
	public void OnPointerDown(PointerEventData eventData) {
		if (PointerDown != null){
			PointerDown();
		}
	}
	public void OnPointerUp(PointerEventData eventData) {
		if (PointerUp != null){
			PointerUp();
		}
	}


	// ********* PRIVATE ******************

	[SerializeField] private GameObject _filledObject;
	[SerializeField] private Image _sprite;
	[SerializeField] private Text _countText;
	[SerializeField] private bool _indestuctable;

	private const string DEFAULT_STATE_TRIGGER = "Default";
	private const string HOVER_STATE_TRIGGER = "Hover";

	private Animator _animator;
	private int _index = -1;

	private void Awake () {
	
		_animator = GetComponent<Animator>();
	}
	private void SetFilledSlot ( InventoryItem item) {

		_filledObject.SetActive( true );

		_sprite.sprite = item.Sprite;
		_countText.text = item.Count.ToString() + "x";
	}
	private void SetUnfilledSlot () {

		_filledObject.SetActive( false );
	}

	private InventoryItem _item;
	private bool _pointerOver;
	private void Update () {

		if ( _pointerOver && Input.GetKeyDown( KeyCode.E ) ){
			((UI.Panels.GunCrafting)Game.UIController.GunCraftingUIPanel).SetItemToEdit( _item );
			Game.UIController.ChangeContext( UIController.UiContext.Identifier.GunCrafting );
		}
	}

	// ************************************

	public enum State {
		Default,
		Hover
	}
}
