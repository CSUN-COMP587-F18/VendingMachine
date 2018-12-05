using System;
using System.Linq;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachineTester
{
    [TestClass]
    public class GoodWeirdOrdering
    {
        /// <summary>
        /// UT01-GO
        /// Checking values with no configuration
        /// </summary
        [TestMethod]
        public void GoodValuesNoConfigure()
        {
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 10, 10, 10);
            new VendingMachineLogic(vm);
            var delivery = VMUtility.ExtractDelivery(vm);
            var unload = VMUtility.Unload(vm);

            Assert.AreEqual(0, delivery.Count);
            Assert.AreEqual(0, VMUtility.CentsInCoinRacks(unload));
            Assert.AreEqual(0, VMUtility.PopsInPopRacks(unload).Count);
            Assert.AreEqual(0, unload.PaymentCoinsInStorageBin.Count);
        }

        /// <summary>
        /// UT02-GO
        /// Mixed coin denominations
        /// </summary>
        [TestMethod]
        public void GoodMixedCoins()
        {
            var vm = new VendingMachine(new int[] { 100, 5, 25, 10 }, 3, 2, 10, 10);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "Coke", "Water", "Juice" }, new List<int>() { 250, 250, 205 });
            vm.LoadCoins(new int[] { 0, 1, 2, 1 });
            vm.LoadPopCans(new int[] { 1, 1, 1 });
            vm.SelectionButtons[0].Press();
            var delivery = VMUtility.ExtractDelivery(vm);
            Assert.AreEqual(0, delivery.Count);

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();
            delivery = VMUtility.ExtractDelivery(vm);

            Assert.IsTrue((VMUtility.Pops(new string[] { "Coke" })).SequenceEqual(VMUtility.PopsInDelivery(delivery)));
            Assert.AreEqual(50, VMUtility.CentsInDelivery(delivery));

            var unload = VMUtility.Unload(vm);
            Assert.IsTrue((VMUtility.Pops(new string[] { "Water", "Juice" })).SequenceEqual(VMUtility.PopsInPopRacks(unload)));
            Assert.AreEqual(215, VMUtility.CentsInCoinRacks(unload));
            Assert.AreEqual(100, unload.PaymentCoinsInStorageBin.Sum(c => c.Value));
        }

        /// <summary>
        /// UT03-GO
        /// Extracting delivery before sale of drink
        /// </summary>
        [TestMethod]
        public void GoodExtractBeforeSale()
        {
            var vm = new VendingMachine(new int[] { 100, 5, 25, 10 }, 3, 10, 10, 10);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "Coke", "Water", "Juice" }, new List<int>() { 250, 250, 205 });
            vm.LoadCoins(new int[] { 0, 1, 2, 1 });
            vm.LoadPopCans(new int[] { 1, 1, 1 });
            vm.SelectionButtons[0].Press();
            var delivery = VMUtility.ExtractDelivery(vm);
            Assert.AreEqual(0, delivery.Count);

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));

            delivery = VMUtility.ExtractDelivery(vm);

            Assert.AreEqual(0, delivery.Count);
            var unload = VMUtility.Unload(vm);
            Assert.IsTrue((VMUtility.Pops(new string[] { "Coke", "Water", "Juice" })).SequenceEqual(VMUtility.PopsInPopRacks(unload)));
            Assert.AreEqual(65, VMUtility.CentsInCoinRacks(unload));
            Assert.AreEqual(0, unload.PaymentCoinsInStorageBin.Sum(c => c.Value));
        }

        /// <summary>
        /// UT04-GO
        /// Changing configuration throughout the tests
        /// </summary>
        [TestMethod]
        public void GoodChangingConfiguration()
        {
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 10, 10, 10);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "A", "B", "C" }, new List<int>() { 5, 10, 25 });
            vm.LoadCoins(new int[] { 1, 1, 2, 0 });
            vm.LoadPopCans(new int[] { 1, 1, 1 });
            vm.Configure(new List<string>() { "Coke", "Water", "Juice" }, new List<int>() { 250, 250, 205 });
            vm.SelectionButtons[0].Press();
            var delivery = VMUtility.ExtractDelivery(vm);
            Assert.AreEqual(0, delivery.Count);

            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();

            // Check values of original configuration

            delivery = VMUtility.ExtractDelivery(vm);
            Assert.AreEqual(50, VMUtility.CentsInDelivery(delivery));
            Assert.IsTrue(VMUtility.Pops(new string[] { "A" }).SequenceEqual(VMUtility.PopsInDelivery(delivery)));

            var unload = VMUtility.Unload(vm);
            Assert.IsTrue((VMUtility.Pops(new string[] { "B", "C" })).SequenceEqual(VMUtility.PopsInPopRacks(unload)));
            Assert.AreEqual(315, VMUtility.CentsInCoinRacks(unload));
            Assert.AreEqual(0, unload.PaymentCoinsInStorageBin.Sum(c => c.Value));

            // Check results of new configuration change

            vm.LoadCoins(new int[] { 1, 1, 2, 0 });
            vm.LoadPopCans(new int[] { 1, 1, 1 });
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();
            delivery = VMUtility.ExtractDelivery(vm);
            Assert.AreEqual(50, VMUtility.CentsInDelivery(delivery));
            Assert.IsTrue(VMUtility.Pops(new string[] { "Coke" }).SequenceEqual(VMUtility.PopsInDelivery(delivery)));

            unload = VMUtility.Unload(vm);
            Assert.IsTrue((VMUtility.Pops(new string[] { "Water", "Juice" })).SequenceEqual(VMUtility.PopsInPopRacks(unload)));
            Assert.AreEqual(315, VMUtility.CentsInCoinRacks(unload));
            Assert.AreEqual(0, unload.PaymentCoinsInStorageBin.Sum(c => c.Value));

        }

    }
}
