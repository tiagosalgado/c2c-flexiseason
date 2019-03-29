using Newtonsoft.Json;

namespace c2c_flexiseason
{
    public class Credentials
    {
        [JsonProperty("j_username")]
        public string Username { get; set; }
        [JsonProperty("j_password")]
        public string Password { get; set; }
    }
    public class Smartcard
    {
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }
    }

    public class ProductModel
    {
        [JsonProperty("product")]
        public Product Product { get; set; }
        public string pTyp { get; set; }
    }

    public class Product
    {
        public string Destination { get; set; }
        public string DestinationName { get; set; }
        public string Isrn { get; set; }
        public string Origin { get; set; }
        public string OriginName { get; set; }
        public string Id { get; set; }
        public string Status { get; set; }
        public string TicketType { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public string OriginatingTisMachineId { get; set; }
        public string FulfilmentRequestReference { get; set; }
        public string FulfilmentRequestIPEReference { get; set; }
        public string IpeIsamId { get; set; }
        public string IpeIsamSeqNum { get; set; }
        public string Lifecycle { get; set; }
        public string TravelClass { get; set; }
        public string PassengerType { get; set; }
        public string MethodOfPayment { get; set; }
        public string RouteCode { get; set; }
        public string RouteName { get; set; }
        public int Fare { get; set; }
        public string CurrencyCode { get; set; }
        public string ValidityStartDate { get; set; }
        public string ExpiryDateCurrent { get; set; }
        public string ExpiryTime { get; set; }
        public string IpeExpiryDate { get; set; }
        public string SalesTransactionReference { get; set; }
        public string SalesTransactionDateTimeStamp { get; set; }
        public string SalesTicketNumber { get; set; }
        public string AmountPaid { get; set; }
        public string MessageCode { get; set; }
        public string PartySizeAdult { get; set; }
        public string PartySizeChild { get; set; }
        public string PartySizeConcession { get; set; }
        public string TransactionType { get; set; }
        public string DaysTravelPermitted { get; set; }
        public string ValidityCode { get; set; }
        public string ValidityDescription { get; set; }
        public string ValidityEnvironment { get; set; }
        public string ExpiryDateSP { get; set; }
        public string NumberRemainingPasses { get; set; }
    }
}