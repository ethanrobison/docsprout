using System.Collections.Generic;

namespace Code.Doods.AI {
	public abstract class Composite : BehaviorTreeNode {
		protected List<BehaviorTreeNode> _children = new List<BehaviorTreeNode> ();
	}
}