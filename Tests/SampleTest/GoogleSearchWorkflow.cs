// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using Framework.Enums;
using Framework.Pages;
using Framework.Steps;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Tests
{
    [TestFixture(BrowserType.Chrome)]
    [TestFixture(BrowserType.InternetExplorer)]
    [TestFixture(BrowserType.FireFox)]
    [TestFixture(BrowserType.Edge)]
    public class GoogleSearchWorkflow : BaseTest
    {
        public GoogleSearchWorkflow(BrowserType type) : base(type) { }

        public GoogleSearchStep _searchStep;

        [Test]
        public void TestMethod()
        {
            _searchStep.PerformSearch("eggs");

        }
        [SetUp]
        public void BeforeSearchTest()
        {
            _searchStep = new GoogleSearchStep(PagesContext);

        }
    }
}
