import { useState, useEffect, useCallback } from 'react';
import { api } from '@/lib/api';

interface UserData {
    userName: string;
    coins: number;
}

export function useUserData() {
     const [userData, setUserData] = useState<UserData>({
        userName: '',
        coins: 0
    });

    const fetchUserData = useCallback(async () => {
        try {
            const data = await api.inventory.userData();
            setUserData({
                userName: data.name,
                coins: data.coins
            });
        } catch (error) {
            console.error('An error occurred during data fetching:', error);
        }
    }, []);

    useEffect(() => {
        fetchUserData();
    }, [fetchUserData]);

    return {
        userName: userData.userName,
        coins: userData.coins,
        refreshUserData: fetchUserData
    };
}