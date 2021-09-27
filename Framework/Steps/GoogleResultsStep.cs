using Framework.Factory;
using Framework.Pages;
using Framework.Pages.HomePage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Steps
{
    public class GoogleResultsStep
    {
        public Page _page;
        //public GoogleHomePage _homePage;
        protected DriverFactory _driverFactoryInstance;

        public GoogleResultsStep(Page page)
        {
            _page = page;

        }

        public GoogleResultsStep()
        {
            //_driverFactoryInstance = DriverFactory.getInstance();
            //_homePage = new GoogleHomePage(_driverFactoryInstance.getDriver());

        }

        public void PerformSearch(String term)
        {
            _page.GoogleHomePage.PerformSearch(term);
            //_homePage.PerformSearch(term);
        }
    }
}
