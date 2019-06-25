using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using quickaccount.Models.dbentities;

namespace quickaccount.Controllers
{
    [Produces("application/json")]
    [Route("api/MainApi")]
    public class MainApiController : Controller
    {

        #region Customer

        [Route("customer_getall")]
        public dynamic customer_getall()
        {
            MyDbContext db = new MyDbContext();
            var data = from d in db.Customer.ToList()
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           Email = d.Email,
                           Address = d.Address,
                           Phone = d.Phone
                       };
            return data.ToList();
        }
        [Route("customer_insert")]
        public int customer_insert(Customer c)
        {
            MyDbContext db = new MyDbContext();
            db.Customer.Add(c);
            db.SaveChanges();
            return c.Id;
        }
        [Route("customer_update")]
        public int customer_update(Customer c)
        {
            MyDbContext db = new MyDbContext();
            db.Entry(c).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return c.Id;
        }
        #endregion customer


        #region Vendor
        [Route("vendor_getall")]
        public dynamic vendor_getall()
        {
            MyDbContext db = new MyDbContext();
            var data = from d in db.Vendor.ToList()
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           Email = d.Email,
                           Address = d.Address,
                           Phone = d.Phone,
                       };

            return data.ToList();
        }
        [Route("vendor_insert")]
        public int vendor_insert(Vendor v)
        {
            MyDbContext db = new MyDbContext();
            db.Vendor.Add(v);
            db.SaveChanges();
            return v.Id;
        }
        [Route("vendor_update")]
        public int vendor_update(Vendor v)
        {
            MyDbContext db = new MyDbContext();
            db.Entry(v).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return v.Id;
        }

        #endregion vendor
        
        


        #region Accounts
        [Route("finance_account_getall")]
        public dynamic finance_account_getall()
        {
            MyDbContext db = new MyDbContext();
            var data = from d in db.FinanceAccount.Include(a => a.FkParent).ToList()
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           FinanceAccountType = d.FinanceAccountType,
                           FkParentId = d.FkParentId,
                           FkParent_Name = d.FkParent == null ? "" : d.FkParent.Name,
                       };
            return data.ToList();
        }
        [Route("finance_accountWithBalance_getall")]
        public dynamic finance_accountWithBalance_getall()
        {
            MyDbContext db = new MyDbContext();
            var data = from d in db.FinanceAccount.Include(a => a.FkParent).Include(a=>a.FinanceTransaction).ToList()
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           FinanceAccountType = d.FinanceAccountType,
                           FkParentId = d.FkParentId,
                           FkParent_Name = d.FkParent == null ? "" : d.FkParent.Name,
                           Total = d.FinanceTransaction.Sum(a => a.Amount)
                       };
            return data.ToList();
        }
        [Route("finance_account_insert")]
        public int finance_account_insert(FinanceAccount a)
        {
            MyDbContext db = new MyDbContext();
            db.FinanceAccount.Add(a);
            db.SaveChanges();
            return a.Id;
        }
        [Route("finance_account_update")]
        public int finance_account_update(FinanceAccount a)
        {
            MyDbContext db = new MyDbContext();
            db.Entry(a).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return a.Id;
        }
        [HttpPost]
        [Route("finance_transaction_getAll")]
        public dynamic finance_transaction_getAll(string FinanceAccountType = "", string FinanceAccount = "", DateTime? FromDate = null, DateTime? ToDate = null)
        {
            MyDbContext db = new MyDbContext();
            List<FinanceTransaction> list = db.FinanceTransaction.Include(a => a.FkFinanceAccount).Include(a => a.FkAspnetusers).ToList();
            if (FinanceAccountType != "")
            {
                list = list.Where(a => a.FkFinanceAccount.FinanceAccountType == FinanceAccountType).ToList();
            }
            if (FinanceAccount != "")
            {
                list = list.Where(a => a.FkFinanceAccount.Name == FinanceAccount).ToList();
            }
            if (FromDate != null)
            {
                DateTime StartTimeOfFromDate = ((DateTime)FromDate).Date;
                list = list.Where(a => a.DateTime >= StartTimeOfFromDate).ToList();
            }
            if (ToDate != null)
            {
                DateTime EndTimeOfToDate = ((DateTime)ToDate).Date.AddDays(1).AddTicks(-1);
                list = list.Where(a => a.DateTime <= EndTimeOfToDate).ToList();
            }
            var data = from d in list
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           Amount = d.Amount,
                           DateTime = ((DateTime)d.DateTime).ToString("dd/MM/yyyy"),
                           GroupId = d.GroupId,
                           UserType = d.UserType,
                           UserId = d.UserId,
                           ChildOf = d.ChildOf,
                           Status = d.Status,
                           FkFinanceAccountId = d.FkFinanceAccountId,
                           FkFinanceAccount_Name = d.FkFinanceAccount == null ? "" : d.FkFinanceAccount.Name,
                           FkAspnetusersId = d.FkAspnetusersId,
                           FkAspnetusers_Name = d.FkAspnetusers == null ? "" : d.FkAspnetusers.UserName,
                           PaymentMethod = d.PaymentMethod == null ? "" : d.PaymentMethod,
                           ReferenceNumber = d.ReferenceNumber == null ? "" : d.ReferenceNumber,
                           Bank = d.Bank == null ? "" : d.Bank,
                           Branch = d.Branch == null ? "" : d.Branch,
                           ChequeDate = d.ChequeDate,
                           OtherDetail = d.OtherDetail == null ? "" : d.OtherDetail,
                           OherDetails2 = d.OherDetails2 == null ? "" : d.OherDetails2
                       };
            return data.Reverse().ToList();
        }
        [Route("finance_transaction_getById")]
        public dynamic finance_transaction_getById(int Id)
        {
            MyDbContext db = new MyDbContext();
            FinanceTransaction d = db.FinanceTransaction.Where(a => a.Id == Id).Include(a => a.FkFinanceAccount).Include(a => a.FkAspnetusers).FirstOrDefault();
            if (d == null)
            {
                return false;
            }
            else
            {
                var data = new
                {
                    Id = d.Id,
                    Name = d.Name,
                    Amount = d.Amount,
                    DateTime = ((DateTime)d.DateTime).ToString("dd/MM/yyyy"),
                    GroupId = d.GroupId,
                    UserType = d.UserType,
                    UserId = d.UserId,
                    ChildOf = d.ChildOf,
                    Status = d.Status,
                    FkFinanceAccountId = d.FkFinanceAccountId,
                    FkFinanceAccount_Name = d.FkFinanceAccount == null ? "" : d.FkFinanceAccount.Name,
                    FkAspnetusersId = d.FkAspnetusersId,
                    FkAspnetusers_Name = d.FkAspnetusers == null ? "" : d.FkAspnetusers.UserName,
                    PaymentMethod = d.PaymentMethod,
                    ReferenceNumber = d.ReferenceNumber,
                    Bank = d.Bank,
                    Branch = d.Branch,
                    ChequeDate = d.ChequeDate,
                    OtherDetail = d.OtherDetail,
                    OherDetails2 = d.OherDetails2,

                };
                return data;
            }
        }
        [Route("expence_insert")]
        public int expence_insert(FinanceTransaction a)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();

            //New Expence Transaction
            FinanceTransaction ftexpence = new FinanceTransaction();
            ftexpence.Amount = a.Amount;
            ftexpence.Name = a.Name;
            ftexpence.FkFinanceAccountId = a.FkFinanceAccountId;
            ftexpence.DateTime = DateTime.Now;
            ftexpence.Status = "Posted";
            ftexpence.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftexpence);
            db.SaveChanges();
            ftexpence.GroupId = ftexpence.Id;
            db.Entry(ftexpence).State = EntityState.Modified;
            db.SaveChanges();


            //New Expence Transaction
            FinanceTransaction ftdeduct = new FinanceTransaction();
            ftdeduct.Amount = -a.Amount;
            ftdeduct.Name = "Payed Expence no " + ftexpence.Id;
            // for adding expence we need a paying account. i am using child of filed for get paying account. i am not using custom model. else i am using same finance_transaction model 
            ftdeduct.FkFinanceAccountId = a.ChildOf;
            ftdeduct.DateTime = DateTime.Now;
            ftdeduct.Status = "Posted";
            ftdeduct.GroupId = ftexpence.Id;
            ftdeduct.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftdeduct);
            db.SaveChanges();
            return ftexpence.Id;
        }
        [Route("assets_insert")]
        public int assets_insert(FinanceTransaction a)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();

            //New Expence Transaction
            FinanceTransaction ftexpence = new FinanceTransaction();
            ftexpence.Amount = a.Amount;
            ftexpence.Name = a.Name;
            ftexpence.FkFinanceAccountId = a.FkFinanceAccountId;
            ftexpence.DateTime = DateTime.Now;
            ftexpence.Status = "Posted";
            ftexpence.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftexpence);
            db.SaveChanges();
            ftexpence.GroupId = ftexpence.Id;
            db.Entry(ftexpence).State = EntityState.Modified;
            db.SaveChanges();


            //New Expence Transaction
            FinanceTransaction ftdeduct = new FinanceTransaction();
            ftdeduct.Amount = -a.Amount;
            ftdeduct.Name = "Payed Asset no " + ftexpence.Id;
            // for adding expence we need a paying account. i am using child of filed for get paying account. i am not using custom model. else i am using same finance_transaction model 
            ftdeduct.FkFinanceAccountId = a.ChildOf;
            ftdeduct.DateTime = DateTime.Now;
            ftdeduct.Status = "Posted";
            ftdeduct.GroupId = ftexpence.Id;
            ftdeduct.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftdeduct);
            db.SaveChanges();
            return ftexpence.Id;
        }
        [Route("income_insert")]
        public int income_insert(FinanceTransaction a)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();

            //New Expence Transaction
            FinanceTransaction ftexpence = new FinanceTransaction();
            ftexpence.Amount = -a.Amount;
            ftexpence.Name = a.Name;
            ftexpence.FkFinanceAccountId = a.FkFinanceAccountId;
            ftexpence.DateTime = DateTime.Now;
            ftexpence.Status = "Posted";
            ftexpence.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftexpence);
            db.SaveChanges();
            ftexpence.GroupId = ftexpence.Id;
            db.Entry(ftexpence).State = EntityState.Modified;
            db.SaveChanges();


            //New Expence Transaction
            FinanceTransaction ftdeduct = new FinanceTransaction();
            ftdeduct.Amount = a.Amount;
            ftdeduct.Name = "Payment against Income  " + ftexpence.Id;
            // for adding expence we need a paying account. i am using child of filed for get paying account. i am not using custom model. else i am using same finance_transaction model 
            ftdeduct.FkFinanceAccountId = a.ChildOf;
            ftdeduct.DateTime = DateTime.Now;
            ftdeduct.Status = "Posted";
            ftdeduct.GroupId = ftexpence.Id;
            ftdeduct.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftdeduct);
            db.SaveChanges();
            return ftexpence.Id;
        }
        [Route("equity_insert")]
        public int equity_insert(FinanceTransaction a)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();

            //New Expence Transaction
            FinanceTransaction ftexpence = new FinanceTransaction();
            ftexpence.Amount = -a.Amount;
            ftexpence.Name = a.Name;
            ftexpence.FkFinanceAccountId = a.FkFinanceAccountId;
            ftexpence.DateTime = DateTime.Now;
            ftexpence.Status = "Posted";
            ftexpence.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftexpence);
            db.SaveChanges();
            ftexpence.GroupId = ftexpence.Id;
            db.Entry(ftexpence).State = EntityState.Modified;
            db.SaveChanges();


            //New Expence Transaction
            FinanceTransaction ftdeduct = new FinanceTransaction();
            ftdeduct.Amount = a.Amount;
            ftdeduct.Name = "Payment against Equity no " + ftexpence.Id;
            // for adding expence we need a paying account. i am using child of filed for get paying account. i am not using custom model. else i am using same finance_transaction model 
            ftdeduct.FkFinanceAccountId = a.ChildOf;
            ftdeduct.DateTime = DateTime.Now;
            ftdeduct.Status = "Posted";
            ftdeduct.GroupId = ftexpence.Id;
            ftdeduct.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftdeduct);
            db.SaveChanges();
            return ftexpence.Id;
        }
        [HttpPost]
        [Route("NewSale")]
        public dynamic NewSale(SaleModel SaleModel)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> accounts = db.FinanceAccount.ToList();

            //New Sale Transaction
            FinanceTransaction ftsale = new FinanceTransaction();
            ftsale.Amount = -SaleModel.TotalBill;
            ftsale.Name = "New Sale";
            ftsale.FkFinanceAccountId = accounts.Where(a => a.Name == "Product Sale").FirstOrDefault().Id;
            if (SaleModel.CustomerId != 0)
            {
                ftsale.UserId = SaleModel.CustomerId;
            }
            ftsale.UserType = "Customer";
            ftsale.DateTime = DateTime.Now;
            ftsale.Status = "Posted";
            ftsale.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftsale);
            db.SaveChanges();
            ftsale.GroupId = ftsale.Id;
            db.Entry(ftsale).State = EntityState.Modified;
            db.SaveChanges();
            

            //New Payment Transaction against sale . if customer is paying some money
            if (SaleModel.TotalPayment > 0)
            {
                FinanceTransaction ftpayment = new FinanceTransaction();
                ftpayment.Amount = SaleModel.TotalPayment;
                ftpayment.Name = "Payment against Sale no " + ftsale.Id;
                ftpayment.FkFinanceAccountId = accounts.Where(a => a.Name == "Cash").FirstOrDefault().Id;
                if (SaleModel.CustomerId != 0)
                {
                    ftpayment.UserId = SaleModel.CustomerId;
                }
                ftpayment.UserType = "Customer";
                ftpayment.DateTime = DateTime.Now;
                ftpayment.Status = "Posted";
                ftpayment.GroupId = ftsale.Id;
                ftpayment.FkAspnetusersId = UserId;
                db.FinanceTransaction.Add(ftpayment);
                db.SaveChanges();
            }


            // New AR Transaction if Ledger is true
            if (SaleModel.Ledger == true)
            {
                FinanceTransaction ftar = new FinanceTransaction();
                ftar.Amount = SaleModel.TotalBill - SaleModel.TotalPayment;
                ftar.Name = "New AR against Sale no " + ftsale.Id; ;
                ftar.FkFinanceAccountId = accounts.Where(a => a.Name == "Account Receivable").FirstOrDefault().Id;
                if (SaleModel.CustomerId != 0)
                {
                    ftar.UserId = SaleModel.CustomerId;
                }
                ftar.UserType = "Customer";
                ftar.DateTime = DateTime.Now;
                ftar.Status = "Posted";
                ftar.FkAspnetusersId = UserId;
                ftar.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftar);
                db.SaveChanges();
            }

            // new cost of goods transaction against sale
            if (SaleModel.DiscountedBill>0) //we are using DiscountedBill property as CGS
            {
                FinanceTransaction ftcgs = new FinanceTransaction();
                ftcgs.Amount = SaleModel.DiscountedBill; //we are using DiscountedBill property as CGS
                ftcgs.Name = "CGS against Sale no " + ftsale.Id; ;
                ftcgs.FkFinanceAccountId = accounts.Where(a => a.Name == "Cost Of Good Sold").FirstOrDefault().Id;
                ftcgs.DateTime = DateTime.Now;
                ftcgs.Status = "Posted";
                ftcgs.FkAspnetusersId = UserId;
                ftcgs.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftcgs);
                db.SaveChanges();


                // new inventory detct transaction against against sale
                FinanceTransaction ftid = new FinanceTransaction();
                ftid.Amount = -SaleModel.DiscountedBill; //we are using DiscountedBill property as CGS
                ftid.Name = "Inventory detuct against Sale no " + ftsale.Id;
                ftid.FkFinanceAccountId = accounts.Where(a => a.Name == "Inventory").FirstOrDefault().Id;
                ftid.DateTime = DateTime.Now;
                ftid.Status = "Posted";
                ftid.FkAspnetusersId = UserId;
                ftid.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftid);
                db.SaveChanges();

            }
            
            return true;
        }
        [HttpPost]
        [Route("NewPurchase")]
        public bool NewPurchase(SaleModel SaleModel)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> accounts = db.FinanceAccount.ToList();

            //New purchase Transaction
            FinanceTransaction ftpurchase = new FinanceTransaction();
            ftpurchase.Amount = SaleModel.TotalBill;
            ftpurchase.Name = "New Purchase";
            ftpurchase.FkFinanceAccountId = accounts.Where(a => a.Name == "Inventory").FirstOrDefault().Id;
            if (SaleModel.CustomerId != 0)
            {
                ftpurchase.UserId = SaleModel.CustomerId;
            }
            ftpurchase.UserType = "Vendor";
            ftpurchase.DateTime = DateTime.Now;
            ftpurchase.Status = "Posted";
            ftpurchase.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftpurchase);
            db.SaveChanges();
            ftpurchase.GroupId = ftpurchase.Id;
            db.Entry(ftpurchase).State = EntityState.Modified;
            db.SaveChanges();
            

            //New Payment Transaction against purchase . if we are paying some money
            if (SaleModel.TotalPayment > 0)
            {
                FinanceTransaction ftpayment = new FinanceTransaction();
                ftpayment.Amount = -(SaleModel.TotalPayment);
                ftpayment.Name = "New Payment against Purchase no " + ftpurchase.Id;
                ftpayment.FkFinanceAccountId = SaleModel.FinanceAccountId;
                if (SaleModel.CustomerId != 0)
                {
                    ftpayment.UserId = SaleModel.CustomerId;
                }
                ftpayment.UserType = "Vendor";
                ftpayment.DateTime = DateTime.Now;
                ftpayment.Status = "Posted";
                ftpayment.GroupId = ftpurchase.Id;
                ftpayment.FkAspnetusersId = UserId;
                db.FinanceTransaction.Add(ftpayment);
                db.SaveChanges();
            }
            // New AP Transaction if TotalRemaining has ammount
            if (SaleModel.Ledger == true)
            {
                FinanceTransaction ftap = new FinanceTransaction();
                ftap.Amount = -(SaleModel.TotalBill - SaleModel.TotalPayment);
                ftap.Name = "New AP against Purchase no " + ftpurchase.Id; ;
                ftap.FkFinanceAccountId = accounts.Where(a => a.Name == "Account Payable").FirstOrDefault().Id;
                if (SaleModel.CustomerId != 0)
                {
                    ftap.UserId = SaleModel.CustomerId;
                }
                ftap.UserId = SaleModel.CustomerId;
                ftap.UserType = "Vendor";
                ftap.DateTime = DateTime.Now;
                ftap.Status = "Posted";
                db.FinanceTransaction.Add(ftap);
                ftap.GroupId = ftpurchase.Id;
                ftap.FkAspnetusersId = UserId;
                db.SaveChanges();
            }
            return true;
        }

        [HttpPost]
        [Route("CombinedEntry")]
        public dynamic CombinedEntry(SaleModel SaleModel)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> accounts = db.FinanceAccount.ToList();

            //New Sale Transaction
            FinanceTransaction ftsale = new FinanceTransaction();
            ftsale.Amount = -SaleModel.TotalBill;
            ftsale.Name = "New Sale";
            ftsale.FkFinanceAccountId = accounts.Where(a => a.Name == "Product Sale").FirstOrDefault().Id;
            ftsale.UserType = "Customer";
            ftsale.DateTime = DateTime.Now;
            ftsale.Status = "Posted";
            ftsale.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftsale);
            db.SaveChanges();
            ftsale.GroupId = ftsale.Id;
            db.Entry(ftsale).State = EntityState.Modified;
            db.SaveChanges();


            //New Payment Transaction against sale . if customer is paying some money
            FinanceTransaction ftpayment = new FinanceTransaction();
            ftpayment.Amount = SaleModel.TotalPayment;
            ftpayment.Name = "Payment against Sale no " + ftsale.Id;
            ftpayment.FkFinanceAccountId = accounts.Where(a => a.Name == "Cash").FirstOrDefault().Id;
            ftpayment.UserType = "Customer";
            ftpayment.DateTime = DateTime.Now;
            ftpayment.Status = "Posted";
            ftpayment.GroupId = ftsale.Id;
            ftpayment.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftpayment);
            db.SaveChanges();
            
            // new cost of goods transaction against sale
            if (SaleModel.DiscountedBill > 0) //we are using DiscountedBill property as CGS
            {
                FinanceTransaction ftcgs = new FinanceTransaction();
                ftcgs.Amount = SaleModel.DiscountedBill; //we are using DiscountedBill property as CGS
                ftcgs.Name = "CGS against Sale no " + ftsale.Id; ;
                ftcgs.FkFinanceAccountId = accounts.Where(a => a.Name == "Cost Of Good Sold").FirstOrDefault().Id;
                ftcgs.DateTime = DateTime.Now;
                ftcgs.Status = "Posted";
                ftcgs.FkAspnetusersId = UserId;
                ftcgs.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftcgs);
                db.SaveChanges();


                // new inventory detct transaction against against sale
                FinanceTransaction ftid = new FinanceTransaction();
                ftid.Amount = -SaleModel.DiscountedBill; //we are using DiscountedBill property as CGS
                ftid.Name = "Inventory detuct against Sale no " + ftsale.Id;
                ftid.FkFinanceAccountId = accounts.Where(a => a.Name == "Inventory").FirstOrDefault().Id;
                ftid.DateTime = DateTime.Now;
                ftid.Status = "Posted";
                ftid.FkAspnetusersId = UserId;
                ftid.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftid);
                db.SaveChanges();
            }

            //New purchase Transaction
            if (SaleModel.TotalPayment>0)
            {
                FinanceTransaction ftpurchase = new FinanceTransaction();
                ftpurchase.Amount = SaleModel.TotalPayment;
                ftpurchase.Name = "New Purchase";
                ftpurchase.FkFinanceAccountId = accounts.Where(a => a.Name == "Inventory").FirstOrDefault().Id;
                ftpurchase.UserType = "Vendor";
                ftpurchase.DateTime = DateTime.Now;
                ftpurchase.Status = "Posted";
                ftpurchase.FkAspnetusersId = UserId;
                db.FinanceTransaction.Add(ftpurchase);
                db.SaveChanges();
                ftpurchase.GroupId = ftpurchase.Id;
                db.Entry(ftpurchase).State = EntityState.Modified;
                db.SaveChanges();

                FinanceTransaction ftpaymentpurchase = new FinanceTransaction();
                ftpaymentpurchase.Amount = -(SaleModel.TotalPayment);
                ftpaymentpurchase.Name = "New Payment against Purchase no " + ftpurchase.Id;
                ftpaymentpurchase.FkFinanceAccountId = SaleModel.FinanceAccountId;
                ftpaymentpurchase.UserType = "Vendor";
                ftpaymentpurchase.DateTime = DateTime.Now;
                ftpaymentpurchase.Status = "Posted";
                ftpaymentpurchase.GroupId = ftpurchase.Id;
                ftpaymentpurchase.FkAspnetusersId = UserId;
                db.FinanceTransaction.Add(ftpaymentpurchase);
                db.SaveChanges();
            }


            return true;
        }
        [Route("SaleReturn")]
        public dynamic SaleReturn(SaleModel SaleModel)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> accounts = db.FinanceAccount.ToList();
            //Return Sale Transaction
            FinanceTransaction ftsale = new FinanceTransaction();
            ftsale.Amount = SaleModel.TotalBill;
            ftsale.Name = "Return Sale # " + SaleModel.TransactionId;
            ftsale.FkFinanceAccountId = accounts.Where(a => a.Name == "Product Sale").FirstOrDefault().Id;
            if (SaleModel.CustomerId != 0)
            {
                ftsale.UserId = SaleModel.CustomerId;
            }
            ftsale.UserType = "Customer";
            ftsale.DateTime = DateTime.Now;
            ftsale.Status = "Posted";
            ftsale.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftsale);
            db.SaveChanges();
            ftsale.GroupId = ftsale.Id;
            db.Entry(ftsale).State = EntityState.Modified;
            db.SaveChanges();

            // New AP Transaction if Ledger is true . else we will do cash detuct transaction
            if (SaleModel.Ledger == true)
            {
                FinanceTransaction ftar = new FinanceTransaction();
                ftar.Amount = -SaleModel.TotalBill;
                ftar.Name = "AP against retun Sale # " + ftsale.Id;
                ftar.FkFinanceAccountId = accounts.Where(a => a.Name == "Account Payable").FirstOrDefault().Id;
                if (SaleModel.CustomerId != 0)
                {
                    ftar.UserId = SaleModel.CustomerId;
                }
                ftar.UserType = "Customer";
                ftar.DateTime = DateTime.Now;
                ftar.Status = "Posted";
                ftar.FkAspnetusersId = UserId;
                ftar.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftar);
                db.SaveChanges();
            }
            else
            {
                // this transaction is for because we are returing ammount to customer
                FinanceTransaction ftpayment = new FinanceTransaction();
                ftpayment.Amount = SaleModel.TotalPayment;
                ftpayment.Name = "Payment against return sale # " + ftsale.Id;
                ftpayment.FkFinanceAccountId = SaleModel.FinanceAccountId;
                if (SaleModel.CustomerId != 0)
                {
                    ftpayment.UserId = SaleModel.CustomerId;
                }
                ftpayment.UserType = "Customer";
                ftpayment.DateTime = DateTime.Now;
                ftpayment.Status = "Posted";
                ftpayment.FkAspnetusersId = UserId;
                ftpayment.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftpayment);
                db.SaveChanges();
            }

            // new cost of goods sold variable 
            float CostOfGoodsSold = 0;
            foreach (SaleItem item in SaleModel.SaleList)
            {

                //ProductTransaction pt = new ProductTransaction();
                //pt.FkProductId = item.id;
                //pt.FkFinanceTransactionId = ftsale.Id;
                //pt.Price = item.price;
                //pt.Quantity = item.quantity;
                //pt.Total = ((item.price - item.discount) * item.quantity);
                //db.ProductTransaction.Add(pt);
                //db.SaveChanges();
                //CostOfGoodsSold += (float)pt.Total;
            }


            // new minus cost of goods transaction against sale return
            FinanceTransaction ftcgs = new FinanceTransaction();
            ftcgs.Amount = -CostOfGoodsSold;
            ftcgs.Name = "CGS against Return Sale # " + ftsale.Id;
            ftcgs.FkFinanceAccountId = accounts.Where(a => a.Name == "Cost Of Good Sold").FirstOrDefault().Id;
            ftcgs.DateTime = DateTime.Now;
            ftcgs.Status = "Posted";
            ftcgs.FkAspnetusersId = UserId;
            ftcgs.GroupId = ftsale.Id;
            db.FinanceTransaction.Add(ftcgs);
            db.SaveChanges();


            // new inventory plus transaction against against sale
            FinanceTransaction ftid = new FinanceTransaction();
            ftid.Amount = CostOfGoodsSold;
            ftid.Name = "Inventory against Return Sale # " + ftsale.Id; ;
            ftid.FkFinanceAccountId = accounts.Where(a => a.Name == "Inventory").FirstOrDefault().Id;
            ftid.DateTime = DateTime.Now;
            ftid.Status = "Posted";
            ftid.FkAspnetusersId = UserId;
            ftid.GroupId = ftsale.Id;
            db.FinanceTransaction.Add(ftid);
            db.SaveChanges();

            //updateInventoryOnSaleReturn(SaleModel.SaleList);
            return ftsale.Id;
        }
        [Route("PurchaseReturn")]
        public dynamic PurchaseReturn(SaleModel SaleModel)
        {
            string UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> accounts = db.FinanceAccount.ToList();
            //Return Sale Transaction
            FinanceTransaction ftsale = new FinanceTransaction();
            ftsale.Amount = -SaleModel.TotalBill;
            ftsale.Name = "Return Purchase # " + SaleModel.TransactionId;
            ftsale.FkFinanceAccountId = accounts.Where(a => a.Name == "Inventory").FirstOrDefault().Id;
            ftsale.UserId = SaleModel.CustomerId;
            ftsale.UserType = "Vendor";
            ftsale.DateTime = DateTime.Now;
            ftsale.Status = "Posted";
            ftsale.FkAspnetusersId = UserId;
            db.FinanceTransaction.Add(ftsale);
            db.SaveChanges();
            ftsale.GroupId = ftsale.Id;
            db.Entry(ftsale).State = EntityState.Modified;
            db.SaveChanges();
            // New AR Transaction if Ledger is true . else we will do cash detuct transaction
            if (SaleModel.Ledger == true)
            {
                FinanceTransaction ftar = new FinanceTransaction();
                ftar.Amount = SaleModel.TotalBill;
                ftar.Name = "AR against return Purchase # " + ftsale.Id;
                ftar.FkFinanceAccountId = accounts.Where(a => a.Name == "Account Receivable").FirstOrDefault().Id;
                ftar.UserId = SaleModel.CustomerId;
                ftar.UserType = "Vendor";
                ftar.DateTime = DateTime.Now;
                ftar.Status = "Posted";
                ftar.FkAspnetusersId = UserId;
                ftar.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftar);
                db.SaveChanges();
            }
            else
            {
                // this transaction is for because we are receiving ammount to vendor
                FinanceTransaction ftpayment = new FinanceTransaction();
                ftpayment.Amount = SaleModel.TotalPayment;
                ftpayment.Name = "Payment against return purchase # " + ftsale.Id;
                ftpayment.FkFinanceAccountId = SaleModel.FinanceAccountId;
                ftpayment.UserId = SaleModel.CustomerId;
                ftpayment.UserType = "Vendor";
                ftpayment.DateTime = DateTime.Now;
                ftpayment.Status = "Posted";
                ftpayment.FkAspnetusersId = UserId;
                ftpayment.GroupId = ftsale.Id;
                db.FinanceTransaction.Add(ftpayment);
                db.SaveChanges();
            }

            foreach (SaleItem item in SaleModel.SaleList)
            {
                //ProductTransaction pt = new ProductTransaction();
                //pt.FkProductId = item.id;
                //pt.FkFinanceTransactionId = ftsale.Id;
                //pt.Price = item.price;
                //pt.Quantity = -item.quantity;
                //pt.Total = ((item.price - item.discount) * -item.quantity);
                //db.ProductTransaction.Add(pt);
                //db.SaveChanges();
            }

            //updateInventoryOnPurchaseReturn(SaleModel.SaleList);
            return ftsale.Id;
        }
        [Route("salepurchase_getAll")]
        public dynamic salepurchase_getAll(int UserId = 0, string Type = "Sale", DateTime? FromDate = null, DateTime? ToDate = null)
        {
            MyDbContext db = new MyDbContext();
            List<FinanceTransaction> list;
            if (Type == "Sale")
            {
                if (UserId != 0)
                {
                    list = db.FinanceTransaction.Where(a => a.UserType == "Customer").Where(a => a.UserId == UserId).Where(a => a.FkFinanceAccount.Name == "Product Sale").Include(a => a.FkFinanceAccount).ToList();

                }
                else
                {
                    list = db.FinanceTransaction.Where(a => a.UserType == "Customer").Where(a => a.FkFinanceAccount.Name == "Product Sale").Include(a => a.FkFinanceAccount).ToList();

                }
            }
            else
            {
                if (UserId != 0)
                {
                    list = db.FinanceTransaction.Where(a => a.UserType == "Vendor").Where(a => a.UserId == UserId).Where(a => a.FkFinanceAccount.Name == "Inventory").Include(a => a.FkFinanceAccount).ToList();

                }
                else
                {
                    list = db.FinanceTransaction.Where(a => a.UserType == "Vendor").Where(a => a.FkFinanceAccount.Name == "Inventory").Include(a => a.FkFinanceAccount).ToList();
                }
            }
            if (FromDate != null)
            {
                DateTime StartTimeOfFromDate = ((DateTime)FromDate).Date;
                list = list.Where(a => a.DateTime >= StartTimeOfFromDate).ToList();
            }
            if (ToDate != null)
            {
                DateTime EndTimeOfToDate = ((DateTime)ToDate).Date.AddDays(1).AddTicks(-1);
                list = list.Where(a => a.DateTime <= EndTimeOfToDate).ToList();
            }
            if (Type == "Sale")
            {
                var data = from d in list
                           select new
                           {
                               Id = d.Id,
                               Name = d.Name,
                               Amount = d.Amount,
                               DateTime = ((DateTime)d.DateTime).ToString("dd/MM/yyyy"),
                               GroupId = d.GroupId,
                               UserType = d.UserType,
                               UserId = d.UserId,
                               ChildOf = d.ChildOf,
                               Status = d.Status,
                               FkFinanceAccountId = d.FkFinanceAccountId,
                               FkFinanceAccount_Name = d.FkFinanceAccount == null ? "" : d.FkFinanceAccount.Name,
                               FkAspnetusersId = d.FkAspnetusersId,
                               FkAspnetusers_Name = d.FkAspnetusers == null ? "" : d.FkAspnetusers.UserName,
                               PaymentMethod = d.PaymentMethod,
                               ReferenceNumber = d.ReferenceNumber,
                               Bank = d.Bank,
                               Branch = d.Branch,
                               ChequeDate = d.ChequeDate,
                               OtherDetail = d.OtherDetail,
                               OherDetails2 = d.OherDetails2,
                               UserName = d.UserId == null ? "" : db.Customer.Find(d.UserId).Name,

                           };
                return data;
            }
            else
            {
                var data = from d in list
                           select new
                           {
                               Id = d.Id,
                               Name = d.Name,
                               Amount = d.Amount,
                               DateTime = ((DateTime)d.DateTime).ToString("dd/MM/yyyy"),
                               GroupId = d.GroupId,
                               UserType = d.UserType,
                               UserId = d.UserId,
                               ChildOf = d.ChildOf,
                               Status = d.Status,
                               FkFinanceAccountId = d.FkFinanceAccountId,
                               FkFinanceAccount_Name = d.FkFinanceAccount == null ? "" : d.FkFinanceAccount.Name,
                               FkAspnetusersId = d.FkAspnetusersId,
                               FkAspnetusers_Name = d.FkAspnetusers == null ? "" : d.FkAspnetusers.UserName,
                               PaymentMethod = d.PaymentMethod,
                               ReferenceNumber = d.ReferenceNumber,
                               Bank = d.Bank,
                               Branch = d.Branch,
                               ChequeDate = d.ChequeDate,
                               OtherDetail = d.OtherDetail,
                               OherDetails2 = d.OherDetails2,
                               UserName = d.UserId == null ? "" : db.Vendor.Find(d.UserId).Name,

                           };
                return data;
            }
        }
        [Route("user_ledger")]
        public dynamic user_ledger(int UserId, string UserType)
        {
            MyDbContext db = new MyDbContext();
            List<FinanceTransaction> list = db.FinanceTransaction.Where(a => a.UserType == UserType).Where(a => a.UserId == UserId).Include(a => a.FkFinanceAccount).ToList();
            if (UserType == "Customer")
            {
                list = list.Where(a => ((a.FkFinanceAccount.Name != "Account Payable") && (a.FkFinanceAccount.Name != "Account Receivable"))).ToList();
            }
            else
            {
                list = list.Where(a => ((a.FkFinanceAccount.Name != "Account Payable") && (a.FkFinanceAccount.Name != "Account Receivable"))).ToList();

            }

            var data = from d in list
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           Amount = d.Amount,
                           DateTime = ((DateTime)d.DateTime).ToString("dd/MM/yyyy"),
                           GroupId = d.GroupId,
                           UserType = d.UserType,
                           UserId = d.UserId,
                           ChildOf = d.ChildOf,
                           Status = d.Status,
                           FkFinanceAccountId = d.FkFinanceAccountId,
                           FkFinanceAccount_Name = d.FkFinanceAccount == null ? "" : d.FkFinanceAccount.Name,
                           FkAspnetusersId = d.FkAspnetusersId,
                           FkAspnetusers_Name = d.FkAspnetusers == null ? "" : d.FkAspnetusers.UserName,
                           PaymentMethod = d.PaymentMethod,
                           ReferenceNumber = d.ReferenceNumber,
                           Bank = d.Bank,
                           Branch = d.Branch,
                           ChequeDate = d.ChequeDate,
                           OtherDetail = d.OtherDetail,
                           OherDetails2 = d.OherDetails2,

                       };
            return data.ToList();
        }
        [Route("addpayment")]
        public bool addpayment(FinanceTransaction a)
        {
            string AspUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> accounts = db.FinanceAccount.ToList();
            float negativeOrPositive = 1;
            if (a.ChildOf == 1)
            {
                negativeOrPositive -= 2;
            }
            if (a.UserType == "Customer")
            {

                //new payment trasaction
                FinanceTransaction ftpayment = new FinanceTransaction();
                ftpayment.Amount = (negativeOrPositive) * a.Amount;
                ftpayment.Name = a.Name;
                ftpayment.FkFinanceAccountId = a.FkFinanceAccountId;
                ftpayment.UserId = a.UserId;
                ftpayment.UserType = "Customer";
                ftpayment.DateTime = DateTime.Now;
                ftpayment.Status = "Posted";
                ftpayment.FkAspnetusersId = AspUserId;
                db.FinanceTransaction.Add(ftpayment);
                db.SaveChanges();
                ftpayment.GroupId = ftpayment.Id;
                db.Entry(ftpayment).State = EntityState.Modified;
                db.SaveChanges();


                FinanceTransaction ftar = new FinanceTransaction();
                ftar.Amount = (negativeOrPositive) * -a.Amount;
                ftar.Name = "AR against Payment # " + ftpayment.Id;
                ftar.FkFinanceAccountId = accounts.Where(b => b.Name == "Account Receivable").FirstOrDefault().Id;
                ftar.UserId = a.UserId;
                ftar.UserType = "Customer";
                ftar.DateTime = DateTime.Now;
                ftar.Status = "Posted";
                ftar.FkAspnetusersId = AspUserId;
                ftar.GroupId = ftpayment.GroupId;
                db.FinanceTransaction.Add(ftar);
                db.SaveChanges();
            }
            // in case of vendor
            else
            {

                //new payment trasaction
                FinanceTransaction ftpayment = new FinanceTransaction();
                ftpayment.Amount = (negativeOrPositive) * -a.Amount;
                ftpayment.Name = a.Name;
                ftpayment.FkFinanceAccountId = a.FkFinanceAccountId;
                ftpayment.UserId = a.UserId;
                ftpayment.UserType = "Vendor";
                ftpayment.DateTime = DateTime.Now;
                ftpayment.Status = "Posted";
                ftpayment.FkAspnetusersId = AspUserId;
                ftpayment.GroupId = ftpayment.GroupId;
                db.FinanceTransaction.Add(ftpayment);
                db.SaveChanges();

                FinanceTransaction ftap = new FinanceTransaction();
                ftap.Amount = (negativeOrPositive) * a.Amount;
                ftap.Name = "AP against Payment # " + ftpayment.Id;
                ftap.FkFinanceAccountId = accounts.Where(b => b.Name == "Account Payable").FirstOrDefault().Id;
                ftap.UserId = a.UserId;
                ftap.UserType = "Vendor";
                ftap.DateTime = DateTime.Now;
                ftap.Status = "Posted";
                ftap.FkAspnetusersId = AspUserId;
                ftap.GroupId = ftpayment.GroupId;
                db.FinanceTransaction.Add(ftap);
                db.SaveChanges();
            }

            return true;
        }

        // this function is not in use, just for example
        [Route("finance_account_getAll_sum_all_transactions")]
        public dynamic finance_account_getAll_sum_all_transactions()
        {
            MyDbContext db = new MyDbContext();
            List<FinanceAccount> list = db.FinanceAccount.Include(a => a.FinanceTransaction).ToList();
            var data = from d in list
                       select new
                       {
                           Id = d.Id,
                           Name = d.Name,
                           AccountType = d.FinanceAccountType,
                           Today = (float)d.FinanceTransaction.Where(a => (a.DateTime >= DateTime.Now.Date && a.DateTime <= DateTime.Now.Date.AddDays(1).AddTicks(-1))).Sum(a => a.Amount),
                           Month = (float)d.FinanceTransaction.Where(a => (a.DateTime >= new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && a.DateTime <= DateTime.Now.Date.AddMonths(1).AddDays(-1))).Sum(a => a.Amount),
                           Total = (float)d.FinanceTransaction.Sum(a => a.Amount)
                       };

            //List<Finance_Account_SumAllTransaction> list1 = new List<Finance_Account_SumAllTransaction>();
            //foreach (FinanceAccount item in list)
            //{
            //    Finance_Account_SumAllTransaction i = new Finance_Account_SumAllTransaction();
            //    i.AccountId = item.Id;
            //    i.AccountName = item.Name;
            //    i.AccountType = item.FinanceAccountType;
            //    i.AccountAmountToday = (float)item.FinanceTransaction.Where(a=>(a.DateTime>=DateTime.Now.Date && a.DateTime <= DateTime.Now.Date.AddDays(1).AddTicks(-1))).Sum(a => a.Amount);
            //    i.AccountAmountMonth = (float)item.FinanceTransaction.Where(a => (a.DateTime >= new DateTime(DateTime.Now.Year,DateTime.Now.Month,1) && a.DateTime <= DateTime.Now.Date.AddMonths(1).AddDays(-1))).Sum(a => a.Amount);
            //    i.AccountAmountTotal = (float)item.FinanceTransaction.Sum(a => a.Amount);
            //    list1.Add(i);
            //}
            return data.ToList();
        }
        [Route("finance_accounts_balance_groupby_Date")]
        public dynamic finance_accounts_balance_groupby_Date()
        {
            MyDbContext db = new MyDbContext();
            List<FinanceTransaction> list;
            list = db.FinanceTransaction.Include(a => a.FkFinanceAccount).ToList();
            // var groups = list.GroupBy(x => new { Month=x.DateTime.Value.Month, Year = x.DateTime.Value.Year });
            var trendData =
             (from d in list
              group d by new
              {
                  Year = d.DateTime.Value.Year,
                  Month = d.DateTime.Value.Month,
                  Day = d.DateTime.Value.Day,
                  FinanceAccount_Name = d.FkFinanceAccount.Name 
              } into g
              select new
              {
                  
                  Year = g.Key.Year,
                  Month = g.Key.Month,
                  Day = g.Key.Day,
                  FinanceAccount_Name = g.Key.FinanceAccount_Name,
                  Total = g.Sum(x => x.Amount)
              }
        ).AsEnumerable()
         .Select(g => new {
             Year = g.Year,
             Month = g.Month,
             Day = g.Day,
             FinanceAccount_Name = g.FinanceAccount_Name,
             Total = g.Total
         });
            return trendData.ToList();

        }
        #endregion Account


        #region Staff

        [Route("staff_getall")]
        public List<AspNetUsers> staff_getall()
        {
            MyDbContext db = new MyDbContext();
            return db.AspNetUsers.ToList();
        }
        [Route("roles_getall")]
        public List<AspNetRoles> roles_getall()
        {
            MyDbContext db = new MyDbContext();
            return db.AspNetRoles.ToList();
        }

        #endregion Staff

    }

    public class SaleModel
    {
        public int CustomerId { get; set; }
        public float TotalPayment { get; set; }
        public float DiscountedBill { get; set; }
        public float TotalBill { get; set; }
        public bool Ledger { get; set; }
        public List<SaleItem> SaleList { get; set; }
        public int FinanceAccountId { get; set; } //to be used only in purchase,sale return, purchase return.   not in sale
        public int TransactionId { get; set; } // to be used in printing, in sale return return

    }
    public class SaleItem
    {
        public int id { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float discount { get; set; }
        public float quantity { get; set; }
    }
}