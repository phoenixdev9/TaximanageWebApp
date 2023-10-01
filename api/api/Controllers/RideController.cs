using api.Model;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Text;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideController : ControllerBase
    {
        private readonly string connectionString;

        private readonly ILogger<RideController> _logger;

        public RideController(ILogger<RideController> logger)
        {
            _logger = logger;
            string username = "admin";
            string password = "kdQzvoNiOY7uBFcI";
            string database = "qdb";
            int port = 32334;
            connectionString =
                $@"host=stuck-bronze-530-6bfe86e2.psql.b1t9.questdb.com;port={port};username={username};password={password};
                database={database};ServerCompatibilityMode=NoTypeLoading;";
        }

        [HttpGet("GetTrips")]
        public async Task<ActionResult<IEnumerable<Trip>>> GetTrips()
        {
            var trips = new List<Trip>();

            await using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();

                var sql = "SELECT * FROM trips;";

                await using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    await using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var trip = new Trip
                            {
                                CabType = reader.GetString(0),
                                VendorId = reader.GetString(1),
                                PickupDatetime = reader.GetDateTime(2),
                                DropoffDatetime = reader.GetDateTime(3),
                                RateCodeId = reader.GetString(4),
                                // Check for null value before attempting to read
                                PickupLatitude = reader.IsDBNull(5) ? 0.0 : reader.GetDouble(5),
                                PickupLongitude = reader.IsDBNull(6) ? 0.0 : reader.GetDouble(6),
                                DropoffLatitude = reader.IsDBNull(7) ? 0.0 : reader.GetDouble(7),
                                DropoffLongitude = reader.IsDBNull(8) ? 0.0 : reader.GetDouble(8),
                                PassengerCount = reader.GetInt32(9),
                                TripDistance = reader.GetDouble(10),
                                FareAmount = reader.GetDouble(11),
                                //Extra = reader.GetDouble(12),
                                //MtaTax = reader.GetDouble(13),
                                TipAmount = reader.GetDouble(14),
                                //TollsAmount = reader.GetDouble(15),
                                //EhailFee = reader.GetDouble(16),
                                //ImprovementSurcharge = reader.GetDouble(17),
                                CongestionSurcharge = reader.GetDouble(18),
                                TotalAmount = reader.GetDouble(19),
                                PaymentType = reader.GetString(20),
                                TripType = reader.GetString(21),
                                //PickupLocationId = reader.GetInt32(22),
                                //DropoffLocationId = reader.GetInt32(23),
                            };

                            trips.Add(trip);
                        }
                    }
                }
            }

            // Convert the list of Trip objects to JSON and return it.
            return Ok(trips);
        }
        //Returns list of dictionary instead of list of Trip objects
        //[HttpGet("Filter")]
        //public async Task<IActionResult> GetTrips(
        //    DateTime? pickupDateTime = null,
        //    DateTime? pickupDateTo = null,
        //    string vendorId = null,
        //    int? passengerCountFrom = null,
        //    string cabType = null,
        //    double? tripDistanceFrom = null,
        //    double? congestionSurcharge = null,
        //    string paymentType = null,
        //    double? totalAmountFrom = null,
        //    string tripType = null,
        //    int? pageNumber = null, // Default to the first page
        //    int? pageSize = null // Default page size
        //)
        //{
        //    try
        //    {
        //        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        //        {
        //            await connection.OpenAsync();

        //            // Calculate the offset based on pageNumber and pageSize
        //            int? offset = null;
        //            if (pageNumber != null && pageSize != null)
        //                offset = (pageNumber - 1) * pageSize;

        //            string sql = "SELECT * FROM trips WHERE 1=1"; // Initial query with a true condition

        //            if (pickupDateTime != null)
        //            {
        //                sql += " AND pickup_datetime >= @pickupDateTime";
        //            }

        //            if (pickupDateTo != null)
        //            {
        //                sql += " AND pickup_datetime <= @pickupDateTo";
        //            }

        //            if (!string.IsNullOrEmpty(vendorId))
        //            {
        //                sql += " AND vendor_id = @vendorId";
        //            }

        //            if (passengerCountFrom != null)
        //            {
        //                sql += " AND passenger_count = @passengerCountFrom";
        //            }

        //            if (!string.IsNullOrEmpty(cabType))
        //            {
        //                sql += " AND cab_type = @cabType";
        //            }

        //            if (tripDistanceFrom != null)
        //            {
        //                sql += " AND trip_distance = @tripDistanceFrom";
        //            }

        //            if (totalAmountFrom != null)
        //            {
        //                sql += " AND total_amount = @totalAmountFrom";
        //            }

        //            if (congestionSurcharge != null)
        //            {
        //                sql += " AND congestion_surcharge = @congestionSurcharge";
        //            }

        //            if (!string.IsNullOrEmpty(paymentType))
        //            {
        //                sql += " AND payment_type = @paymentType";
        //            }

        //            if (!string.IsNullOrEmpty(tripType))
        //            {
        //                sql += " AND trip_type = @tripType";
        //            }

        //            // Add pagination: LIMIT clauses
        //            if (offset != null)
        //                sql += $" LIMIT {offset} , {offset + pageSize}";


        //            await using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
        //            {
        //                if (pickupDateTime != null)
        //                {
        //                    command.Parameters.AddWithValue("@pickupDateTime", pickupDateTime);
        //                }

        //                if (pickupDateTo != null)
        //                {
        //                    command.Parameters.AddWithValue("@pickupDateTo", pickupDateTo);
        //                }

        //                if (!string.IsNullOrEmpty(vendorId))
        //                {
        //                    command.Parameters.AddWithValue("@vendorId", vendorId);
        //                }

        //                if (passengerCountFrom != null)
        //                {
        //                    command.Parameters.AddWithValue("@passengerCountFrom", passengerCountFrom);
        //                }

        //                if (!string.IsNullOrEmpty(cabType))
        //                {
        //                    command.Parameters.AddWithValue("@cabType", cabType);
        //                }

        //                if (tripDistanceFrom != null)
        //                {
        //                    command.Parameters.AddWithValue("@tripDistanceFrom", tripDistanceFrom);
        //                }

        //                if (totalAmountFrom != null)
        //                {
        //                    command.Parameters.AddWithValue("@totalAmountFrom", totalAmountFrom);
        //                }

        //                if (congestionSurcharge != null)
        //                {
        //                    command.Parameters.AddWithValue("@congestionSurcharge", congestionSurcharge);
        //                }

        //                if (!string.IsNullOrEmpty(paymentType))
        //                {
        //                    command.Parameters.AddWithValue("@paymentType", paymentType);
        //                }

        //                if (!string.IsNullOrEmpty(tripType))
        //                {
        //                    command.Parameters.AddWithValue("@tripType", tripType);
        //                }

        //                using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
        //                {
        //                    DataTable dataTable = new DataTable();
        //                    adapter.Fill(dataTable);

        //                    // Convert the DataTable to a list of dictionaries
        //                    List<Dictionary<string, object>> trips = new List<Dictionary<string, object>>();
        //                    foreach (DataRow row in dataTable.Rows)
        //                    {
        //                        var trip = new Dictionary<string, object>();
        //                        foreach (DataColumn col in dataTable.Columns)
        //                        {
        //                            trip[col.ColumnName] = row[col];
        //                        }

        //                        trips.Add(trip);
        //                    }

        //                    return Ok(trips);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        [HttpGet("Filter")]
        public async Task<IActionResult> GetRidesFiltered(
            DateTime? pickupDateFrom = null,
            DateTime? pickupDateTo = null,
            string vendorId = null,
            int? passengerCountFrom = null,
            int? passengerCountTo = null,
            string cabType = null,
            double? tripDistanceFrom = null,
            double? tripDistanceTo = null,
            double? congestionSurcharge = null,
            string paymentType = null,
            double? totalAmountFrom = null,
            double? totalAmountTo = null,
            string tripType = null,
            int? pageNumber = null, // Default to the first page
            int? pageSize = null // Default page size
        )
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    // Calculate the offset based on pageNumber and pageSize
                    int? offset = null;
                    if (pageNumber != null && pageSize != null)
                        offset = pageNumber * pageSize;

                    string sql = "SELECT * FROM trips WHERE 1=1"; // Initial query with a true condition

                    if (pickupDateFrom != null)
                    {
                        sql += " AND pickup_datetime >= @pickupDateTime";
                    }

                    if (pickupDateTo != null)
                    {
                        sql += " AND pickup_datetime <= @pickupDateTo";
                    }

                    if (!string.IsNullOrEmpty(vendorId))
                    {
                        sql += " AND vendor_id = @vendorId";
                    }

                    if (passengerCountFrom != null)
                    {
                        sql += " AND passenger_count >= @passengerCountFrom";
                    }

                    if (passengerCountTo != null)
                    {
                        sql += " AND passenger_count <= @passengerCountTo";
                    }

                    if (!string.IsNullOrEmpty(cabType))
                    {
                        sql += " AND cab_type = @cabType";
                    }

                    if (tripDistanceFrom != null)
                    {
                        sql += " AND trip_distance >= @tripDistanceFrom";
                    }

                    if (tripDistanceTo != null)
                    {
                        sql += " AND trip_distance <= @tripDistanceTo";
                    }

                    if (totalAmountFrom != null)
                    {
                        sql += " AND total_amount >= @totalAmountFrom";
                    }

                    if (totalAmountTo != null)
                    {
                        sql += " AND total_amount <= @totalAmountTo";
                    }

                    if (congestionSurcharge != null)
                    {
                        sql += " AND congestion_surcharge = @congestionSurcharge";
                    }

                    if (!string.IsNullOrEmpty(paymentType))
                    {
                        //sql += " AND '%' || payment_type || '%' like @paymentType";
                        //paymentType = $"%{paymentType}%"; // Add wildcards to the parameter value
                        //sql += " AND payment_type LIKE @paymentType";
                        var paymentTypes = paymentType.Split(',');
                        sql += " AND payment_type IN (" + string.Join(",", paymentTypes.Select(p => "'" + p + "'")) +
                               ")";
                    }

                    if (!string.IsNullOrEmpty(tripType))
                    {
                        sql += " AND trip_type = @tripType";
                    }

                    // Add pagination: LIMIT clauses
                    if (offset != null)
                        sql += $" LIMIT {offset}, {(pageNumber + 1) * pageSize}";

                    await using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        if (pickupDateFrom != null)
                        {
                            command.Parameters.AddWithValue("@pickupDateTime", pickupDateFrom);
                        }

                        if (pickupDateTo != null)
                        {
                            command.Parameters.AddWithValue("@pickupDateTo", pickupDateTo);
                        }

                        if (!string.IsNullOrEmpty(vendorId))
                        {
                            command.Parameters.AddWithValue("@vendorId", vendorId);
                        }

                        if (passengerCountFrom != null)
                        {
                            command.Parameters.AddWithValue("@passengerCountFrom", passengerCountFrom);
                        }

                        if (passengerCountTo != null)
                        {
                            command.Parameters.AddWithValue("@passengerCountTo", passengerCountTo);
                        }

                        if (!string.IsNullOrEmpty(cabType))
                        {
                            command.Parameters.AddWithValue("@cabType", cabType);
                        }

                        if (tripDistanceFrom != null)
                        {
                            command.Parameters.AddWithValue("@tripDistanceFrom", tripDistanceFrom);
                        }

                        if (tripDistanceTo != null)
                        {
                            command.Parameters.AddWithValue("@tripDistanceTo", tripDistanceTo);
                        }

                        if (totalAmountFrom != null)
                        {
                            command.Parameters.AddWithValue("@totalAmountFrom", totalAmountFrom);
                        }

                        if (totalAmountTo != null)
                        {
                            command.Parameters.AddWithValue("@totalAmountTo", totalAmountTo);
                        }

                        if (congestionSurcharge != null)
                        {
                            command.Parameters.AddWithValue("@congestionSurcharge", congestionSurcharge);
                        }

                        if (!string.IsNullOrEmpty(paymentType))
                        {
                            command.Parameters.AddWithValue("@paymentType", paymentType);
                        }

                        if (!string.IsNullOrEmpty(tripType))
                        {
                            command.Parameters.AddWithValue("@tripType", tripType);
                        }

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Convert the DataTable to a list of Trip objects
                            List<Trip> trips = new List<Trip>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                Trip trip = new Trip
                                {
                                    CabType = row["cab_type"].ToString(),
                                    VendorId = row["vendor_id"].ToString(),
                                    PickupDatetime = (DateTime)row["pickup_datetime"],
                                    DropoffDatetime = (DateTime)row["dropoff_datetime"],
                                    RateCodeId = row["rate_code_id"].ToString(),
                                    PickupLatitude = row["pickup_latitude"] is DBNull
                                        ? 0.0
                                        : (double)row["pickup_latitude"],
                                    PickupLongitude = row["pickup_longitude"] is DBNull
                                        ? 0.0
                                        : (double)row["pickup_longitude"],
                                    DropoffLatitude = row["dropoff_latitude"] is DBNull
                                        ? 0.0
                                        : (double)row["dropoff_latitude"],
                                    DropoffLongitude = row["dropoff_longitude"] is DBNull
                                        ? 0.0
                                        : (double)row["dropoff_longitude"],
                                    PassengerCount = row["passenger_count"] is DBNull ? 0 : (int)row["passenger_count"],
                                    TripDistance = row["trip_distance"] is DBNull ? 0.0 : (double)row["trip_distance"],
                                    FareAmount = row["fare_amount"] is DBNull ? 0.0 : (double)row["fare_amount"],
                                    TipAmount = row["tip_amount"] is DBNull ? 0.0 : (double)row["tip_amount"],
                                    CongestionSurcharge = row["congestion_surcharge"] is DBNull
                                        ? 0.0
                                        : (double)row["congestion_surcharge"],
                                    TotalAmount = row["total_amount"] is DBNull ? 0.0 : (double)row["total_amount"],
                                    PaymentType = row["payment_type"].ToString(),
                                    TripType = row["trip_type"].ToString()
                                };


                                trips.Add(trip);
                            }


                            long totalCount;
                            string countSql = sql.Replace("SELECT * FROM trips WHERE 1=1", "");
                            if (offset != null)
                                countSql = countSql.Remove(countSql.IndexOf("LIMIT"));
                            using (NpgsqlCommand countCommand =
                                   new NpgsqlCommand($"SELECT COUNT(*) FROM trips WHERE 1=1 {countSql}", connection))
                            {
                                foreach (NpgsqlParameter parameter in command.Parameters)
                                {
                                    countCommand.Parameters.Add(parameter.Clone());
                                }

                                totalCount = (long)await countCommand.ExecuteScalarAsync();
                            }

                            var result = new TripFilterResult
                            {
                                Rides = trips, // List of Trip objects
                                TotalCount = totalCount // Total count of trips
                            };

                            return Ok(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetRide")]
        public async Task<IActionResult> GetRide(
            DateTime pickupDateTime,
            DateTime dropoffDateTime,
            double pickupLat,
            double pickupLng
        )
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql =
                        "SELECT * FROM trips WHERE pickup_latitude = @pickupLat " +
                        "AND pickup_longitude = @pickupLng " +
                        "AND pickup_datetime = @pickupDateTime " +
                        "AND dropoff_datetime = @dropoffDateTime";


                    await using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@pickupDateTime", pickupDateTime);
                        command.Parameters.AddWithValue("@dropoffDateTime", dropoffDateTime);
                        command.Parameters.AddWithValue("@pickupLat", pickupLat);
                        command.Parameters.AddWithValue("@pickupLng", pickupLng);

                        using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);
                            DataRow row = dataTable.Rows[0];

                            Trip trip = new Trip
                            {
                                CabType = row["cab_type"].ToString(),
                                VendorId = row["vendor_id"].ToString(),
                                PickupDatetime = (DateTime)row["pickup_datetime"],
                                DropoffDatetime = (DateTime)row["dropoff_datetime"],
                                RateCodeId = row["rate_code_id"].ToString(),
                                PickupLatitude = row["pickup_latitude"] is DBNull
                                    ? 0.0
                                    : (double)row["pickup_latitude"],
                                PickupLongitude = row["pickup_longitude"] is DBNull
                                    ? 0.0
                                    : (double)row["pickup_longitude"],
                                DropoffLatitude = row["dropoff_latitude"] is DBNull
                                    ? 0.0
                                    : (double)row["dropoff_latitude"],
                                DropoffLongitude = row["dropoff_longitude"] is DBNull
                                    ? 0.0
                                    : (double)row["dropoff_longitude"],
                                PassengerCount = row["passenger_count"] is DBNull ? 0 : (int)row["passenger_count"],
                                TripDistance = row["trip_distance"] is DBNull ? 0.0 : (double)row["trip_distance"],
                                FareAmount = row["fare_amount"] is DBNull ? 0.0 : (double)row["fare_amount"],
                                TipAmount = row["tip_amount"] is DBNull ? 0.0 : (double)row["tip_amount"],
                                CongestionSurcharge = row["congestion_surcharge"] is DBNull
                                    ? 0.0
                                    : (double)row["congestion_surcharge"],
                                TotalAmount = row["total_amount"] is DBNull ? 0.0 : (double)row["total_amount"],
                                PaymentType = row["payment_type"].ToString(),
                                TripType = row["trip_type"].ToString()
                            };

                            return Ok(trip);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetStatistic")]
        public async Task<IActionResult> GetStatistic()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to gather statistics
                var query = @"
            SELECT
                COUNT(*) AS total_trips,
                SUM(fare_amount) AS total_revenue,
                AVG(trip_distance) AS avg_trip_distance,
                AVG(fare_amount) AS avg_fare_amount
            FROM trips_2015;

            SELECT
                payment_type,
                COUNT(*) AS payment_count
            FROM trips_2015
            GROUP BY payment_type;

            SELECT
                passenger_count,
                COUNT(*) AS passenger_count_number
            FROM trips_2015
            GROUP BY passenger_count;

            SELECT
                CASE
                    WHEN ROUND(tip_amount) = 0 THEN '0'
                    WHEN ROUND(tip_amount) >= 1 AND ROUND(tip_amount) <= 5 THEN '0-5'
                    WHEN ROUND(tip_amount) >= 6 AND ROUND(tip_amount) <= 10 THEN '5-10'
                    WHEN ROUND(tip_amount) >= 11 AND ROUND(tip_amount) <= 20 THEN '10-20'
                    WHEN ROUND(tip_amount) >= 21 AND ROUND(tip_amount) <= 50 THEN '20-50'
                    WHEN ROUND(tip_amount) >= 51 AND ROUND(tip_amount) <= 100 THEN '50-100'
                    WHEN ROUND(tip_amount) >= 101 AND ROUND(tip_amount) <= 200 THEN '100-200'
                    ELSE '200+'
                END AS tip_range,
                COUNT(*) AS tip_count
            FROM trips_2015
            GROUP BY tip_range;

            SELECT
                CASE
                    WHEN ROUND(tip_amount, 2) = 0.00 THEN '0%'
                    ELSE ROUND((tip_amount / (fare_amount - tip_amount)) * 100, -1)::TEXT || '%'
                END AS tip_percentage_range,
                COUNT(*) AS tip_percentage_count
            FROM trips_2015
            WHERE fare_amount - tip_amount > 0
            GROUP BY tip_percentage_range";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    await using var reader = await command.ExecuteReaderAsync();

                    var statistics = new TripStatistics();

                    var resultCount = 0;

                    do
                    {
                        while (await reader.ReadAsync())
                        {
                            switch (resultCount)
                            {
                                case 0:
                                    statistics.TotalTrips = reader.GetInt32(0);
                                    statistics.TotalRevenue = reader.GetDouble(1);
                                    statistics.AverageTripDistance = reader.GetDouble(2);
                                    statistics.AverageFareAmount = reader.GetDouble(3);
                                    break;
                                case 1:
                                    statistics.PaymentTypeDistribution.Add(reader.GetString(0), reader.GetInt32(1));
                                    break;
                                case 2:
                                    statistics.PassengerCountDistribution.Add(reader.GetInt32(0), reader.GetInt32(1));
                                    break;
                                case 3:
                                    statistics.TipDistribution.Add(reader.GetDouble(0), reader.GetInt32(1));
                                    break;
                                case 4:
                                    // Round the tip percentage to a whole number
                                    var tipPercentage = Math.Round(Convert.ToDouble(reader.GetString(0).TrimEnd('%')));
                                    statistics.TipPercentageDistribution.Add(tipPercentage.ToString() + '%',
                                        reader.GetInt32(1));
                                    break;
                            }
                        }

                        resultCount++;
                    } while (await reader.NextResultAsync());

                    return Ok(statistics);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTotalTripsAndRevenue")]
        public async Task<IActionResult> GetTotalTripsAndRevenue()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to gather total trips and total revenue
                var query = @"
            SELECT
                COUNT(*) AS total_trips,
                SUM(fare_amount) AS total_revenue
            FROM trips_2015";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    await using var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        var totalTrips = reader.GetInt32(0);
                        var totalRevenue = reader.GetDouble(1);

                        var result = new { TotalTrips = totalTrips, TotalRevenue = totalRevenue };

                        return Ok(result);
                    }

                    return BadRequest("No data found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPaymentTypeDistribution")]
        public async Task<IActionResult> GetPaymentTypeDistribution()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to gather payment type distribution
                var query = @"
            SELECT
                payment_type,
                COUNT(*) AS payment_count
            FROM trips_2015
            GROUP BY payment_type";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    await using var reader = await command.ExecuteReaderAsync();

                    var paymentTypeDistribution = new Dictionary<string, int>();

                    while (await reader.ReadAsync())
                    {
                        var paymentType = reader.GetString(0);
                        var paymentCount = reader.GetInt32(1);

                        paymentTypeDistribution.Add(paymentType, paymentCount);
                    }

                    return Ok(paymentTypeDistribution);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPassengerCountDistribution")]
        public async Task<IActionResult> GetPassengerCountDistribution()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to gather passenger count distribution
                var query = @"
            SELECT
                passenger_count,
                COUNT(*) AS passenger_count_number
            FROM trips_2015
            GROUP BY passenger_count";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    await using var reader = await command.ExecuteReaderAsync();

                    var passengerCountDistribution = new Dictionary<int, int>();

                    while (await reader.ReadAsync())
                    {
                        var passengerCount = reader.GetInt32(0);
                        var passengerCountNumber = reader.GetInt32(1);

                        passengerCountDistribution.Add(passengerCount, passengerCountNumber);
                    }

                    return Ok(passengerCountDistribution);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTipDistribution")]
        public async Task<IActionResult> GetTipDistribution()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to gather tip distribution
                var query = @"
            SELECT
                CASE
                    WHEN ROUND(tip_amount) = 0 THEN '0'
                    WHEN ROUND(tip_amount) >= 1 AND ROUND(tip_amount) <= 5 THEN '0-5'
                    WHEN ROUND(tip_amount) >= 6 AND ROUND(tip_amount) <= 10 THEN '5-10'
                    WHEN ROUND(tip_amount) >= 11 AND ROUND(tip_amount) <= 20 THEN '10-20'
                    WHEN ROUND(tip_amount) >= 21 AND ROUND(tip_amount) <= 50 THEN '20-50'
                    WHEN ROUND(tip_amount) >= 51 AND ROUND(tip_amount) <= 100 THEN '50-100'
                    WHEN ROUND(tip_amount) >= 101 AND ROUND(tip_amount) <= 200 THEN '100-200'
                    ELSE '200+'
                END AS tip_range,
                COUNT(*) AS tip_count
            FROM trips_2015
            GROUP BY tip_range";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    await using var reader = await command.ExecuteReaderAsync();

                    var tipDistribution = new Dictionary<string, int>();

                    while (await reader.ReadAsync())
                    {
                        var tipRange = reader.GetString(0);
                        var tipCount = reader.GetInt32(1);

                        tipDistribution.Add(tipRange, tipCount);
                    }

                    return Ok(tipDistribution);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetTipPercentageDistribution")]
        public async Task<IActionResult> GetTipPercentageDistribution()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to gather tip percentage distribution
                var query = @"
            SELECT
                CASE
                    WHEN ROUND(tip_amount, 2) = 0.00 THEN 0
                    ELSE ROUND((tip_amount / (fare_amount - tip_amount)) * 100, -1)
                END AS tip_percentage_range,
                COUNT(*) AS tip_percentage_count
            FROM trips_2015
            WHERE fare_amount - tip_amount > 0
            GROUP BY tip_percentage_range
        ";

                await using (var command = new NpgsqlCommand(query, connection))
                {
                    command.CommandTimeout = 120;
                    await using var reader = await command.ExecuteReaderAsync();

                    var tipPercentageDistribution = new Dictionary<float, int>();

                    while (await reader.ReadAsync())
                    {
                        var tipPercentageRange = reader.GetFloat(0);
                        var tipPercentageCount = reader.GetInt32(1);

                        tipPercentageDistribution.Add(tipPercentageRange, tipPercentageCount);
                    }

                    return Ok(tipPercentageDistribution);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetTimeSeriesAnaysis")]
        public async Task<ActionResult> GetTimeSeriesAnalysis()
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();

                // Query to perform time-series analysis by month
                var monthlyAnalysisQuery = @"
                    SELECT
                        EXTRACT(MONTH FROM pickup_datetime) AS month_number,
                        COUNT(*) AS total_trips
                    FROM trips_2015
                    GROUP BY month_number
                    ORDER BY month_number;";


                // Query to perform time-series analysis by day of the week
                var dayOfWeekAnalysisQuery = @"
                    SELECT
                        EXTRACT(DOW FROM pickup_datetime) AS day_of_week,
                        COUNT(*) AS total_trips
                    FROM trips_2015
                    GROUP BY day_of_week
                    ORDER BY day_of_week;";

                // Query to perform time-series analysis by hour of the day
                var hourOfDayAnalysisQuery = @"
                    SELECT
                        EXTRACT(HOUR FROM pickup_datetime) AS hour_of_day,
                        COUNT(*) AS total_trips
                    FROM trips_2015
                    GROUP BY hour_of_day
                    ORDER BY hour_of_day;";

                var result = new
                {
                    MonthlyAnalysis = await ExecuteTimeSeriesQuery(monthlyAnalysisQuery, connection),
                    DayOfWeekAnalysis = await ExecuteTimeSeriesQuery(dayOfWeekAnalysisQuery, connection),
                    HourOfDayAnalysis = await ExecuteTimeSeriesQuery(hourOfDayAnalysisQuery, connection),
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetHeatmapData")]
        public async Task<IActionResult> GetHeatmapData()
        {
            try
            {
                await using var dataSource = NpgsqlDataSource.Create(connectionString);
                await using (var cmd = dataSource.CreateCommand(@"
                    SELECT pickup_geohash, COUNT(*) as total_rides
                    FROM trips_2015
                    GROUP BY pickup_geohash
                    ORDER BY total_rides DESC"))
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var value = reader.GetInt32(1).ToString();
                    }
                }

                return Ok();
                //
                //await using var connection = new NpgsqlConnection(connectionString);
                //await connection.OpenAsync();


                //var query = @"
                //    SELECT pickup_geohash, COUNT(*) as total_rides
                //    FROM trips_2015
                //    GROUP BY pickup_geohash
                //    ORDER BY total_rides DESC
                //";

                //await using NpgsqlCommand command = new NpgsqlCommand(query, connection);
                //command.CommandTimeout = 360;
                //var heatmapData = new Dictionary<String, Int32>();
                //await using (var reader = await command.ExecuteReaderAsync())
                //{
                //    while (await reader.ReadAsync())
                //    {
                //        var pickupGeohashBytes = reader.GetFieldValue<byte[]>(0);
                //        var pickupGeohash = Encoding.ASCII.GetString(pickupGeohashBytes);
                //        var totalRides = reader.GetInt32(1);
                //        heatmapData.Add(pickupGeohash, totalRides);
                //    }
                //}

                //return Ok(heatmapData);

                ////command.CommandTimeout = 120;
                //await using var reader = await command.ExecuteReaderAsync();

                //var heatmapData = new Dictionary<String, Int32>();

                //while (await reader.ReadAsync())
                //{
                //    var pickupGeohashBytes = reader.GetFieldValue<byte[]>(0);
                //    var pickupGeohash = Encoding.ASCII.GetString(pickupGeohashBytes);
                //    var totalRides = reader.GetInt32(1);

                //    // Convert pickupGeohash to latitude and longitude (implement this conversion)

                //    //var heatmapPoint = new HeatmapPoint
                //    //{
                //    //    Geohash = pickupGeohash,
                //    //    TotalRides = totalRides
                //    //};
                //    heatmapData.Add(pickupGeohash, totalRides);
                //}

                //return Ok(heatmapData);
                //using var command = new NpgsqlCommand(query, connection);
                //using var reader = command.ExecuteReader();
                //var heatmapData = new List<HeatmapPoint>();

                //while (reader.Read())
                //{
                //    var pickupGeohashBytes = reader.GetFieldValue<byte[]>(0);
                //    var pickupGeohash = Encoding.ASCII.GetString(pickupGeohashBytes);
                //    var totalRides = reader.GetInt32(1);

                //    // Convert pickupGeohash to latitude and longitude (implement this conversion)

                //    var heatmapPoint = new HeatmapPoint
                //    {
                //        Geohash = pickupGeohash,
                //        TotalRides = totalRides
                //    };

                //    heatmapData.Add(heatmapPoint);
                //}

                //return Ok(heatmapData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private async Task<Dictionary<string, int>> ExecuteTimeSeriesQuery(string query, NpgsqlConnection connection)
        {
            await using var command = new NpgsqlCommand(query, connection);
            //command.CommandTimeout = 120;
            await using var reader = await command.ExecuteReaderAsync();

            var timeSeriesData = new Dictionary<string, int>();

            while (await reader.ReadAsync())
            {
                var key = reader[0]
                    .ToString(); // Assuming the first column is the key (e.g., month, day of the week, hour of the day)
                var value = reader.GetInt32(1); // Assuming the second column is the count
                timeSeriesData[key] = value;
            }

            return timeSeriesData;
        }
    }
}