using System;
using System.Windows.Forms;
using Xunit;

namespace BiopticPowerPathDicomServer
{
    public class UnitTestBPServer
    {
        [Fact]
        public void BPServer_RunServer()
        {
            BPServer bp = new BPServer(new ApplicationContext());
            var ex = Record.Exception(() => bp.RunServer());
            Assert.Null(ex);
        }
    }
}
