using System;
using System.Linq;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachineTester
{

    [TestClass]
    public class GoodPath
    {
        VendingMachine vm;

        // Initialize this test class with a "default" vending machine and settings
        [TestInitialize]
        public void Initialize()
        {
            this.vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 10, 10, 10);
            this.vm.Configure(new List<string>() { "coke", "water", "juice" }, new List<int>() { 250, 250, 205 });
            this.vm.LoadCoins(new int[] { 1, 1, 2, 0 });
            this.vm.LoadPopCans(new int[] { 1, 1, 1 });
            new VendingMachineLogic(this.vm);
        }

        /// <summary>
        /// UT01-GP
        /// Testing vending machine delivery and content values, no change
        /// </summary>
        [TestMethod]
        public void GoodValuesNoChange()
        {
            this.vm.CoinSlot.AddCoin(new Coin(100));
            this.vm.CoinSlot.AddCoin(new Coin(100));
            this.vm.CoinSlot.AddCoin(new Coin(25));
            this.vm.CoinSlot.AddCoin(new Coin(25));
            this.vm.SelectionButtons[0].Press();

            var delivery = VMUtility.ExtractDelivery(this.vm);
            var contents = VMUtility.Unload(this.vm);

            Assert.AreEqual(delivery.Count, 1);
            Assert.AreEqual(delivery.First(), new PopCan("coke"));
            Assert.AreEqual(VMUtility.CentsInCoinRacks(contents), 315);
            Assert.IsTrue(VMUtility.PopsInPopRacks(contents).SequenceEqual(new List<PopCan>() { new PopCan("water"), new PopCan("juice") }));
        }
    }
}
