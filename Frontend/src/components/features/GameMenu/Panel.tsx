import React, {useEffect, useState} from 'react';
import { api } from '@/lib/api'; // Import the new api object
import './GameMenu.css';
import './ItemGrid.css';
import { getItemMetadata } from '@/lib/itemsMetadata';

// Тип для ключей панелей
type PanelType = 'inventory' | 'trade' | 'craft';

// Интерфейс для элемента инвентаря с сервера
interface InventoryItemDTO {
    id: string;
    quantity: number;
}

interface TradeItemDTO {
    id: string;
    itemId: string;
    price: number;
    seller: string;
}

const Panel: React.FC = () => {
    // Состояние для активной панели
    const [activePanel, setActivePanel] = useState<PanelType>('inventory');
    const [inventoryItems, setInventoryItems] = useState<InventoryItemDTO[]>([]);
    const [tradeItems, setTradeItems] = useState<TradeItemDTO[]>([]);

    // Обработчик клика по кнопкам
    const handleMenuClick = (item: PanelType) => {
        setActivePanel(item);
    };

    const fetchInventory = async () => {
        try {
            const data: InventoryItemDTO[] = await api.inventory.getUserInventoryItems();
            setInventoryItems(data);
        } catch (err) {
            console.error('Failed to fetch inventory:', err);
        }
    };

    const fetchTrade = async () => {
        try {
            const data: TradeItemDTO[] = await api.inventory.getTradeItems();
            setTradeItems(data);
        } catch (err) {
            console.error('Failed to fetch trade:', err);
        }
    };

    const handleBuy = (itemId : string) => {
        console.log('Покупка товара с ID:', itemId);
        try {
            api.trade.buyTrade(itemId);
        } catch (err) {
            console.error('Failed to fetch trade:', err);
        }
    };

    useEffect(() => {
        if (activePanel === 'inventory') {
            fetchInventory();
        } else if (activePanel === 'trade') {
            fetchTrade();
        }
    }, [activePanel]);

    // Временные данные для крафта
    const craftItems = ['Sword: Wood x5 + Iron x2', 'Shield: Wood x3 + Iron x1', 'Axe: Wood x2 + Stone x3'];

    return (
        <div className="panel">
            {/* Бар с кнопками переключения */}
            <div className="panel-tabs">
                <button
                    className={`tab-button ${activePanel === 'inventory' ? 'active' : ''}`}
                    onClick={() => handleMenuClick('inventory')}
                >
                    Inventory
                </button>
                <button
                    className={`tab-button ${activePanel === 'trade' ? 'active' : ''}`}
                    onClick={() => handleMenuClick('trade')}
                >
                    Trade
                </button>
                <button
                    className={`tab-button ${activePanel === 'craft' ? 'active' : ''}`}
                    onClick={() => handleMenuClick('craft')}
                >
                    Craft
                </button>
            </div>

            {/* Контент панели */}
            <div className="panel-content">
                {activePanel === 'inventory' && (
                    <div className="inventory-list">
                        {inventoryItems.map((item, index) => {
                            const metadata = getItemMetadata(item.id);
                            return (
                                <div key={`${item.id}-${index}`} className="inventory-row">
                                    <img
                                        src={metadata.icon}
                                        alt={metadata.name}
                                        className="item-icon"
                                    />
                                    <span className="item-name">{metadata.name}</span>
                                    <span className="item-quantity">x{item.quantity}</span>
                                </div>
                            );
                        })}
                    </div>
                )}

                {activePanel === 'trade' && (
                    <div className="inventory-list"> {/* Используем тот же класс для похожего стиля */}
                        {tradeItems.map((item) => {
                            const metadata = getItemMetadata(item.itemId);
                            return (
                                <div key={item.id} className="inventory-row trade-row">
                                    <img
                                        src={metadata.icon}
                                        alt={metadata.name}
                                        className="item-icon"
                                    />
                                    <span className="item-name">{metadata.name}</span>
                                    <span className="item-price">{item.price} gold</span>
                                    <span className="item-seller">by {item.seller}</span>
                                    <button className="buy-button" onClick={() => handleBuy(item.id)}>Buy</button>
                                </div>
                            );
                        })}
                    </div>
                )}

                {activePanel === 'craft' && (
                    <ul>
                        {craftItems.map((item, index) => (
                            <li key={index}>{item}</li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    );
};

export default Panel;