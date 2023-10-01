"use client"
import RideService, { FilterCriteria, PaginationCriteria } from '@/services/RideService';
import { Accordion, AccordionSummary, Typography, AccordionDetails, Pagination, Box, Tabs, Tab } from '@mui/material';
import axios from 'axios';
import { useEffect, useState } from 'react';
import RideFilter from '@/components/rideFilter/rideFilter';
import { initialFilterCriteria } from '../../list/page';
import { BarChart, Bar, Cell, XAxis, YAxis, CartesianGrid, Tooltip, Legend, ResponsiveContainer, ScatterChart, Scatter, Pie, PieChart, LineChart, Line, RadarChart, PolarAngleAxis, PolarGrid, PolarRadiusAxis, Radar } from 'recharts';
import StatisticsService from '@/services/StatisticsService';
import { useRouter } from 'next/navigation';
import { LoadingSpinner } from '@/components/common/loadingSpinner/loadingSpinner';

interface GraphData {
    name: string,
    Rides: number
}
const COLORS = ['#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#FF33FF', '#33CCFF', '#FF6633'];
const WEEKDAYS = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];


export default function Page() {
    const [loading, setLoading] = useState(false);
    const [filterCriteria, setFilterCriteria] = useState<FilterCriteria>(initialFilterCriteria);
    const [tipPercentageDistribution, setTipPercentageDistribution] = useState<[]>([])
    const [chargeDistribution, setChargeDistribution] = useState([]);
    const [tipPerDistanceDistribution, setTipPerDistanceDistribution] = useState([]);
    const [paymentTypeDistribution, setPaymentTypeDistribution] = useState([]);
    const router = useRouter()

    useEffect(() => {
        // const fetchTipPercentageDistribution = async () => {
        //     try {
        //         const { data: response, status } = await StatisticsService.getTipPercentageDistribution();
        //         setTipPercentageDistribution(Object.keys(response).map((key) => ({
        //             name: key,
        //             Rides: response[key],
        //         })))
        //     } catch (error) {
        //         if (axios.isAxiosError(error)) console.error(error.message);
        //     }
        // };
        const fetchChargeDistribution = async () => {
            try {
                const { data: response, status } = await StatisticsService.getChargeDistribution();
                setChargeDistribution(response)
            } catch (error) {
                if (axios.isAxiosError(error)) console.error(error.message);
            }
        };
        const fetchTipPerDistanceDistribution = async () => {
            try {
                const { data: response, status } = await StatisticsService.getTipPerDistanceDistribution();
                setTipPerDistanceDistribution(response.map(([name, pv]) => ({ name: name.toString(), pv })))
            } catch (error) {
                if (axios.isAxiosError(error)) console.error(error.message);
            }
        };
        const fetchPaymentTypeDistribution = async () => {
            try {
                const { data: response, status } = await StatisticsService.getPaymentTypeDistribution();
                let otherTypeCount = response[2][1] + response[3][1] + response[4][1];
                let res = [response[0], response[1], ['Other', otherTypeCount]]
                const formatedData = res.map((item) => ({
                    name: item[0],
                    percent: item[1],
                }));
                setPaymentTypeDistribution(formatedData)
            } catch (error) {
                if (axios.isAxiosError(error)) console.error(error.message);
            }
        };
        setLoading(true);
        // fetchTipPercentageDistribution();
        // fetchTipPerDistanceDistribution();
        // fetchChargeDistribution();
        Promise.all([fetchTipPerDistanceDistribution(), fetchChargeDistribution(), fetchPaymentTypeDistribution()]).then(() => setLoading(false))
        // setLoading(false);
    }, []);

    const formatRadarChartData = (responseData) => {
        return [
            // { subject: 'Fare', A: responseData.totalFare },
            { subject: 'Extra', A: responseData.totalExtra },
            { subject: 'MTA Tax', A: responseData.totalMta },
            { subject: 'Tips', A: responseData.totalTips },
            { subject: 'Tolls', A: responseData.totalTolls },
            { subject: 'Improvement', A: responseData.totalImprovment },
        ];
    };
    // const handleFilterSubmit = () => {
    //     const filterRides = async () => {
    //         try {
    //             const { data: response, status } = await RideService.getRides(paginationModel, filterCriteria);
    //             setRides(response.rides);
    //             setRowCountState((prevRowCountState) =>
    //                 response.totalCount !== undefined ? response.totalCount : prevRowCountState,
    //             );
    //         } catch (error) {
    //             if (axios.isAxiosError(error)) console.error(error.message);
    //         }
    //     }
    //     setLoading(true)
    //     filterRides()
    //     setLoading(false)
    // }

    const [value, setValue] = useState(0);

    const handleChange = (event: React.SyntheticEvent, newValue: number) => {
        // event.type can be equal to focus with selectionFollowsFocus.
        if (
            event.type !== 'click'
            // ||
            // (event.type === 'click' &&
            //     samePageLinkNavigation(
            //         event as React.MouseEvent<HTMLAnchorElement, MouseEvent>,
            //     ))
        ) {
            setValue(newValue);
        }
    };

    return (
        <div className="min-h-screen items-center justify-between p-2 h-screen bg-white text-blue-800">
            {/* <RideFilter
                fetchFilteredRides={handleFilterSubmit}
                filterCriteria={filterCriteria}
                setFilterCriteria={setFilterCriteria} /> */}
            {loading && (
                <div className="flex flex-grow justify-center absolute top-1/2 left-1/2 z-10 w-24 h-24 bg-white shadow-lg rounded-lg">
                    <div className="flex flex-col justify-center">
                        <LoadingSpinner />
                    </div>
                </div>
            )}
            <Box sx={{ width: '100%' }}>
                <Tabs value={1} onChange={handleChange} aria-label="nav tabs example">
                    <Tab label="Time series statistics" onClick={() => router.push("/statistics/time")} />
                    <Tab label="Financial statistics" />
                    <Tab label="General statistics" onClick={() => router.push("/statistics/general")} />
                </Tabs>
            </Box>
            <span>Based on 161,843,930 rides</span>
            <div className='h-full w-full flex flex-row flex-wrap'>
                {/* <ResponsiveContainer width="50%" height={400}>
                    <ScatterChart
                        margin={{
                            top: 20,
                            right: 20,
                            bottom: 20,
                            left: 20,
                        }}
                    >
                        <CartesianGrid />
                        <XAxis type="number" dataKey="name" name="stature" unit="cm" />
                        <YAxis type="number" dataKey="Rides" name="weight" unit="kg" />
                        <Tooltip cursor={{ strokeDasharray: '3 3' }} />
                        <Scatter name="A school" data={tipPercentageDistribution} fill="#8884d8" />
                    </ScatterChart>
                </ResponsiveContainer> */}

                {/* TODO: NOTEPAD++ spisak */}
                {/* Tip percentage Distribution */}
                {/* <ResponsiveContainer width="50%" height="50%">
                    <LineChart
                        width={500}
                        height={300}
                        data={tipPercentageDistribution}
                        margin={{
                            top: 5,
                            right: 30,
                            left: 20,
                            bottom: 5,
                        }}
                    >
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="name" scale="log" />
                        <YAxis />
                        <Tooltip />
                        <Legend />
                        <Line type="monotone" dataKey="Rides" stroke="#8884d8" activeDot={{ r: 8 }} />
                    </LineChart>
                </ResponsiveContainer> */}
                <ResponsiveContainer width="28%" height="40%">
                    <RadarChart cx="50%" cy="50%" outerRadius="80%" data={formatRadarChartData(chargeDistribution)}>
                        <PolarGrid />
                        <PolarAngleAxis dataKey="subject" />
                        <PolarRadiusAxis />
                        <Radar name="Mike" dataKey="A" stroke="#8884d8" fill="#8884d8" fillOpacity={0.6} />
                    </RadarChart>
                </ResponsiveContainer>
                <ResponsiveContainer width="72%" height="40%">
                    <LineChart
                        width={500}
                        height={300}
                        data={tipPerDistanceDistribution} // Use the formatted data here
                        margin={{
                            top: 5,
                            right: 30,
                            left: 20,
                            bottom: 5,
                        }}
                    >
                        <CartesianGrid strokeDasharray="3 3" />
                        <XAxis dataKey="name" /*unit={'mi'}*/ />
                        <YAxis unit={'$'} />
                        <Tooltip />
                        <Legend />
                        <Line type="monotone" dataKey="pv" stroke="#8884d8" activeDot={{ r: 4 }} dot={{ r: 2 }} />
                    </LineChart>
                </ResponsiveContainer>
                <ResponsiveContainer width="33%" height="25%">
                    <PieChart width={300} height={250}>
                        <Pie data={paymentTypeDistribution} dataKey="percent" cx="50%" cy="50%" outerRadius={60} fill="#8884d8" label={(en) => en.name}>
                            {paymentTypeDistribution.map((entry, index) => (
                                <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                            ))}
                        </Pie>
                        <Tooltip />
                        <Legend />
                    </PieChart>
                </ResponsiveContainer>
            </div>

        </div>
    )
}