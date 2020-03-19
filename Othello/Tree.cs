using System.Collections.Generic;
using System.Linq;

/**
 * This data structure can store the entire game tree. Each node on the tree contains a reference
 * to a class that represents the state of the game, and another class that represents the change
 * that brought about this state from the state encapsulated in the node's parent. Its child nodes
 * represent all the potential future states from this specific state as a result of a single step.
 * 
 * Game states can be accessed by querying this data structure with a Path, which encapsulates a
 * sequence of changes (or moves) leading to that state from the current state.
 * 
*/
namespace Tree
{

	/**
	 * Encapsulates a sequence of discrete events or moves
	*/
	class Path<S>
	{
		private S[] path;

		/**
		 * Creates an empty Path. The first element always points
		 * to null, since the head of the tree represents the
		 * current state
		*/
		public Path()
		{
			path = new S[1];
			path[0] = default(S);
		}

		/**
		 * Encapsulates a single move
		 */
		public Path(S initialIndex)
		{
			path = new S[2];
			path[0] = default(S);
			path[1] = initialIndex;
		}

		/**
		 * Allows the "extension" of a path with an additional move
		 */
		public Path(Path<S> existingPath, S addendum)
		{
			path = new S[existingPath.path.Length];

			for (int i = 0; i < existingPath.path.Length; i++)
			{
				path[i] = existingPath.path[i];
			}
			path[path.Length - 1] = addendum;
		}

		public S this[int index]
		{
			get { return path[index]; }
		}

		public int getLength()
		{
			return path.Length;
		}
	}


	class Tree<I, C>
	{
		private class Node<V, U>
		{
			private V index;
			private U content;
			private List<Node<V, U>> children;

			public Node(U content)
			{
				this.content = content;
				this.children = new List<Node<V, U>>();
			}

			public Node(V index, U content)
			{
				this.content = content;
				this.index = index;
				this.children = new List<Node<V, U>>();
			}

			public V getIndex()
			{
				return index;
			}

			public U getContent()
			{
				return content;
			}

			public List<Node<V, U>> getChildren()
			{
				return children.Select(item => item).ToList();
			}

			public void addChild(Node<V, U> nNode)
			{
				children.Add(nNode);
			}
		}

		/**
		 * Represents the game in its current state. Its children represent
		 * future possibilities of the game. Note that this node may or may
		 * not have an index. This index is always ignored because it is
		 * irrelevant. Since the index represents the change that results
		 * in the current state of the game based on the previous state, and
		 * since the current state of the game has no previous state (it is
		 * the head node, so it has no parent), the index is irrelevant.
		 */
		private Node<I, C> head;

		public Tree(C initialHead)
		{
			head = new Node<I, C>(initialHead);
		}

		private Node<I, C> lookup(Path<I> path, int depth)
		{
			//depth is the number of levels to go down
			//currentDepth is the index of the current level

			/*
			 * The Path is juss a sequence of moves that alter one state to produce
			 * a hypothetical state that would be the result if that move or change
			 * were applied. Since the head node is the current state and not a
			 * hypothetical one, the path to access it is just a Path with a single
			 * null element. You won't ever have to worry about this, however,
			 * because that is all handled in the Path constructors.
			 */
			//                     ----------
			//      O <-----------| move=N/A | path[0]
			//     / \             ----------
			//    /   \            ----------
			//   O     O <--------|move=(3,8)| path[1]
			//  /|\    |\          ----------
			// / | \   | \         ----------
			//O  O  O  O  O <-----|move=(5,1)| path[2]
			//		               ----------

			//I could do this recursively, but I didn't feel like it
			Node<I, C> currentNode = head;
			for (int currentDepth = 1; currentDepth < depth; currentDepth++)
			{
				try
				{
					currentNode =
						currentNode
						.getChildren()
						.Where(
							node =>
								node.getIndex().Equals(path[currentDepth])
						)
						.Single()
					;
				}
				catch (System.InvalidOperationException)
				{
					//if this exception is thrown, there are two possibilities:
					//(1) that there is no future scenario based on this path,
					//or (2) that there is more than one child with the same
					//index. The latter scenario should never happen.

					currentNode = null;
					break; //this *should* break out of the loop
				}
			}

			return currentNode;
		}

		private Node<I, C> lookup(Path<I> path)
		{
			return lookup(path, path.getLength()); //lookup full depth
		}

		public C retrieveHead()
		{
			return head.getContent();
		}

		public bool contains(Path<I> path)
		{
			return (lookup(path) != null);
		}

		public bool setPath(Path<I> path)
		{
			Node<I, C> newHead = lookup(path);
			if (newHead != null)
			{
				head = newHead;
				return true;
			}
			else
			{
				return false;
			}
		}

		public bool addPath(Path<I> path, C nContent)
		{
			Node<I, C> parent = lookup(path, path.getLength() - 1);
			if (parent != null)
			{
				parent.addChild(new Node<I, C>(path[path.getLength() - 1], nContent));
				return true;
			}
			else
			{
				return false;
			}
		}

		public C getPath(Path<I> path)
		{
			Node<I, C> node = lookup(path);
			if (node != null)
			{
				return node.getContent();
			}
			else
			{
				return default(C);
			}
		}

		public List<Path<I>> getChildren(Path<I> path)
		{
			Node<I, C> parent = lookup(path);
			if (parent != null)
			{
				return
					parent
					.getChildren()
					.Select(
						node =>
							new Path<I>(path, node.getIndex())
					)
					.ToList()
				;
			}
			else
			{
				return null;
			}
		}
	}
}