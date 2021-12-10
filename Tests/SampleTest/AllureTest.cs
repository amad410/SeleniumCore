using Framework.Enums;
using Framework.Helpers;
using Framework.Pages.HomePage;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.SampleTest
{
    [TestFixture(BrowserType.Chrome)]
    [AllureNUnit]
    [AllureSuite("SmokeTest")]
    [AllureLink("https://github.com/unickq/allure-nunit")]
    public class AllureTest : BaseTest
    {
        public AllureTest(BrowserType type) : base(type) { }

        public GoogleHomePage _homePage;

        public static IEnumerable<object> SearchData()
        {

            return ExcelHelper.ReadExcel(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\TestData\SampleData.xlsx")), "Sheet1");
        }


        [AllureTag("HomeTest")]
        [AllureStory("This is going to seach for stats royale using Google")]
        [TestCaseSource("SearchData")]
        public void SearchTermInGoogle2(string searchtext)
        {
            PagesContext.GoogleHomePage.PerformSearch(searchtext);

        }
    }
}
