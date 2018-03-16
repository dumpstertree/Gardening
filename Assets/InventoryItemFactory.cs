using System.Collections.Generic;
using UnityEngine;

public class InventoryItemFactory {

	[SerializeField] private InventoryItemTemplate _nullTemplate;

	Dictionary<string,InventoryItemTemplate> _templates;

	public InventoryItemFactory () {
		
		_templates = new Dictionary<string,InventoryItemTemplate>();
		
		var templates = Resources.LoadAll<InventoryItemTemplate>( "" );
		foreach ( InventoryItemTemplate t in templates ) {
			_templates.Add( t.ID, t );
		}
	}


	public void GetItem ( string id ) {

		if ( _templates.ContainsKey( id ) ) {
			//return _templates[ id ].GetInstance();
		}

		//return _nullTemplate.GetInstance();
	}
	public void GetItem ( InventoryItemTemplate template ) {

		template.GetInstance();
	}
}
