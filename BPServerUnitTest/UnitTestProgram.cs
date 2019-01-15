using System;
using System.Windows.Forms;
using Xunit;

namespace BiopticPowerPathDicomServer
{
    public class UnitTestProgram
    {
        [Fact]
        public void NullAppContext()
        {
            ApplicationContext appcontext = null;
            var ex = Record.Exception(() => new BPServer(appcontext));
            Assert.IsType<NullReferenceException>(ex);
        }
    }
}
