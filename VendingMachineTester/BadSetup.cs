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

    }
}
