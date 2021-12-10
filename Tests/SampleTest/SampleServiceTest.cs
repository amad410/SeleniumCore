using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.SampleTest
{
    [TestFixture]
    public class SampleServiceTest : BaseTest
    {
        [Test]
        public void SampleGetUsers()
        {
            //Sample test to get list of users
            ServiceContext.SampleUsersClient.GetListOfUser();
        }

        [Test]
        public void SampleCreateUser()
        {
            //Sample test to create a user
            ServiceContext.SampleUsersClient.CreateUser();
        }
    }
}
