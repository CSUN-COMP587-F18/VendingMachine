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

    }
}
