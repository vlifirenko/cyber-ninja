using System;
using System.Collections.Generic;
using CyberNinja.Models.Enums;
using UnityEngine;

namespace CyberNinja.Models.Army
{
    [Serializable]
    public class PlayerResources
    {
        public List<ResourceItem> items = new List<ResourceItem>();
        
        public void AddItem(EResourceType resourceType, float value)
        {
            items.Add(new ResourceItem
            {
                type = resourceType,
                value = value
            });
        }

        public void UpdateItem(EResourceType resourceType, float delta)
        {
            var isUpdated = false;
            foreach (var item in items)
            {
                if (item.type == resourceType)
                {
                    item.value += delta;
                    isUpdated = true;
                    break;
                }
            }
            if (!isUpdated)
                AddItem(resourceType, delta);
        }

        public float GetItem(EResourceType resourceType)
        {
            foreach (var item in items)
            {
                if (item.type == resourceType)
                {
                    return item.value;
                }
            }

            return 0f;
        }
    }
}