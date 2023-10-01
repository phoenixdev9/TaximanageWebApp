import { NextApiRequest, NextApiResponse } from 'next';
import { NextResponse } from 'next/server';

export async function GET(req: NextApiRequest, res: NextApiResponse) {
    try {
        const query = `
        SELECT
            CEIL(trip_distance) AS distance_rounded_to_mile,
            AVG(tip_amount) AS average_tip_amount
        FROM
            trips
        GROUP BY
            distance_rounded_to_mile
        ORDER BY
            distance_rounded_to_mile;
    `;

        // Send a GET request to QuestDB
        const response = await fetch(`${process.env.QUESTDB_REST_URL}?query=${query}`, {
            headers: {
                'Authorization': 'Basic ' + btoa(process.env.QUESTDB_REST_USER + ":" + process.env.QUESTDB_REST_PASS),
            },
        })

        if (!response.ok) {
            throw new Error('Failed to fetch data from QuestDB');
        }
        
        const data = await response.json();
        const dataset = data.dataset;
        // const totalAmount = dataset[0];

        // const retVal: ChargeDistribution = {
        //     totalFare: dataset[1] / totalAmount * 100,
        //     totalExtra: dataset[2] / totalAmount * 100,
        //     totalMta: dataset[3] / totalAmount * 100,
        //     totalTips: dataset[4] / totalAmount * 100,
        //     totalTolls: dataset[5] / totalAmount * 100,
        //     totalImprovment: dataset[6] / totalAmount * 100,
        // }

        return NextResponse.json([...dataset]);
    } catch (error) {
        console.error(error);
        res.status(500).json({ error: 'Internal server error' });
    }
}
