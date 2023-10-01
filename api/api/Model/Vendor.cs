namespace api.Model;

public class Vendor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<Trip> Trips { get; set; } = new List<Trip>();
}