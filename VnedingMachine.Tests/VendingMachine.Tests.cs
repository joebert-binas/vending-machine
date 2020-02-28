using NUnit.Framework;
using Shouldly;
using System.Linq;
using VendingMachine;

namespace Tests
{
    public class VendingMachineTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(10)]
        [TestCase(5)]
        [TestCase(1)]
        [TestCase(.50)]
        [TestCase(.25)]
        public void VendingMachine_InsertPayment_WhenCoinIsValid(double coin)
        {
            var machine = new Machine();
            machine.InsertPayment(coin);

            var result = machine.Payments.Any();

            result.ShouldBeTrue();
        }

        [Test]
        [TestCase(100)]
        [TestCase(50)]
        [TestCase(20)] 
        public void VendingMachine_InsertPayment_WhenBillIsValid(double bill)
        {
            var machine = new Machine();
            machine.InsertPayment(bill);

            var result = machine.Payments.Any();

            result.ShouldBeTrue();
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void VendingMachine_SelectProduct_WhenProductIdIsValid(int productId)
        {
            var machine = new Machine();
            machine.SelectProduct(productId);

            var result = machine.SelectedProducts.Any();

            result.ShouldBeTrue(); 
        }

        [Test]
        [TestCase(-1, 0)]
        [TestCase(0, 0)]
        public void VendingMachine_Should_Ignore_InvalidSelectedProduct(int productId, int expected)
        {
            var machine = new Machine();
            machine.SelectProduct(productId);

            var result = machine.SelectedProducts.Count();

            expected.ShouldBe(result);
        }


        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void VendingMachine_Should_Return_GreaterThanZeroSelectedProduct(int productId)
        {
            var machine = new Machine();
            machine.SelectProduct(productId);

            var price = machine.SelectedProducts.First().Price;

            price.ShouldBeGreaterThan(0);
        }

        [Test]
        [TestCase(1, 100,100 )]
        [TestCase(2, 50, 50)]
        public void VendingMachine_Should_Return_Refund_WhenCancellingRequest(int productId, double payment, double expected)
        {
            var machine = new Machine();
            machine.SelectProduct(productId);
            machine.InsertPayment(payment);
             
            var result = machine.CancelRequest();

            expected.ShouldBe(result);
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        public void VendingMachine_Should_Return_SelectedProduct(int productId, int expected)
        {
            var machine = new Machine();
            machine.SelectProduct(productId); 

            var result = machine.SelectedProducts.First();

            expected.ShouldBe(result.Id);
        }

        [Test]
        [TestCase(1, new double [] {50 }, 25)]
        [TestCase(2, new double[] { 100 }, 65)]
        [TestCase(3, new double[] { 50 }, 5)]
        [TestCase(4, new double[] { 10,10, 5 }, 4.75)]
        [TestCase(4, new double[] {20, 1 }, 0.75)]
        [TestCase(5, new double[] { 5, 5, .50 }, 0)]
        [TestCase(6, new double[] {20 }, 5)]
        public void VendingMachine_Should_Return_RemainingChange(int productId, double[] payments, double expected)
        {
            var machine = new Machine();
            machine.SelectProduct(productId);

            foreach (var payment in payments)
            {
                machine.InsertPayment(payment);
            } 

            machine.RunRequest();

            var change = machine.GetChange();

            expected.ShouldBe(change);
        }


        [Test] 
        [TestCase(1, new double[] { 20 },  "Inserted money is insufficient to the selected product")]
        public void VendingMachine_Should_Return_Message_IfInsertedMoneyIsInsufficient(int productId, double[] payments, string expected)
        {
            var machine = new Machine();
            machine.SelectProduct(productId);

            foreach (var payment in payments)
            {
                machine.InsertPayment(payment);
            }

            var result = machine.RunRequest();
             
            expected.ShouldBe(result);

        } 
    }
}