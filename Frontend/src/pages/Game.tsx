import { useState, useEffect, useCallback } from 'react';
import { api } from '@/lib/api';
import { Link } from 'react-router-dom';
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
            <div style={{
                position: 'absolute',
                top: '50px',
                left: '25%',
                transform: 'translateX(-50%)'
            }}>
                <img className="App-logo" src="../favicon.ico" height={50} width={50}/>
            </div>

            <div style={{
                textAlign: 'center',
                padding: '2.5rem 1rem 1rem'
            }}>
                <header style={{
                    fontFamily: "'Inter', -apple-system, sans-serif",
                    fontWeight: 400,
                    fontSize: '1.5rem',
                    color: '#c1fde9',
                    margin: '0 0 0.5rem 0',
                    lineHeight: 1.4,
                    letterSpacing: '-0.01em'
                }}>
                    Hello, {userName || 'Guest'}!
                </header>

                <div style={{
                    fontFamily: "'Inter', monospace",
                    fontWeight: 500,
                    fontSize: '1.25rem',
                    color: '#c1fde9',
                    lineHeight: 1.3
                }}>
                    Your coins: {coins || '0'}
                </div>
            </div>
            <div style={{
                position: 'absolute',
                top: '50px',
                right: '3%'
            }}>
                <Link to="/login">
                    <button
                        style={{
                            border: 'none',
                            background: '#090606',
                            cursor: 'pointer',
                            zIndex: 1000,
                            borderRadius: '12px',
                            padding: 0,
                            overflow: 'hidden',
                        }}
                    >
                        <img
                            src="file.svg"
                            alt="Выйти"
                            height={50}
                            width={50}
                            style={{
                                borderRadius: '12px',
                                display: 'block',
                            }}
                        />
                    </button>
                </Link>
            </div>

            <Panel onTradeAction={fetchUserData} />
        </div>
    );
}