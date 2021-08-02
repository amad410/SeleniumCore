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
        public GoogleHomePage _homePage;
        protected DriverFactory _driverFactoryInstance;

        public GoogleResultsStep()
        {
            _driverFactoryInstance = DriverFactory.getInstance();
            _homePage = new GoogleHomePage(_driverFactoryInstance.getDriver());

        }

        public void PerformSearch(String term)
        {
            _homePage.PerformSearch(term);
        }
    }
}
