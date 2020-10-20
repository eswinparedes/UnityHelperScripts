using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RXUI
{
    public class InventoryItem
    {
        public InventoryItem(string name, int stackCount)
        {
            Name = name;
            StackCount = stackCount;
        }

        public string Name { get; }
        public int StackCount { get; }
    }
}

