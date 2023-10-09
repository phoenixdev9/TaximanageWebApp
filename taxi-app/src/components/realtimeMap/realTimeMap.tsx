"use client"
import "leaflet/dist/leaflet.css"
import { MapContainer, Marker, Popup, TileLayer } from "react-leaflet";
import L from 'leaflet';
import inProgressIcon from '/public/rideInProgress.png'
import doneIcon from '/public/rideDone.png'
import MarkerClusterGroup from 'react-leaflet-cluster'
import { Button, Icon } from "@mui/material";
import { useRouter } from "next/navigation";
import dayjs from "dayjs";

let rideInProgressIcon = L.icon({
    iconUrl: inProgressIcon.src,
    iconSize: [32, 32],
    iconAnchor: [16, 32]
})
let rideDoneIcon = L.icon({
    iconUrl: doneIcon.src,
    iconSize: [32, 32],
    iconAnchor: [10, 6]
})

interface RealTiemMapProps {
    rides: Ride[]
}

export default function RealTimeMap({ rides }: RealTiemMapProps) {
    const router = useRouter()
    console.log(rides)
    return (
        <MapContainer center={[40.730610, -73.935242]} zoom={13} scrollWheelZoom={true} className="w-full h-full"
            preferCanvas={true}
        >
            <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />
            <MarkerClusterGroup
                chunkedLoading
            >
                {rides.length > 0 && (rides).map((ride, index) => (
                    <Marker
                        key={index}
                        position={[ride.pickupLongitude, ride.pickupLatitude]}
                        icon={ride.dropoffDatetime ? rideDoneIcon : rideInProgressIcon}
                    >
                        <Popup>
                            <p><strong>Pickup: </strong>{dayjs(ride.pickupDatetime).toString()}</p>
                            {/* <div className="w-full "></div> */}
                            {/* <Button variant="outlined" fullWidth startIcon={<Icon className="material-symbols-outlined">info</Icon>}
                                onClick={() =>
                                    router.push(`/ride?pickup=${ride.pickupDatetime}&dropoff=${ride.dropoffDatetime}&lat=${ride.pickupLatitude}&lng=${ride.pickupLongitude}`)}>
                                Ride Details
                            </Button> */}
                        </Popup></Marker>
                ))}
            </MarkerClusterGroup>
        </MapContainer>
    )
}