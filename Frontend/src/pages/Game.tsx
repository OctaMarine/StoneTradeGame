import { useState, useEffect, useCallback } from 'react';
import { api } from '@/lib/api';
import { Link } from 'react-router-dom';
import '@/components/features/Game/Game.css';
import '@/components/features/GameMenu/GameMenu.css';
import Panel from '@/components/features/GameMenu/Panel';

export default function Game() {
    const [coins, setCoins] = useState<number>(0);
    const [userName, setUserName] = useState<string>('');

    const fetchUserData = useCallback(async () => {
        try {
            const data = await api.inventory.userData();
            // Используем функциональную форму setState для предотвращения батчинга
            setUserName(data.name);
            setCoins(data.coins);
        } catch (error) {
            console.error('An error occurred during data fetching:', error);
            // Опционально: показать пользователю сообщение об ошибке
        }
    }, []); // Зависимости пустые, так как функция не зависит от внешних переменных

    const handleExit = () => {
        console.log('Выход');
    };

    useEffect(() => {
        let isMounted = true; // Флаг для предотвращения обновления состояния на размонтированном компоненте

        const loadData = async () => {
            try {
                const data = await api.inventory.userData();
                if (isMounted) {
                    setUserName(data.name);
                    setCoins(data.coins);
                }
            } catch (error) {
                if (isMounted) {
                    console.error('An error occurred during data fetching:', error);
                }
            }
        };

        loadData();

        return () => {
            isMounted = false; // Очистка при размонтировании
        };
    }, []); // Пустой массив зависимостей - эффект выполнится только один раз

    // Альтернативный вариант с useCallback (если fetchUserData нужно передавать в дочерние компоненты)
    useEffect(() => {
        fetchUserData();
    }, [fetchUserData]);

    return (
        <div className="App">
            <div className="action-buttons-container-left-aligned">
                <img 
                    className="action-button-img" 
                    src="../favicon.ico" 
                />
            </div>

            <div className="game-info-container">
                <header className="greeting-header">
                    Hello, {userName || 'Guest'}!
                </header>

                <div className="coin-display">
                    Your coins: {coins || '0'}
                </div>
            </div>
            <div className="action-buttons-container-right-aligned">
                <Link to="/login">
                    <button className="action-button">
                        <img
                            className="action-button-img"
                            src="file.svg"
                            alt="Выйти"
                        />
                    </button>
                </Link>
            </div>
            <div className="action-buttons-container-right-aligned" style={{ marginTop: '70px'}}>
                <button className="action-button">
                    <img
                        src="file.svg"
                        alt="Настройки"
                        className="action-button-img"
                    />
                </button>
            </div>
            <div className="action-buttons-container-right-aligned" style={{ marginTop: '140px'}}>
                <button className="action-button">
                    <img
                        src="file.svg"
                        alt="Игроки"
                        className="action-button-img"
                    />
                </button>
            </div>

            <Panel onTradeAction={fetchUserData} />
        </div>
    );
}
