using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Genetic_V8;

namespace TripPlannerTests
{
    [TestClass]
    public class ValidatorTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            Params.NumberOfPoints = 5;
            Params.Profits = new double[6] { 0, 1, 2, 3, 4, 5 };
            Params.Distances = new double[6, 6]
            {
                {0, 1, 1, 1, 1, 1 },
                {1, 0, 1, 1, 1, 1 },
                {1, 1, 0, 1, 1, 1 },
                {1, 1, 1, 0, 1, 1 },
                {1, 1, 1, 1, 0, 1 },
                {1, 1, 1, 1, 1, 0 }
            };
            Results.AvailablePoints = new HashSet<int>(new int[] { 1, 2, 3, 4, 5 });
            Params.MaxLength = 5;
            Results.Solutions = new List<Route>();
            Route r1 = new Route();
            r1.Points = new List<int>();
            r1.Points.Add(0);
            r1.Points.Add(1);
            r1.Points.Add(2);
            r1.Points.Add(0);
            Route r2 = new Route();
            r2.Points = new List<int>();
            r2.Points.Add(0);
            r2.Points.Add(1);
            r2.Points.Add(3);
            r2.Points.Add(0);

            Results.Solutions.Add(r1);
            Results.Solutions.Add(r2);

        }
        [TestMethod]
        public void duplicates_validator_detects_duplicates()
        {
            SolutionValidator validator = new SolutionValidator();

            bool duplicatesStatus = validator.SolutionHasDuplicates();

            Assert.AreEqual(true, duplicatesStatus);
        }
        [TestMethod]
        public void length_validator_detects_length_error()
        {
            SolutionValidator validator = new SolutionValidator();
            Results.TotalLength = 5;

            bool lengthStatus = validator.SolutionLengthIsWrong();
            Assert.AreEqual(true, lengthStatus);

            Results.TotalLength = 6;
            lengthStatus = validator.SolutionLengthIsWrong();
            Assert.AreEqual(false, lengthStatus);

        }
        [TestMethod]
        public void profit_validator_detects_profit_error()
        {
            SolutionValidator validator = new SolutionValidator();
            Results.TotalProfit = 7;
            bool profitStatus = validator.SolutionTotalProfitIsWrong();

            Assert.AreEqual(false, profitStatus);
            Results.TotalProfit = 6;
            profitStatus = validator.SolutionTotalProfitIsWrong();
            Assert.AreEqual(true, profitStatus);
        }
    }
}
