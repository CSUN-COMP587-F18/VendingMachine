using System;
using System.Linq;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachineTester
{
    [TestClass]
    public class GoodCoin
    {

        /// <summary>
        /// UT01-GC
        /// Calculating approximate change
        /// </summary>
        [TestMethod]
        public void GoodApproximateChange()
        {
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 1, 10, 10, 10);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "Coke" }, new List<int>() { 140 });
            vm.LoadCoins(new int[] { 0, 5, 1, 1, });
            vm.LoadPopCans(new int[] { 1 });
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();
            var delivery = VMUtility.ExtractDelivery(vm);
            CheckDelivery(delivery, 155, new string[] { "Coke" });
            var contents = VMUtility.Unload(vm);
            CheckUnload(contents, 320, 0, new string[] { });
        }
        /// <summary>
        /// UT02-GC
        /// Calculating exact change
        /// </summary>
        [TestMethod]
        public void GoodExactChange()
        {
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 1, 10, 10, 10);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "Coke" }, new List<int>() { 140 });
            vm.LoadCoins(new int[] { 1, 6, 1, 1, });
            vm.LoadPopCans(new int[] { 1 });
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.CoinSlot.AddCoin(new Coin(100));
            vm.SelectionButtons[0].Press();
            var delivery = VMUtility.ExtractDelivery(vm);
            CheckDelivery(delivery, 160, new string[] { "Coke" });
            var contents = VMUtility.Unload(vm);
            CheckUnload(contents, 330, 0, new string[] { });
        }
        
        //Check delivery chute contents
        public void CheckDelivery(List<IDeliverable> delivery, int cents, string[] popNames)
        {
            Assert.AreEqual(cents, delivery.Sum(i => (i is Coin) ? ((Coin)i).Value : 0));
            Assert.IsTrue(popNames.SequenceEqual(delivery.Where(i => i is PopCan).Select(i => ((PopCan)i).Name)));
        }

        //Check remaining contents in vending machine
        public void CheckUnload(VendingMachineStoredContents contents, int centsRemaining, int centsInStorage, string[] popNames)
        {
            Assert.AreEqual(centsRemaining, VMUtility.CentsInCoinRacks(contents));
            Assert.AreEqual(centsInStorage, contents.PaymentCoinsInStorageBin.Sum(c => c.Value));
            Assert.IsTrue(popNames.SequenceEqual(VMUtility.PopsInPopRacks(contents).Select(p => p.Name)));
        }
    }
}
