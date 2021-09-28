using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZxDungeon.Logic
{
    /// <summary>
    /// Base element of the graph. Qbit, circle, square, knot etc
    /// </summary>
    public class Element
    {
        /// <summary>
        /// ID of the element
        /// </summary>
        public int id { get; }
        /// <summary>
        /// Type of the element. Green, Red, Square etc.
        /// </summary>
        public ElementType elementType { get; set; }
        /// <summary>
        /// Value or Phase of the element
        /// </summary>
        public float value {get; set;}

        public int lineIndex { get; set; }

        /// <summary>
        /// Constructor of the Element class
        /// </summary>
        /// <param name="id">ID of the element</param>
        /// <param name="elementType">Type of the element. Green, Red, Square etc.</param>
        /// <param name="value">Value or Phase of the element</param>
        public Element(int id, ElementType elementType, float value, int lineIndex)
        {
            this.id = id;
            this.elementType = elementType;
            this.value = value;
            this.lineIndex = lineIndex;
        }
    }
}