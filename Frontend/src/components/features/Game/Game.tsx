import { useState, useEffect, useCallback , useRef} from 'react';
import { api } from '@/lib/api';
import { Link } from 'react-router-dom';
import '@/components/features/Game/Game.css';
import '@/components/features/Panel/Panel.css';
import Panel from '@/components/features/Panel/Panel';
import { useUserData } from '@/hooks/useUserData';

export default function Game() {
    const { userName, coins, refreshUserData } = useUserData();

    const refreshInventoryRef = useRef<(() => void) | null>(null);

    const handleAddSupply = async () => {
        try {
            await api.inventory.addSupply();
            refreshInventoryRef.current?.();
        } catch (error) {
            console.error('Ошибка при выполнении поставки:', error);
        }
    }; // Зависимости пустые, так как функция не зависит от внешних переменных

    const handleExit = () => {
        console.log('Выход');
    }; // Пустой массив зависимостей - эффект выполнится только один раз

    return (
        <div className="App">
            <div className="action-buttons-container-left-aligned">
                <img 
                    className="player-icons" 
                    src="icons/player.png" 
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
                        Выйти
                    </button>
                </Link>
            </div>
            <div className="action-buttons-container-right-aligned" style={{ marginTop: '70px'}}>
                <Link to="/leveling">
                    <button className="action-button">
                        Прокачка
                    </button>
                </Link>
            </div>
            <div className="action-buttons-container-right-aligned" style={{ marginTop: '140px'}}>
                <button className="action-button">
                    Настройки
                </button>
            </div>
            <div className="action-buttons-container-right-aligned" style={{ marginTop: '210px'}}>
                <button className="action-button">
                    Игроки
                </button>
            </div>
            <div className="action-buttons-container-right-aligned" style={{ marginTop: '280px'}}>
                <button 
                    className="action-button" 
                    onClick={handleAddSupply}>
                    Поставка
                </button>
            </div>
            <Panel onRefreshUserData={refreshUserData} 
                   onRefreshInventory={(refreshFn) => {
                   refreshInventoryRef.current = refreshFn;
            }}/>
        </div>
    );
}
