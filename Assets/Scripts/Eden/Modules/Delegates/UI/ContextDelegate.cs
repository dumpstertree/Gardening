using Dumpster.Core;
using Eden.Modules;
using UnityEngine;
using System.Collections.Generic;
using Dumpster.BuiltInModules;

namespace Eden.UI {
	
	public class ContextDelegate : MonoBehaviour, IContextDelgate {

		Context IContextDelgate.GetContext ( string forContextIdentifier ) {

			if ( forContextIdentifier == Game.GetModule<Constants>().UIContexts.Player ) {
				return GetPlayerContext ();
			}
			if ( forContextIdentifier == Game.GetModule<Constants>().UIContexts.Dialog ) {
				return GetDialogContext ();
			}
			if ( forContextIdentifier == Game.GetModule<Constants>().UIContexts.Inventory ) {
				return GetInventoryContext ();
			}
			if ( forContextIdentifier == Game.GetModule<Constants>().UIContexts.Building ) {
				return GetBuildingContext ();
			}

			return null;
		}
		void IContextDelgate.ReturnContext ( Context context ) {

			context.Destroy();
		}

		
		// ******************* Context *******************

		private Context GetPlayerContext () {

			return new InteractiveContext( 
				
				Game.GetModule<Constants>().UIContexts.Player, 
				Game.GetModule<Constants>().InputLayers.Player, 
				
				new List<InteractivePanel>{
				}, 
				new List<Panel>{ 
					ConditionForCanvas( _targetingPanel ),
					ConditionForCanvas( _interactablePanel ),
					ConditionForCanvas( _hudUIPanel ),
					ConditionForCanvas( _quickslotPanel ) 
				}
			);
		}
		private Context GetDialogContext () {
			
			return new InteractiveContext( 
				
				Game.GetModule<Constants>().UIContexts.Dialog, 
				Game.GetModule<Constants>().InputLayers.ForegroundUI, 
				
				new List<InteractivePanel>{
					ConditionForCanvas( _dialogPanel )
				}, 
				new List<Panel>{ 
				}
			);
		}
		private Context GetInventoryContext () {
			return new InteractiveContext( 
				
				Game.GetModule<Constants>().UIContexts.Inventory, 
				Game.GetModule<Constants>().InputLayers.MidgroundUI, 
				
				new List<InteractivePanel>{
					ConditionForCanvas( _inventoryPanel ),
				}, 
				new List<Panel>{ 
				}
			);
		}
		private Context GetBuildingContext () {
			return new InteractiveContext( 
				
				Game.GetModule<Constants>().UIContexts.Building, 
				Game.GetModule<Constants>().InputLayers.MidgroundUI, 
				
				new List<InteractivePanel>{
					ConditionForCanvas( _buildingPanel ) 
				}, 
				new List<Panel>{ 
				}
			);
		}


		// ******************* Panels *******************

		private Panel _menuButtonsPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "MenuButtonsUIPanel" ) ).GetComponent<Panel>();  }
		}
		private Panel _quickslotPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "QuickslotUIPanel" ) ).GetComponent<Panel>(); }
		}
		private Panel _hudUIPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "HUD" ) ).GetComponent<Panel>(); }
		}
		private Panel _interactablePanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "Interactable" ) ).GetComponent<Panel>(); }
		}
		private Panel _targetingPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "Targeting" ) ).GetComponent<Panel>(); }
		}

		

		// *************** Interactive Panels *******************

		private InteractivePanel _inventoryPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "InventoryUIPanel" ) ).GetComponent<InteractivePanel>(); }
		}
		private InteractivePanel _dialogPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "DialogUIPanel" ) ).GetComponent<InteractivePanel>(); }
		}
		private InteractivePanel _buildingPanel {
			get { return GameObject.Instantiate( Resources.Load<GameObject>( "Building" ) ).GetComponent<InteractivePanel>(); }
		}
		


		// ******************* Private *******************
		
		[SerializeField] private Canvas _canvas;

		private Panel ConditionForCanvas ( Panel panel ) {

			panel.transform.SetParent( _canvas.transform, false );
			return panel;
		}
		private InteractivePanel ConditionForCanvas ( InteractivePanel panel ) {

			panel.transform.SetParent( _canvas.transform, false );
			return panel; 
		}		
	}
}