using System;
using System.Collections.Generic;

namespace CyberNinja.Models
{
    [Serializable]
    public class PlayerResources
    {
        public List<ResourceItem> items = new();
        
        public void Add(EResourceType resourceType, float value)
        {
            items.Add(new ResourceItem
            {
                type = resourceType,
                value = value
            });
        }

        public void Update(EResourceType resourceType, float delta)
        {
            foreach (var item in items)
            {
                if (item.type == resourceType)
                {
                    item.value += delta;
                    break;
                }
            }
        }

        public float Get(EResourceType resourceType)
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

    [Serializable]
    public class ResourceItem
    {
        public EResourceType type;
        public float value;
    }

    public enum EResourceType
    {
        None = 0,
        Resource1 = 10,
        Resource2 = 20,
        Resource3 = 30
    }
}