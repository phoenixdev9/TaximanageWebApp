"use client"
import RideService, { FilterCriteria, PaginationCriteria } from '@/services/RideService';
import { Accordion, AccordionSummary, Typography, AccordionDetails, Pagination } from '@mui/material';
import axios from 'axios';
import dynamic from 'next/dynamic';
import { useEffect, useState } from 'react';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import RideFilter from '@/components/rideFilter/rideFilter';
import { initialFilterCriteria } from '../list/page';
import { LoadingSpinner } from '@/components/common/loadingSpinner/loadingSpinner';

const Map = dynamic(() => import('../../components/ridesMap/ridesMap'), {
    ssr: false,
});


export default function Page() {
    const [rides, setRides] = useState<Ride[]>([]);
    const [rowCountState, setRowCountState] = useState<number>();
    const [loading, setLoading] = useState(true);
    const [filterCriteria, setFilterCriteria] = useState<FilterCriteria>(initialFilterCriteria);
    const [paginationModel, setPaginationModel] = useState<PaginationCriteria>({
        page: 0,
        pageSize: 10000,
    });

    useEffect(() => {
        const fetchRides = async () => {
            try {
                const { data: response, status } = await RideService.getRides(paginationModel);
                setRides(response.rides);
                setRowCountState((prevRowCountState) =>
                    response.totalCount !== undefined ? response.totalCount : prevRowCountState,
                );
            } catch (error) {
                if (axios.isAxiosError(error)) console.error(error.message);
            }
        };
        setLoading(true);
        Promise.resolve(fetchRides()).then(() => setLoading(false))
    }, [paginationModel.page]);

    const handleFilterSubmit = () => {
        const filterRides = async () => {
            try {
                const { data: response, status } = await RideService.getRides(paginationModel, filterCriteria);
                setRides(response.rides);
                setRowCountState((prevRowCountState) =>
                    response.totalCount !== undefined ? response.totalCount : prevRowCountState,
                );
            } catch (error) {
                if (axios.isAxiosError(error)) console.error(error.message);
            }
        }
        setLoading(true)
        Promise.resolve(filterRides()).then(() => setLoading(false))
    }

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setPaginationModel(prev => ({ ...prev, page: value - 1 }));
    };

    return (
        <div className="min-h-screen items-center justify-between p-2 h-screen bg-white">
            {loading && (
                <div className="flex flex-grow justify-center absolute top-1/2 left-1/2 z-10 w-24 h-24 bg-white shadow-lg rounded-lg">
                    <div className="flex flex-col justify-center">
                        <LoadingSpinner />
                    </div>
                </div>
            )}
            <Accordion className='mb-2'>
                <AccordionSummary
                    expandIcon={<ExpandMoreIcon />}
                    aria-controls="panel1a-content"
                    id="panel1a-header"
                >
                    {<Typography>{"Filter rides"}{rowCountState &&
                        `:\t\t${paginationModel.pageSize * paginationModel.page + 1}-${paginationModel.pageSize * (paginationModel.page + 1) + 1}  of ${rowCountState}`}
                    </Typography>}
                </AccordionSummary>
                <AccordionDetails className='bg-white'>
                    <RideFilter
                        fetchFilteredRides={handleFilterSubmit}
                        filterCriteria={filterCriteria}
                        setFilterCriteria={setFilterCriteria} />
                    {rowCountState &&
                        <Pagination count={Math.ceil(rowCountState / paginationModel.pageSize)}
                            color="primary" onChange={handlePageChange} />}

                </AccordionDetails>
            </Accordion>
            <div className='h-5/6 w-full'>
                {!loading && <Map rides={rides} />}
            </div>
        </div>
    )
}