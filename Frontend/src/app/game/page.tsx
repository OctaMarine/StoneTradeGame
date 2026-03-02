'use client';

import { useState, useEffect } from 'react';
import { api } from '@/lib/api';
import '@/components/features/GameMenu/GameMenu.css';
import Panel from '@/components/features/GameMenu/Panel';

export default function Game() {
    const [coins, setCoins] = useState('');
    const [userName, setUserName] = useState('');
    
    useEffect(() => {
        api.inventory.userData()
            .then(data => {
                // Предполагается, что API возвращает объект с полями 'name' и 'coins'
                // Например: { "name": "Player1", "coins": 100 }
                setUserName(data.name);
                setCoins(data.coins);
            })
            .catch(error => {
                console.error('An error occurred during data fetching:', error);
            });
    }, []); // Пустой массив зависимостей гарантирует, что эффект выполнится один раз после монтирования компонента

    return (
        <div className="App">
            <div style={{
                position: 'absolute',
                top: '50px',
                left: '15%',
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

            <Panel />
        </div>
    );
}
