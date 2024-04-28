namespace Edge.Shared.DataContracts.Enums
{
    /// <summary>
    /// enum for response data codes
    /// </summary>
    [Flags]
    public enum EDataResponseCode
    {
        Success = 0,
        NoDataFound = 1,
        DatabaseConnectionError = 2,
        InvalidInputParameter = 3,
        RecordSuccessfullyAdded = 4,
        RecordSuccesfullyDeleted = 5,
        RecordSuccesfullyEdited = 6,
        StaleObjectState = 7,
        ReferenceDtoHasNoId = 8,
        Locked = 9,
        InvalidToken = 10,
        CheckoutSessionNotCompleted = 11,
        GenericError = 10000
    }
}
