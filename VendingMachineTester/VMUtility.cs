using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Frontend2;
using Frontend2.Hardware;

namespace VendingMachineTester
{
    public class VMUtility
    {
        // Remove and return delivery chute contents
        public static List<IDeliverable> ExtractDelivery(VendingMachine vm)
        {
            var items = vm.DeliveryChute.RemoveItems();
            var itemsAsList = new List<IDeliverable>(items);

            return itemsAsList;
        }

        // Remove and return stored contents in vending machine
        public static VendingMachineStoredContents Unload(VendingMachine vm)
        {
            var storedContents = new VendingMachineStoredContents();

            foreach (var coinRack in vm.CoinRacks)
            {
                storedContents.CoinsInCoinRacks.Add(coinRack.Unload());
            }
            storedContents.PaymentCoinsInStorageBin.AddRange(vm.StorageBin.Unload());
            foreach (var popCanRack in vm.PopCanRacks)
            {
                storedContents.PopCansInPopCanRacks.Add(popCanRack.Unload());
            }

            return storedContents;
        }

        public static int CentsInCoinRacks(VendingMachineStoredContents contents)
        {
            var total = 0;
            var listOfListOfCoins = contents.CoinsInCoinRacks;
            foreach (var listOfCoins in listOfListOfCoins)
            {
                foreach (var coin in listOfCoins)
                {
                    total += coin.Value;
                }
            }
            return total;
        }

        public static List<PopCan> PopsInPopRacks(VendingMachineStoredContents contents)
        {
            var pops = new List<PopCan>();
            foreach (var rack in contents.PopCansInPopCanRacks)
            {
                foreach (var pop in rack)
                {
                    pops.Add(pop);
                }
            }
            return pops;
        }

        public static int CentsInDelivery(ICollection<IDeliverable> delivery)
        {
            int cents = 0;
            foreach (var coin in delivery.Where(i => i is Coin))
            {
                cents += ((Coin)coin).Value;
            }
            return cents;
        }

        public static List<PopCan> PopsInDelivery(ICollection<IDeliverable> delivery)
        {
            var pops = new List<PopCan>();
            foreach (var pop in delivery.Where(i => i is PopCan))
            {
                pops.Add((PopCan)pop);
            }
            return pops;
        }

        public static List<PopCan> Pops(string[] popNames)
        {
            var list = new List<PopCan>();
            foreach (var pop in popNames)
            {
                list.Add(new PopCan(pop));
            }
            return list;
        }
    }
}
