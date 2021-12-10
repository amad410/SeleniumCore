using Framework.Assemblies;
using Framework.Assemblies;
using Framework.Pages;
using Framework.Pages.HomePage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.Steps
{
    
    public class GoogleSearchStep 
    {
        public Page _page;
        public GoogleHomePage _homePage;
        protected DriverFactory _driverFactoryInstance;
        

        public GoogleSearchStep(Page page)
        {
            _page = page;

        }

        public void PerformSearch(String term)
        {
            _page.GoogleHomePage.PerformSearch(term);
        }


    }
    
}
