/*
	This is an implementation of a trinary tree (similar to a binary tree, except nodes can also have a middle child that is equal to the node's value).
	
	Insertion and deletion methods are included, along with some helper functions, and basic testing.

	Amy Schlesener
	amy@amyschlesener.com
*/

using System;
using System.Collections;

namespace TrinaryTreeImplementation
{
	public class TestTrinaryTree 
	{
		// Basic method to test insertion and deletion
		public static void Main() 
		{
			TrinaryTree tree = new TrinaryTree();
			tree.Insert(5);	
			tree.Insert(4);
			tree.Insert(9);
			tree.Insert(5);	
			tree.Insert(7);
			tree.Insert(2);
			tree.Insert(2);

			tree.PrintLevels(tree);
			
			if (tree.Delete(9))
				Console.WriteLine("Successfully deleted node."); 
			else
				Console.WriteLine("Failed to delete node.");
			
			tree.PrintLevels(tree);
		}
	}

	// Node class. In addition to child nodes, the nodes also contain pointers to parent
	// 	nodes as a doubly linked list.
	public class TrinaryNode 
	{
		public int value;
		public TrinaryNode leftChild;
		public TrinaryNode middleChild;
		public TrinaryNode rightChild;
		public TrinaryNode parent;

		public TrinaryNode(int givenValue) 
		{
			this.value = givenValue;
		}
		
		public bool HasChildren() 
		{
			if (this != null && (this.leftChild != null || this.rightChild != null || this.middleChild != null))
				return true;
			else
				return false;
		}		
		
		// Return smallest element of (right) subtree 
		public TrinaryNode FindMinSuccessor(TrinaryNode node) 
		{
			while (node != null) 
			{
				if (node.leftChild != null)
					node = node.leftChild;
				else
					return node;
			}
			return node;
		}

		// Removes a child node from a given node
		public void RemoveChildNode(TrinaryNode parent, char childPosition) 
		{
			if (childPosition == 'l')
				parent.leftChild = null;
			else if (childPosition == 'm')
				parent.middleChild = null;
			else if (childPosition == 'r')
				parent.rightChild = null;
			else 
				return;
		}
		
		// Replaces a given node with its child, updating parent pointers as well
		public TrinaryNode ReplaceNodeWithChild(TrinaryNode current, TrinaryNode child, char childPosition) 
		{
			TrinaryNode oldParent = current.parent;
			current = child;
			current.parent = oldParent;
			if(oldParent != null) 
			{ 
				if (childPosition == 'l') 
					oldParent.leftChild = current;
				else if (childPosition == 'r') 
					oldParent.rightChild = current;
				else if (childPosition == 'm') 
					oldParent.middleChild = current;
			}
			return current;
		}
	}
	
	// Tree class - contains insertion and deletion methods
	public class TrinaryTree 
	{
		private TrinaryNode root;
		
		public TrinaryTree() 
		{
			this.root = null;
		}
	
		public void Insert(int value) 
		{
			Insert(value, this.root);
		}
		
		// Recursive function to add a node. 
		private void Insert(int value, TrinaryNode current) 
		{
			if (current == null)  
			{
				if (this.root == null)
					this.root = new TrinaryNode(value);
				else
					current = new TrinaryNode(value);
			}
			else if (current.value == value)
			{ 
				if (current.middleChild == null) 
				{
					current.middleChild = new TrinaryNode(value);
					current.middleChild.parent = current;
				}
				else
					Insert(value, current.middleChild);
			}
			else if (current.value > value) 
			{
				if (current.leftChild == null) 
				{
					current.leftChild = new TrinaryNode(value);
					current.leftChild.parent = current;
				}
				else
					Insert(value, current.leftChild);
			}
			else {
				if (current.rightChild == null) 
				{
					current.rightChild = new TrinaryNode(value);
					current.rightChild.parent = current;
				}
				else
					Insert(value, current.rightChild);
			}
		}
		
		public bool Delete(int value) 
		{
			return Delete(value, this.root);
		}
		
		// Recursive function to delete node. Keeps track of child position (e.g. middle child).
		private bool Delete(int value, TrinaryNode current, char childPosition = '\0') 
		{
			bool result = false;
			if (current != null) 
			{
				bool isRoot = false;
				
				if (this.root == null)
					return false;
					
				else if (this.root.value == value) 
				{
					isRoot = true;
					
					if (current.HasChildren() == false) 
					{
						this.root = null;
						return true;
					}
				}
				
				// Check recursively to find node
				if (value < current.value) 
					return Delete(value, current.leftChild, 'l');
				else if (value > current.value)
					return Delete(value, current.rightChild, 'r');
					
				// Node matches the value to be deleted
				else 
				{ 
					// Leaf node, simply delete and remove reference from parent
					if(current.HasChildren() == false) 
					{
						current.RemoveChildNode(current.parent, childPosition);
						current = null;
						return true;
					}
					
					// Middle child exists - replace node with middle child
					if (current.middleChild != null)
					{ 	
						if (current.leftChild != null)
							current.middleChild.leftChild = current.leftChild;
						if (current.rightChild != null)
							current.middleChild.rightChild = current.rightChild;
						current = current.ReplaceNodeWithChild(current, current.middleChild, childPosition);
						return true;
					}
					else
					{
						// Only left child exists, replace node with left child
						if (current.leftChild != null && current.rightChild == null) 
						{ 
							current = current.ReplaceNodeWithChild(current, current.leftChild, childPosition);
							return true;
						}
						
						 // Only right child exists, node replace with right child
						else if (current.leftChild == null && current.rightChild != null) 
						{
							current = current.ReplaceNodeWithChild(current, current.rightChild, childPosition);
							return true;
						}
						
						// Both left and right children exist - choose minimum successor as replacement node
						else if (current.leftChild != null && current.rightChild != null) 
						{ 
							TrinaryNode replacement = current.FindMinSuccessor(current.rightChild);
							current.value = replacement.value;
							Delete(replacement.value, current.rightChild, 'r');
							return true;
						}
					}
				
					if (isRoot) 
						this.root = current;
					result = true;
				}
			}
			else {
				result = false;
			}
			return result;
		}
		
		// Prints tree levels to console using breadth-first search
		public void PrintLevels(TrinaryTree tree) 
		{
			TrinaryNode root = tree.root;
			if (root != null) 
			{
				// Put each tree level in a queue and display that queue in a line
				Queue nodes = new Queue();
				nodes.Enqueue(root);
				int nodeCount =  nodes.Count;
				while (nodeCount > 0) 
				{
					TrinaryNode current = (TrinaryNode)nodes.Dequeue();
					nodeCount--;
					Console.Write(current.value + " ");
					
					if (current.leftChild != null)
						nodes.Enqueue(current.leftChild);
					if (current.middleChild != null)
						nodes.Enqueue(current.middleChild);
					if (current.rightChild != null) 
						nodes.Enqueue(current.rightChild);
						
					// End of level, restart queue with next level	
					if(nodeCount == 0) 
					{
						Console.WriteLine("");
						nodeCount = nodes.Count;
					}
				}
			}
		}
	}
}