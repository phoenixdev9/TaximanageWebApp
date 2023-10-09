"use client"
import RideService, { } from '@/services/RideService';
import axios from 'axios';
import dynamic from 'next/dynamic';
import { useEffect, useRef, useState } from 'react';
import { LoadingSpinner } from '@/components/common/loadingSpinner/loadingSpinner';
import dayjs from 'dayjs';
import { useRouter } from 'next/navigation';

const Map = dynamic(() => import('../../components/realtimeMap/realTimeMap'), {
    ssr: false,
});

export default function Page() {
    const [rides, setRides] = useState<Ride[]>([]);
    const [loading, setLoading] = useState(true);
    const fetching = useRef(false);

    const fetchRides = async () => {
        try {
            // let prevTime = undefined
            let reqTime = dayjs()
            while (true) {
                const { data: response, status } = await RideService.getRealTimeRides(reqTime);
                if (status != axios.HttpStatusCode.Ok)
                    break;
                //TODO: Add completed rides state to avoid unneceserry rerender
                //also check for response including already completed rides and filter them out of response
                setRides(prev => {
                    let prevKeep = prev.filter(p => !p.dropoffDatetime
                        && !response.rides.find(r => r.pickupDatetime == p.pickupDatetime
                            && r.pickupLatitude == p.pickupLatitude
                            && r.pickupLongitude == p.pickupLongitude
                            && r.passengerCount == p.passengerCount)
                        || p.dropoffDatetime && !response.rides.find(r => r.pickupDatetime == p.pickupDatetime
                            && r.pickupLatitude == p.pickupLatitude
                            && r.pickupLongitude == p.pickupLongitude
                            && r.passengerCount == p.passengerCount)
                    )
                    return ([...prevKeep, ...response.rides])
                });
                reqTime = dayjs(response.reqTime)
            }
        } catch (error) {
            if (axios.isAxiosError(error)) console.error(error.message);
        }
    };
    useEffect(() => {
        setLoading(true);
        if (!fetching.current) {
            fetching.current = true;
            fetchRides();
        }
        setLoading(false);
    }, []);

    //TODO: Add clear rides button to clear states

    const router = useRouter()
    return (
        <div className="min-h-screen items-center justify-between p-2 h-screen bg-white">
            {loading && (
                <div className="flex flex-grow justify-center absolute top-1/2 left-1/2 z-10 w-24 h-24 bg-white shadow-lg rounded-lg">
                    <div className="flex flex-col justify-center">
                        <LoadingSpinner />
                    </div>
                </div>
            )}
            <div className='h-full w-full'>
                {!loading && <Map rides={rides} />}
            </div>
        </div>
    )
}