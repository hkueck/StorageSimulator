using FluentAssertions;
using StorageSimulator.Core.Model;
using Xunit;

namespace StorageSimulatorTests
{
    public class StorageSimulatorConfigTest
    {
        [Fact]
        public void SetProperties()
        {
            var simulatorConfig = new StorageSimulatorConfig();

            simulatorConfig.CommunicationPath = "Path";

            simulatorConfig.CommunicationPath.Should().Be("Path");
        }
    }
}