using StorageSimulator.Core.Interfaces;

namespace StorageSimulator.Core.Model
{
    public class StorageSimulatorConfig: IStorageSimulatorConfig
    {
        public string WatchPath { get; set; } = "./WatchPath";
    }
}