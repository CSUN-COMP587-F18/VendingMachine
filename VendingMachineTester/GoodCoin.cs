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

        /// <summary>
        /// UT03-GC
        /// Checks if vending machine works normally with weird coin denomination inserted
        /// </summary>
        [TestMethod]
        public void GoodWeirdCoin()
        {
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 1, 10, 10, 10);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "Coke" }, new List<int>() { 140 });
            vm.LoadCoins(new int[] { 1, 6, 1, 1 });
            vm.LoadPopCans(new int[] { 1 });
            vm.CoinSlot.AddCoin(new Coin(1));
            vm.CoinSlot.AddCoin(new Coin(139));
            vm.SelectionButtons[0].Press();
            var delivery = VMUtility.ExtractDelivery(vm);
            CheckDelivery(delivery, 140, new string[] { });
            var contents = VMUtility.Unload(vm);
            CheckUnload(contents, 190, 0, new string[] { "Coke" });
        }

        /// <summary>
        /// UT04-GC
        /// Automated testing for exact change
        /// </summary>
        [TestMethod]
        public void GoodAutoExactChange()
        {
            Random rnd = new Random();

            // Configure vending machine to have enough pop and coins for the tests
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 100000, 100000, 100000);
            new VendingMachineLogic(vm);
            vm.Configure(new List<string>() { "Coke", "Water", "Juice" }, new List<int>() { 250, 200, 150 });
            vm.LoadCoins(new int[] { 50000, 50000, 50000, 50000 });
            vm.LoadPopCans(new int[] { 10000, 10000, 10000 });

            int i = 1001;
            while (i > 0)
            {
                // Randomly selecting the coins for credit (enough to purchase)
                int credit = 0;
                while (credit <= 250)
                {
                    int num = rnd.Next(1, 5);
                    if (num == 1)
                    {
                        vm.CoinSlot.AddCoin(new Coin(5));
                        credit += 5;
                    }
                    if (num == 2)
                    {
                        vm.CoinSlot.AddCoin(new Coin(10));
                        credit += 10;
                    }
                    if (num == 3)
                    {
                        vm.CoinSlot.AddCoin(new Coin(25));
                        credit += 25;
                    }
                    if (num == 4)
                    {
                        vm.CoinSlot.AddCoin(new Coin(100));
                        credit += 100;
                    }
                }

                // Random choice
                string choice = "";
                int cost = 0;
                int select = rnd.Next(0, 3);
                if (select == 0)
                {
                    vm.SelectionButtons[0].Press();
                    cost = 250;
                    choice = "Coke";
                }
                if (select == 1)
                {
                    vm.SelectionButtons[1].Press();
                    cost = 200;
                    choice = "Water";
                }
                if (select == 2)
                {
                    vm.SelectionButtons[2].Press();
                    cost = 150;
                    choice = "Juice";
                }

                // Check if test passes
                credit -= cost;
                var delivery = VMUtility.ExtractDelivery(vm);
                CheckDelivery(delivery, credit, new string[] { choice });
                i--;
            }

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
