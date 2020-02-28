using System.Collections.Generic;
using System.Linq;

namespace VendingMachine
{
    public class Machine
    { 
        public List<Product> SelectedProducts { get; set; }
        public List<double> Payments { get; set; }

        public Machine()
        {
            SelectedProducts = new List<Product>();
            Payments = new List<double>();
        }

        public List<Product> GetProducts()
        {
           return new List<Product>()
            {
                new Product{Id = 1, Name = "Coke", Price = 25},
                new Product{Id = 2, Name = "Pepsi", Price = 35},
                new Product{Id = 3, Name = "Soda", Price = 45},
                new Product{Id = 4, Name = "Chocolate Bar", Price = 20.25},
                new Product{Id = 5, Name = "Chewing Gum", Price = 10.50},
                new Product{Id = 6, Name = "Bottled Water", Price = 15},
            };  
        }

        public void SelectProduct(int productId)
        {
            var product = GetProducts().FirstOrDefault(x => x.Id == productId);

            if (product != null)
            {
               SelectedProducts.Add(product);
            }
             
        }

        public void InsertPayment(double payment)
        {
            if (IsValidPayment(payment))
            {
                Payments.Add(payment);
            }
        }
 
        public bool IsValidPayment(double payment)
        {
            var validDenomination = new List<double> { 100, 50, 20, 10, 5, 1, .50, .25 };

            return validDenomination.Any(x => x.Equals(payment));
        }

        public double CancelRequest()
        {
            var totalPayments = Payments.Sum();

            ClearRequest(); 

            return totalPayments;
        }

        public void ClearRequest()
        {
            SelectedProducts.Clear();
            Payments.Clear(); 
        }

        public double GetAmountDue()
        {
            return SelectedProducts.Sum(x => x.Price); 
        }

        public double GetTotalPayment()
        {
            return Payments.Sum(); 
        } 
        public double GetChange()
        {
            return GetTotalPayment() - GetAmountDue();
        }
         
        public string RunRequest()
        {
            var due = GetAmountDue();
            var payment = GetTotalPayment();

            if (due > payment)
            {
                return "Inserted money is insufficient to the selected product";
            }

            return string.Empty;
        } 
    }

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
    }

}
