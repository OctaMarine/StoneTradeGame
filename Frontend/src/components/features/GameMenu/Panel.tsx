import React, {useEffect, useState} from 'react';
import { api } from '@/lib/api';
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

// Интерфейс для пропсов компонента Panel
interface PanelProps {
    onTradeAction: () => void;
}

const Panel: React.FC<PanelProps> = ({ onTradeAction }) => {
    // Состояние для активной панели
    const [activePanel, setActivePanel] = useState<PanelType>('inventory');
    const [inventoryItems, setInventoryItems] = useState<InventoryItemDTO[]>([]);
    const [tradeItems, setTradeItems] = useState<TradeItemDTO[]>([]);

    // State for Sell Modal
    const [isSellModalOpen, setIsSellModalOpen] = useState(false);
    const [selectedItem, setSelectedItem] = useState<InventoryItemDTO | null>(null);
    const [sellPrice, setSellPrice] = useState<number>(0);

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

    const handleBuy = async (itemId : string) => {
        console.log('Покупка товара с ID:', itemId);
        try {
            await api.trade.buyTrade(itemId);
            onTradeAction(); // Call to refresh user data (coins) and trade items
            fetchTrade();
        } catch (err) {
            console.error('Failed to fetch trade:', err);
        }
    };

    const handleSellClick = (item: InventoryItemDTO) => {
        setSelectedItem(item);
        setSellPrice(0);
        setIsSellModalOpen(true);
    };

    const handleSellCancel = () => {
        setIsSellModalOpen(false);
        setSelectedItem(null);
    };

    const handleSellSubmit = async () => {
        if (!selectedItem || sellPrice <= 0) return;

        try {
            await api.trade.setTrade(selectedItem.id, sellPrice);
            setIsSellModalOpen(false);
            setSelectedItem(null);
            fetchInventory(); // Refresh inventory after selling
            onTradeAction(); // Call to refresh user data (coins)
        } catch (err) {
            console.error('Failed to set trade:', err);
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
                                    <button className="sell-button" onClick={() => handleSellClick(item)}>Sell</button>
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

            {/* Sell Modal */}
            {isSellModalOpen && selectedItem && (
                <div className="modal-overlay">
                    <div className="modal-content">
                        <h3>Sell {getItemMetadata(selectedItem.id).name}</h3>
                        <p>Enter price (gold):</p>
                        <input 
                            type="number" 
                            value={sellPrice} 
                            onChange={(e) => setSellPrice(parseInt(e.target.value) || 0)}
                            min="1"
                        />
                        <div className="modal-actions">
                            <button className="modal-button cancel" onClick={handleSellCancel}>Cancel</button>
                            <button className="modal-button confirm" onClick={handleSellSubmit}>List</button>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Panel;