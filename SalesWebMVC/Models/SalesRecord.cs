using SalesWebMVC.Models.Enums;
using System;

namespace SalesWebMVC.Models
{
    public class SalesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public SaleStatus StatusSale { get; set; }
        public Seller Seller { get; set; }

        public SalesRecord() { }
        public SalesRecord(DateTime date, double amount, SaleStatus statusSale, Seller seller)
        {
            Date = date;
            Amount = amount;
            StatusSale = statusSale;
            Seller = seller;
        }
    }


}
