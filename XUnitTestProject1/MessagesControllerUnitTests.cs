using System;
using Xunit;
using QaikuRestCosmos.Controllers;
using Microsoft.Extensions.Configuration;

namespace XUnitTestProject1
{
    public class MessagesControllerUnitTests
    {
        [Fact]
        public async Task Values_Get_All()
        {
            var controller = new MessagesController(configuration);

        }
    }
}
