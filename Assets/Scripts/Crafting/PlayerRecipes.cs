using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecipes : MonoBehaviour {

	public List<Crafting.Recipe> KnowRecipes;
}

/*
namespace Crafting.Recipes {

	public class Axe : Recipe {

		public override string RecipeName {
			get { return "Axe"; }
		}

		override public InventoryItem OutputObject {
			get{ return new Items.Axe(); }
		}

		override public Sprite Sprite {
			get{ return Game.Instance.Sprites.Axe; }
		}

		override public List<Component> Components {
			get{
				var c = new List<Component>();
				//c.Add( new Crafting.Component( typeof( Items.FlowerSeed ) , 5 ) );
				return c;
			}
		}
	}

	public class Shovel : Recipe {

		public override string RecipeName {
			get { return "Shovel"; }
		}

		override public InventoryItem OutputObject {
			get{ return new Items.Shovel(); }
		}

		override public Sprite Sprite {
			get{ return Game.Instance.Sprites.Hand; }
		}

		override public List<Component> Components {
			get{
				var c = new List<Component>();
				//c.Add( new Crafting.Component( typeof( Items.Axe ) , 5 ) );
				//c.Add( new Crafting.Component( typeof( Items.FlowerSeed ) , 5 ) );
				//c.Add( new Crafting.Component( typeof( Items.Axe ) , 8 ) );
				//c.Add( new Crafting.Component( typeof( Items.FlowerSeed ) , 12 ) );
				//c.Add( new Crafting.Component( typeof( Items.Axe ) , 1 ) );
				//c.Add( new Crafting.Component( typeof( Items.FlowerSeed ) , 55 ) );
				//c.Add( new Crafting.Component( typeof( Items.Axe ) , 5 ) );
				//c.Add( new Crafting.Component( typeof( Items.FlowerSeed ) , 99 ) );
				//c.Add( new Crafting.Component( typeof( Items.Axe ) , 2 ) );
				return c;
			}
		}
	}
}
*/