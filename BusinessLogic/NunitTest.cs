using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic
{ 
    // Represent blue print that test method will use.
    public class NormalDistributionTestCase
    {
        public int TotalQty { set; get; }
        public List<InCap> InCaps { set; get; }
        public List<OutCap> Expected { set; get; }
        public string TestName { set; get; }
    }

    // Rsponse for get all test case data from test file line by line 
    // Then performe each line to be in test case form. 
    public class TestCaseProvider
    {
        public static List<NormalDistributionTestCase> TestCases()
        {
            List<NormalDistributionTestCase> testCases = new List<NormalDistributionTestCase>();
            
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + "TestCases.txt";

            var file = File.ReadAllLines(path).TakeWhile(line => !string.IsNullOrWhiteSpace(line)).ToList();

            //Looping on each line in file and build test case parameter.
            foreach (var line in file)
            {
                //Temp parameter used to store data after perform it.
                int totalQty;
                string testCaseName;
                List<InCap> inCaps = new List<InCap>();
                List<OutCap> expected = new List<OutCap>();

                // Remove all white space and square brackets.
                string trim = line.Replace(" ", "").Replace("[", "").Replace("]", "");

                // Get case name and total quantity.
                var nameAndTotalQty = trim.Split('@')[0].Split(':');
                testCaseName = nameAndTotalQty[0];
                totalQty = int.Parse(nameAndTotalQty[1]);

                // Get all inCaps and outCaps as strings.
                var inCapsAndOutCaps = trim.Split('@')[1].Split('=');
                var inCapsArray = inCapsAndOutCaps[0].Replace(">,<", ";").Replace("<", "").Replace(">", "").Split(';');
                var outCapsArray = inCapsAndOutCaps[1].Replace(">,<", ";").Replace("<", "").Replace(">", "").Split(';');

                //Start Build Test Case
                foreach (var inCapitem in inCapsArray)
                {
                    // Perform each inCap and add it to inCaps list
                    inCaps.Add(new InCap()
                    {
                        ID = inCapitem.Split(',')[0],
                        Participation = Decimal.Divide(decimal.Parse(inCapitem.Split(',')[1].Split('/')[0]), decimal.Parse(inCapitem.Split(',')[1].Split('/')[1]))
                    });
                }

                foreach (var outCapitem in outCapsArray)
                {
                    // Perform each outCap and add it to outCaps list
                    expected.Add(new OutCap()
                    {
                        ID = outCapitem.Split(',')[0],
                        Quantity = int.Parse(outCapitem.Split(',')[1])
                    });
                }
                //End Build Test Case.

                //Add test Case to test cases list.
                testCases.Add(
                    new NormalDistributionTestCase()
                    {
                        TotalQty = totalQty,
                        InCaps = inCaps,
                        Expected = expected,
                        TestName = testCaseName
                    }
                );
            }
            return testCases;
        }

        //Static method return test cases one by one after named each one.
        public static IEnumerable TestCasesRules()
        {
            var cases = TestCaseProvider.TestCases();
            foreach (var caseitem in cases)
            {
                yield return new TestCaseData(caseitem).SetName(caseitem.TestName);
            } 
        }
    }

    [TestFixture]
    class NunitTest
    {

       
        [Test, TestCaseSource(typeof(TestCaseProvider), nameof(TestCaseProvider.TestCasesRules))]
        public void NormalDistributionTest(NormalDistributionTestCase testCase)
        {
            /* Get Actual Data */
            var actual = CapOperations.NormalDistribution(testCase.TotalQty, testCase.InCaps);

            /* Assert */
            Assert.AreEqual(testCase.Expected, actual);
        }
    }
}
