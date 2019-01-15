using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BiopticPowerPathDicomServer
{
    public class UnitTestDicomServerConfiguration
    {
        [Fact]
        public void nullConstructor()
        {
            DicomServerConfiguration dsc = new DicomServerConfiguration();
            Assert.NotNull(dsc);
        }

        [Fact]
        public void NotNullRegistryKeyBiopticVisionSCP()
        {
            DicomServerConfiguration dsc = new DicomServerConfiguration();
            Assert.NotNull(dsc.RkBiopticVisionSCP);
        }

        [Fact]
        public void NotNullProperties()
        {
            DicomServerConfiguration dsc = new DicomServerConfiguration();
            Assert.NotNull(dsc.AETitle);
            Assert.True(dsc.AETitle.Length > 3);

            Assert.NotNull(dsc.ExamScheduledTable);
            Assert.True(dsc.ExamScheduledTable.Length > 3);

            // must be a non-admin port
            Assert.True(dsc.Portnumber > 1024);

            System.Net.IPAddress address;
            Assert.True((System.Net.IPAddress.TryParse(dsc.IpAddress, out address)));
        }

    }
}
