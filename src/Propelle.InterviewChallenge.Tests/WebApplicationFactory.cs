using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Propelle.InterviewChallenge.Tests
{
    public class WebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
    }
}
