using System;
using System.Linq;
using System.Collections.Generic;
using Frontend2;
using Frontend2.Hardware;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VendingMachineTester
{
    [TestClass]
    public class BadSetup
    {
        /// <summary>
        /// UT01-BS
        /// Setting a cost of a drink to 0 should fail
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BadCostsList()
        {
            // Creates vending machine object
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 10, 10, 10);

            // Configures vending machine by passing in the list of drink names and a list of drink prices
            vm.Configure(new List<string>() { "Coke", "water", "sprite" }, new List<int>() { 250, 250, 0 });
        }

        /// <summary>
        /// UT02-BS
        /// # of selection buttons/pop can racks NOT EQUAL TO # of drinks configured
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BadNamesList()
        {
            // Creates vending machine with 3 selection buttons/pop can racks
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 10, 10, 10);

            // Configures the vending machine to only have 2 types of drinks and prices
            vm.Configure(new List<string>() { "Coke", "water" }, new List<int>() { 250, 250 });
        }

        /// <summary>
        /// UT03-BS
        /// Pressing a button that does not exist
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void BadButton()
        {
            var vm = new VendingMachine(new int[] { 5, 10, 25, 100 }, 3, 10, 10, 10);
            new VendingMachineLogic(vm);
            vm.SelectionButtons[3].Press();
        }

        /// <summary>
        /// UT04-BS
        /// Creating vending machine that accepts two of the same denomination
        /// {1, 1} needs to be unique
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BadUniqueDenomination()
        {
            var vm = new VendingMachine(new int[] { 1, 1 }, 1, 10, 10, 10);
        }

        /// <summary>
        /// UT05-BS
        /// Creating vending machine that accepts 0 as a coin should fail
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void BadCoinType()
        {
            var vm = new VendingMachine(new int[] { 0 }, 1, 10, 10, 10);
        }


    }
}
