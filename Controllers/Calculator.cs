using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRViabilityPortal.Controllers
{
    public class Calculator
    {
        public DateTime PaymentDate { get; set; }
        public double RepaymentAmount { get; set; }
        public double RemainingCapital { get; set; }
        public double Profit { get; set; }
        public double AmountToPayFromCapital { get; set; }
        public double AveragePayment { get; set; }
        public int NoOfDays { get; set; }
        public string Response { get; set; }

        public List<Calculator> GetCalculationDetails(Request clone)
        {

            try
            {
                List<Calculator> L = new List<Calculator>();

                if (clone == null)
                    return GetDefaultCalculationDetails("91");
                else
                {
                    if (clone.Type == CalculationType.MONTHLY)
                    {
                        int counter = 0;
                        while (counter <= clone.No_of_Months)
                        {
                            double amountToPayFromCapital = 0;
                            amountToPayFromCapital = Math.Round(((clone.Amount - clone.DownPayment) / clone.No_of_Months), 2, MidpointRounding.AwayFromZero);
                            if (counter == 0)
                            {

                                L.Add(new Calculator()
                                {
                                    PaymentDate = clone.StartDate,
                                    RepaymentAmount = 0,
                                    RemainingCapital = Math.Round((clone.Amount), 2, MidpointRounding.AwayFromZero),
                                    AmountToPayFromCapital = Math.Round((clone.DownPayment), 2, MidpointRounding.AwayFromZero),
                                    AveragePayment = 0,
                                    NoOfDays = 0,
                                    Profit = 0,
                                    Response = "00"
                                });

                                ++counter;
                            }
                            else
                            {
                                Calculator C = L[counter - 1];

                                L.Add(new Calculator()
                                {
                                    PaymentDate = C.PaymentDate.AddMonths(1),
                                    RemainingCapital = Math.Round((C.RemainingCapital - C.AmountToPayFromCapital), 2, MidpointRounding.AwayFromZero),
                                    AmountToPayFromCapital = amountToPayFromCapital,
                                    AveragePayment = 0,
                                    NoOfDays = (C.PaymentDate.AddMonths(1) - C.PaymentDate).Days,
                                    Profit = Math.Round(((((C.RemainingCapital - C.AmountToPayFromCapital) * clone.Rate) / 360) * ((C.PaymentDate.AddMonths(1) - C.PaymentDate).Days)), 2, MidpointRounding.AwayFromZero),
                                    RepaymentAmount = Math.Round(((((C.RemainingCapital - C.AmountToPayFromCapital) * clone.Rate) / 360) * ((C.PaymentDate.AddMonths(1) - C.PaymentDate).Days) + amountToPayFromCapital), 2, MidpointRounding.AwayFromZero),
                                    Response = "00"

                                });

                                ++counter;

                            }


                        }


                        double totalPayment = L.Sum(item => item.RepaymentAmount);
                        double averagePayment = Math.Round((totalPayment / clone.No_of_Months), 2, MidpointRounding.AwayFromZero);

                        L = L.Select(c => { c.AveragePayment = averagePayment; return c; }).ToList();

                    }
                    else if (clone.Type == CalculationType.YEARLY)
                    {
                        int counter = 0;
                        while (counter <= clone.No_of_Months)
                        {
                            double amountToPayFromCapital = 0;
                            amountToPayFromCapital = Math.Round(((clone.Amount - clone.DownPayment) / clone.No_of_Months), 2, MidpointRounding.AwayFromZero);
                            if (counter == 0)
                            {

                                //var err2 = new LogUtility.Error()
                                //{
                                //    ErrorDescription = "test",
                                //    ErrorTime = DateTime.Now,
                                //    ModulePointer = "GetCalculationDetails",
                                //    StackTrace = "test"
                                //};

                                //new LogUtility.ActivityLogger().WriteErrorLog(err2);

                                L.Add(new Calculator()
                                {
                                    PaymentDate = clone.StartDate,
                                    RepaymentAmount = 0,
                                    RemainingCapital = Math.Round((clone.Amount), 2, MidpointRounding.AwayFromZero),
                                    AmountToPayFromCapital = Math.Round((clone.DownPayment), 2, MidpointRounding.AwayFromZero),
                                    AveragePayment = 0,
                                    NoOfDays = 0,
                                    Profit = 0,
                                    Response = "00"
                                });



                                ++counter;

                            }
                            else
                            {
                                Calculator C = L[counter - 1];

                                L.Add(new Calculator()
                                {
                                    PaymentDate = C.PaymentDate.AddYears(1),
                                    RemainingCapital = Math.Round((C.RemainingCapital - C.AmountToPayFromCapital), 2, MidpointRounding.AwayFromZero),
                                    AmountToPayFromCapital = amountToPayFromCapital,
                                    AveragePayment = 0,
                                    NoOfDays = (C.PaymentDate.AddYears(1) - C.PaymentDate).Days,
                                    Profit = Math.Round(((((C.RemainingCapital - C.AmountToPayFromCapital) * clone.Rate) / 360) * ((C.PaymentDate.AddYears(1) - C.PaymentDate).Days)), 2, MidpointRounding.AwayFromZero),
                                    RepaymentAmount = Math.Round(((((C.RemainingCapital - C.AmountToPayFromCapital) * clone.Rate) / 360) * ((C.PaymentDate.AddYears(1) - C.PaymentDate).Days) + amountToPayFromCapital), 2, MidpointRounding.AwayFromZero),
                                    Response = "00"

                                });

                                ++counter;

                            }


                        }


                        double totalPayment = L.Sum(item => item.RepaymentAmount);
                        double averagePayment = Math.Round((totalPayment / clone.No_of_Months), 2, MidpointRounding.AwayFromZero);

                        L = L.Select(c => { c.AveragePayment = averagePayment; return c; }).ToList();

                    }


                    return L;
                }
            }
            catch (Exception ex)
            {

                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "GetCalculationDetails",
                    StackTrace = ex.StackTrace
                };

               // new LogUtility.ActivityLogger().WriteErrorLog(err2);

                return null;
            }

        }

        private List<Calculator> GetDefaultCalculationDetails(string response)
        {

            try
            {
                List<Calculator> L = new List<Calculator>();

                L.Add(new Calculator()
                {
                    PaymentDate = DateTime.Now,
                    RepaymentAmount = 0,
                    AmountToPayFromCapital = 0,
                    AveragePayment = 0,
                    RemainingCapital = 0,
                    Profit = 0,
                    Response = response
                });

                return L;
            }
            catch (Exception ex)
            {

                var err2 = new LogUtility.Error()
                {
                    ErrorDescription = ex.Message + Environment.NewLine + ex.InnerException,
                    ErrorTime = DateTime.Now,
                    ModulePointer = "GetDefaultCalculationDetails",
                    StackTrace = ex.StackTrace
                };

              //  new LogUtility.ActivityLogger().WriteErrorLog(err2);

                return null;
            }

        }
    }

    public enum CalculationType
    {
        MONTHLY = 0,
        YEARLY = 1,
        QUARTERLY = 2
    }

    public class Request
    {
        public static object ServerVariables { get; internal set; }
        public double Amount { get; set; }
        public double DownPayment { get; set; }
        public int No_of_Months { get; set; }
        public double Rate { get; set; }
        public DateTime StartDate { get; set; }
        public CalculationType Type { get; set; }
    }
}