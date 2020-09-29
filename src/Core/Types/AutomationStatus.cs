namespace StorageSimulator.Core.Types
{
    public enum AutomationStatus
    {
        TransportOrder = 1,
        InsertionSucceeded = 2,
        DeletionSucceeded = 3,
        InvalidOrderTargetSourceNotFound = 11,
        WrongPartCount = 12,
        ShippedNotAllItems = 13,
        InsertionFailed = 14,
        CountIsZero = 98,
        OrderAlreadyExists = 99
    }
}