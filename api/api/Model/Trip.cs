namespace api.Model;

//public class Trip
//{
//    //Needed?
//    public int Id { get; set; }
//    public DateTime PickupDatetime { get; set; }
//    public DateTime DropoffDatetime { get; set; }
//    public CabType CabType { get; set; }
//    public Vendor? Vendor { get; set; }
//    public Rate Rate { get; set; }
//    public double PickupLatitude { get; set; }
//    public double PickupLongitude { get; set; }
//    public double DropoffLatitude { get; set; }
//    public double DropoffLongitude { get; set; }
//    public int PassengerCount { get; set; }
//    public double Distance { get; set; }
//    public double Fare { get; set; }
//    public double Extra { get; set; }
//    public double MtaTax { get; set; }
//    public double Tip { get; set; }
//    public double Tolls { get; set; }

//    public double EhailFee { get; set; }

//    //Improvement and Congestion surcharge
//    public double TotalPrice { get; set; }
//    public PaymentType PaymentType { get; set; }
//    public TripType TripType { get; set; }

//    public Location? PickupLocation { get; set; }
//    public Location? DropoffLocation { get; set; }
//}

public enum CabType
{
    Yellow,
    Green
}

public enum Rate
{
    Standard,
    Group,
    Jfk,
    Negotiated,
    Newark,
    NassayWestchester
}

public enum PaymentType
{
    Cash,
    NoCharge,
    Card,
    Voided,
    Dispute,
    Unknown
}

public enum TripType
{
    Na,
    StreetHail,
    Dispatch
}

public class Trip
{
    public string CabType { get; set; }
    public string VendorId { get; set; }
    public DateTime PickupDatetime { get; set; }
    public DateTime DropoffDatetime { get; set; }
    public string RateCodeId { get; set; }
    public double PickupLatitude { get; set; }
    public double PickupLongitude { get; set; }
    public double DropoffLatitude { get; set; }
    public double DropoffLongitude { get; set; }
    public int PassengerCount { get; set; }
    public double TripDistance { get; set; }

    public double FareAmount { get; set; }

    //public double Extra { get; set; }
    //public double MtaTax { get; set; }
    public double TipAmount { get; set; }

    //public double TollsAmount { get; set; }
    //public double EhailFee { get; set; }
    //public double ImprovementSurcharge { get; set; }
    public double CongestionSurcharge { get; set; }
    public double TotalAmount { get; set; }
    public string PaymentType { get; set; }

    public string TripType { get; set; }
    //public int PickupLocationId { get; set; }
    //public int DropoffLocationId { get; set; }
}