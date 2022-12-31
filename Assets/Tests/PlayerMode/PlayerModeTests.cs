using CyberNinja.Views.Unit;
using NUnit.Framework;
using UnityEngine;

namespace Tests.PlayerMode
{
    public class PlayerModeTests
    {
        [Test]
        public void ItemConfigTest()
        {
            var gameObject = new GameObject();
            var itemView = gameObject.AddComponent<ItemView>();
            
            Assert.IsNull(itemView.Config);
            Assert.IsNotNull(gameObject.GetComponent<ItemView>());
        }
    }
}