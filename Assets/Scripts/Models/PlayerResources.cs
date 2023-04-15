using System;
using System.Collections.Generic;

namespace CyberNinja.Models
{
    [Serializable]
    public class PlayerResources
    {
        public List<ResourceItem> items = new List<ResourceItem>();
        
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
        Magnesium = 10,
        Silicon = 20,
        Lithium = 30,
        Tungsten = 40,
        Cobalt = 50,
        Tantalum  = 60,
    }
}