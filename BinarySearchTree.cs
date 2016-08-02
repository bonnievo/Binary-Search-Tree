/* Bonnie Vo
 * BIT143 FALL2013
 * A4.0
 */
using System;
using System.Collections.Generic;

namespace BSTSolution
{
    public class CBinarySearchTree
    {
        public enum ErrorCode
        {
            Success = 0,
            AleadyInTree = 1
        }

        private class CTreeNode
        {
            private CTreeNode m_LeftChild, m_RightChild;
            private char m_Data;

            public CTreeNode(char data) { m_Data = data; }

            public CTreeNode LeftChild
            {
                get { return m_LeftChild; }
                set { m_LeftChild = value; }
            }

            public CTreeNode RightChild
            {
                get { return m_RightChild; }
                set { m_RightChild = value; }
            }
            public char Data
            {
                get { return m_Data; }
            }
        }

        private CTreeNode m_Root;
        private int m_cNodes;
        private char[] array;

        public CBinarySearchTree()
        {
        }

        /// <summary>
        /// Add the given value to the tree. Does not add a duplicate.
        /// Add value to tree in proper place using recursion: left if value is smaller; right if value is larger.
        /// </summary>
        /// <returns> 
        /// ErrorCode.Success if value is added.
        /// ErrorCode.AlreadyInTree if value is a duplicate.
        /// </returns>
        public ErrorCode Add(char c)
        {
            if (m_Root == null) // Tree is empty
            {
                m_Root = new CTreeNode(c);
                m_cNodes++;
                return ErrorCode.Success;
            }
            return AddR(m_Root, new CTreeNode(c));
        }
        private ErrorCode AddR(CTreeNode cur, CTreeNode nodeToAdd)
        {
            if (nodeToAdd.Data < cur.Data) // going left
            {
                if (cur.LeftChild == null) // add value here
                {
                    cur.LeftChild = nodeToAdd;
                    m_cNodes++;
                    return ErrorCode.Success;
                }
                else
                {
                    return AddR(cur.LeftChild, nodeToAdd);
                }
            }
            else if (nodeToAdd.Data > cur.Data) // going right
            {
                if (cur.RightChild == null) // add value here
                {
                    cur.RightChild = nodeToAdd;
                    m_cNodes++;
                    return ErrorCode.Success;
                }
                else
                {
                    return AddR(cur.RightChild, nodeToAdd);
                }
            }
            else // character is equal
            {
                return ErrorCode.AleadyInTree;
            }
        }

        /// <summary>
        /// Finds the character in the tree using recursion.
        /// </summary>
        /// <returns> TRUE if character is presented in the tree
        ///           FALSE if character is not presented in the tree
        /// </returns>
        public bool Find(char c)
        {
            if (m_Root == null) // Tree is empty
                return false;
            return FindR(m_Root, c);
        }
        private bool FindR(CTreeNode cur, char target)
        {
            if (target < cur.Data) // go left
            {
                if (cur.LeftChild == null) // End of tree. Value not found.
                    return false;
                else
                    return FindR(cur.LeftChild, target);
            }
            else if (target > cur.Data) // go right
            {
                if (cur.RightChild == null) // End of tree. Value not found.
                    return false;
                else
                    return FindR(cur.RightChild, target);
            }
            else // target == data
            {
                return true;
            }
        }

        /// <summary>
        /// Finds the largest number of levels in the tree recursively.
        /// </summary>
        /// <returns> 
        /// Maximum height of the tree
        /// </returns>
        public int FindDepth()
        {
            return FindDepth(m_Root);
        }
        private int FindDepth(CTreeNode cur)
        {
            if (cur == null)
                return 0;
            else
            {
                return 1 + Math.Max(FindDepth(cur.LeftChild), FindDepth(cur.RightChild));
            }
        }

        /// <summary>
        /// Clone the existing set of BSTNodes into a new BST object using recursion.
        /// Running time O(N).
        /// </summary>
        /// <returns>
        /// New BST Object with all new set of BSTNodes
        /// </returns>
        public CBinarySearchTree Clone()
        {
            CBinarySearchTree copy = new CBinarySearchTree();

            if (m_Root == null) return copy;

            copy.m_cNodes = m_cNodes;
            copy.array = array;
            copy.m_Root = Clone(m_Root);
            return copy;
        }
        private CTreeNode Clone(CTreeNode toCopy)
        {
            if (toCopy == null)
                return null;
            CTreeNode newCopy = new CTreeNode(toCopy.Data);
            if (toCopy.LeftChild != null) // Copy Left Child
            {
                newCopy.LeftChild = Clone(toCopy.LeftChild);
            }
            if (toCopy.RightChild != null) // Copy Right Child
            {
                newCopy.RightChild = Clone(toCopy.RightChild);
            }
            return newCopy;
        }

        /// <summary>
        /// Does an inorder traversal to retrieve values that are stored in tree to an array.
        /// </summary>
        /// <returns>
        /// Sorted array containing all characters in tree from lowest/smallest value to largest/highest value
        /// </returns>
        public char[] GetAllInOrder()
        {
            array = new char[m_cNodes];
            GetAllInOrder(m_Root, 0);
            return array;
        }
        private int GetAllInOrder(CTreeNode cur, int index)
        {
            if (cur != null)
            {
                index = GetAllInOrder(cur.LeftChild, index);
                array[index] = cur.Data;
                return GetAllInOrder(cur.RightChild, index + 1);
            }
            return index;

        }

        /// <summary>
        /// Print all the values in the tree using pre-order traversal.
        /// </summary>   
        public void PrintStructure()
        {
            if (m_Root == null) // Empty Tree
            {
                Console.WriteLine("<There is no data in the tree>");
                return; 
            }
            PrintStructure(m_Root);
        }
        private void PrintStructure(CTreeNode cur)
        {
            if (cur != null)
            {
                Console.Write("Current data: {0} ", cur.Data);
                if (cur.LeftChild != null)
                    Console.Write("Left: {0} ", cur.LeftChild.Data);
                else
                    Console.Write("Left: NULL ");
                if (cur.RightChild != null)
                    Console.WriteLine("Right: " + cur.RightChild.Data);
                else
                    Console.WriteLine("Right: NULL");
                PrintStructure(cur.LeftChild);
                PrintStructure(cur.RightChild);
            }
        }

        /// <summary>
        /// Balance the binary search tree using recursion and a sorted array.
        /// </summary>
        public void Balance()
        {
            if (m_Root == null) 
                return;
            GetAllInOrder(); // Input values from tree to sorted array
            CTreeNode node = new CTreeNode(array[m_cNodes / 2]); // new m_Root node
            node.LeftChild = Balance(0, (m_cNodes / 2));
            node.RightChild = Balance((m_cNodes / 2) + 1, m_cNodes);
            m_Root = node;
        }
        private CTreeNode Balance(int startIndex, int endIndex) // Balance helper returns CTreeNode 
        {
            if (startIndex < 0 || startIndex >= endIndex)
                return null;

            int midIndex = (startIndex + endIndex) / 2;
            CTreeNode node = new CTreeNode(array[midIndex]); // CTreeNode to return
            node.LeftChild = Balance(startIndex, midIndex); // CTreeNode LeftChild
            node.RightChild = Balance(midIndex + 1, endIndex); // CTreeNode RightChild
            return node; // Return CTreeNode with Left/Right Child as character or null
        }

    }
}