using UnityEngine;
using System.Collections.Generic;

public class UIController : MonoBehaviour {

	// ************ PUBLIC **********************

	public UiPanel QuickSlotUIPanel {
		get{ return _quickSlotUIPanelInstance; }
	}
	public UiPanel InventorySlotUIPanel {
		get{ return _inventorySlotUIPanelInstance; }
	}
	public UiPanel RecipesUIPanel {
		get{ return _recipesUIPanelInstance; }
	}
	public UiPanel CraftingUIPanel {
		get{ return _craftingUIPanelInstance; }
	}

	public void Init () {

		CreateCanvas();
		CreatePanels();
		CreateContexts();
		InitPanels();
	}
	public void ChangeContext ( UiContext.Identifier contextToLoad ) {

		if ( _contexts.ContainsKey( contextToLoad ) ) {

			if ( _loadedContext != null  ) {
				_loadedContext.Dismiss();
			}

			_loadedContext = _contexts[ contextToLoad ];
			_loadedContext.Present();
		}
		else{ Debug.LogWarning( "Trying to load context that does not exist : " + contextToLoad ); }
	}

	// *********** PRIVATE ********************
	
	private GameObject _canvasPrefab{
		get{ return Resources.Load( "Canvas" ) as GameObject; }
	}
	private UiPanel _quickSlotUIPanelPrefab{
		get{ return (Resources.Load( "QuickSlotUIPanel" ) as GameObject).GetComponent<UiPanel>(); }
	}
	private UiPanel _inventorySlotUIPanelPrefab{
		get{ return (Resources.Load( "InventoryUIPanel" ) as GameObject).GetComponent<UiPanel>(); }
	}
	private UiPanel _recipesUIPanelPrefab{
		get{ return (Resources.Load( "RecipesUIPanel" ) as GameObject).GetComponent<UiPanel>(); }
	}
	private UiPanel _craftingUIPanelPrefab{
		get{ return (Resources.Load( "CraftingUIPanel" ) as GameObject).GetComponent<UiPanel>(); }
	}

	private GameObject _canvasInstance; 
	private UiPanel _quickSlotUIPanelInstance;
	private UiPanel _inventorySlotUIPanelInstance;
	private UiPanel _recipesUIPanelInstance;
	private UiPanel _craftingUIPanelInstance;

	private Dictionary<UiContext.Identifier,UiContext> _contexts;
	private UiContext _loadedContext;

	private void CreateCanvas () {
		
		// create canvas
		_canvasInstance = Instantiate( _canvasPrefab );
		_canvasInstance.transform.SetParent( transform );
	}
	private void CreatePanels () {

		// create panels
		_quickSlotUIPanelInstance 	  = CreatePanel( _quickSlotUIPanelPrefab );
		_inventorySlotUIPanelInstance = CreatePanel( _inventorySlotUIPanelPrefab );
		_recipesUIPanelInstance 	  = CreatePanel( _recipesUIPanelPrefab );
		_craftingUIPanelInstance 	  = CreatePanel( _craftingUIPanelPrefab );
	}
	private void CreateContexts () {

		_contexts = new Dictionary<UiContext.Identifier,UiContext>();
		
		// create farm context
		var farmContext = new UiContext();
		farmContext.RegisterPanel( _quickSlotUIPanelInstance );
		farmContext.Dismiss();

		// create dungeon context
		var dungeonContext = new UiContext();
		dungeonContext.RegisterPanel( _quickSlotUIPanelInstance );
		dungeonContext.Dismiss();

		// create inventory context
		var inventoryContext = new UiContext();
		inventoryContext.RegisterPanel( _inventorySlotUIPanelInstance );
		inventoryContext.Dismiss();

		// create crafting context
		var craftingContext = new UiContext();
		craftingContext.RegisterPanel( _inventorySlotUIPanelInstance );
		craftingContext.RegisterPanel( _craftingUIPanelInstance );
		craftingContext.RegisterPanel( _recipesUIPanelInstance );
		craftingContext.Dismiss();

		// register all created contexts
		RegisterContext( UiContext.Identifier.Farm, farmContext );
		RegisterContext( UiContext.Identifier.Dungeon, dungeonContext );
		RegisterContext( UiContext.Identifier.Inventory, inventoryContext );
		RegisterContext( UiContext.Identifier.Crafting, craftingContext );
	}

	// **********************************

	private void InitPanels () {

		// initialize new panels 
		_quickSlotUIPanelInstance.Init();
		_inventorySlotUIPanelInstance.Init();
		_recipesUIPanelInstance.Init();
		_craftingUIPanelInstance.Init();
	}

	// **********************************

	private UiPanel CreatePanel ( UiPanel prefab ) {
		
		var newPanel = Instantiate( prefab );
		newPanel.transform.SetParent( _canvasInstance.transform, false );

		return newPanel;
	}
	private void RegisterContext ( UiContext.Identifier id, UiContext context ) {

		if ( !_contexts.ContainsKey( id ) ){ 
			_contexts.Add( id, context ); 
		}
		else { 
			Debug.LogWarning( "Context" + id + "is trying to be registered again" ); 
		}
	}

	// **********************************

	public class UiContext {

		private List<UiPanel> _panels = new List<UiPanel>();

		public void RegisterPanel ( UiPanel panel ) {
			
			if ( !_panels.Contains( panel) ) {
				_panels.Add( panel );
			}
		}
		public void Present () {
			
			foreach( UiPanel p in _panels) {
				p.Present();
			}
		}
		public void Dismiss () { 
			
			foreach( UiPanel p in _panels) {
				p.Dismiss();
			}
		}

		public enum Identifier {
			Farm,
			Dungeon,
			Inventory,
			Crafting
		}
	}
}