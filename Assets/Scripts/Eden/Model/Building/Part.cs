namespace Eden.Model.Building {
	
	abstract public class Part<T> {

		public char[,] Blocks { get; protected set; }
		public T Stats { get; protected set; }

		protected Part( char[,] blocks, T stats  ) {

			// Projectors 	⇡ ⇢ ⇣ ⇠
			// Recievers  	∪ ⊂ ∩ ⊃

			Blocks = blocks;
			Stats = stats; 
		}
	}
}