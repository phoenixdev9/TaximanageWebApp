import { NextApiRequest } from 'next';
import { NextResponse } from 'next/server'

export async function GET(req: NextApiRequest) {

    const url = new URL(req.url as string)

    const query = `
    SELECT
        'Total Number of Trips' AS statistic_name,
        COUNT(*) AS statistic_value
    FROM
        trips
    UNION ALL
    SELECT
        'Average Trip Distance',
        AVG(trip_distance)
    FROM
        trips
    UNION ALL
    SELECT
        'Average Fare Amount',
        AVG(fare_amount)
    FROM
        trips
    UNION ALL
    SELECT
        'Average Tip Amount',
        AVG(tip_amount)
    FROM
        trips
    UNION ALL
    SELECT
        'Average Tip Percentage',
        AVG(tip_percentage) AS average_tip_percentage
    FROM
    (
        SELECT
        (tip_amount / (total_amount - tip_amount)) * 100 AS tip_percentage
        FROM
        trips
        WHERE
        fare_amount > 0
    );
    UNION ALL
    SELECT
        'Max Tip Amount',
        MAX(tip_amount)
    FROM
        trips
    UNION ALL
    SELECT
        'Average Trip Time(m)',
        AVG(datediff('m', pickup_datetime, dropoff_datetime))
    FROM
        trips
    `;

    try {

        const res = await fetch(`${process.env.QUESTDB_REST_URL}?query=${encodeURIComponent(query)}`, {
            headers: {
                'Authorization': 'Basic ' + btoa(process.env.QUESTDB_REST_USER + ":" + process.env.QUESTDB_REST_PASS),
            },
        })
        const data = await res.json()
        const response = data.dataset
        return NextResponse.json(response)
    } catch (error) {
        console.error(error);
        // res.status(500).json({ error: 'Internal server error' });
    }
}