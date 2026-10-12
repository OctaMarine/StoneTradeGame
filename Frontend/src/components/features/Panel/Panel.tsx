import React, {useEffect, useState, useCallback} from 'react';
import { api, type CraftingIngredient,type CraftingRecipe } from '@/lib/api';
import './Panel.css';
import './ItemGrid.css';
import { getItemMetadata } from '@/lib/itemsMetadata';


type PanelType = 'inventory' | 'trade' | 'craft';

interface InventoryItemDTO {
    slotId: number;
    itemId: number;
    quantity: number;
}

interface TradeItemDTO {
    id: number;
    itemId: number;
    price: number;
    seller: number;
}


interface PanelProps {
    onRefreshUserData: () => void;
    onRefreshInventory: (refreshFn: () => void) => void;
}

const Panel: React.FC<PanelProps> = ({ onRefreshUserData ,onRefreshInventory}) => {

    const [activePanel, setActivePanel] = useState<PanelType>('inventory');
    const [inventoryItems, setInventoryItems] = useState<InventoryItemDTO[]>([]);
    const [tradeItems, setTradeItems] = useState<TradeItemDTO[]>([]);
    const [craftableRecipes, setCraftableRecipes] = useState<CraftingRecipe[]>([]);


    const [isSellModalOpen, setIsSellModalOpen] = useState(false);
    const [selectedItem, setSelectedItem] = useState<InventoryItemDTO | null>(null);
    const [sellPrice, setSellPrice] = useState<number>(0);


    const handleMenuClick = (item: PanelType) => {
        setActivePanel(item);
    };

    const fetchInventory = useCallback(async () => {
        try {
            const data: InventoryItemDTO[] = await api.inventory.getUserInventoryItems();
            setInventoryItems(data);
        } catch (err) {
            console.error('Failed to fetch inventory:', err);
        }
    }, []); 

    const fetchTrade = useCallback(async () => {
        try {
            const data: TradeItemDTO[] = await api.trade.getTradeItems();
            setTradeItems(data || []);
        } catch (err) {
            console.error('Failed to fetch trade:', err);
        }
    }, []);

    const fetchCraftableRecipes = useCallback(async () => {
        try {
            const data: CraftingRecipe[] = await api.craft.getAvailableRecipes();
            setCraftableRecipes(data);
        } catch (err) {
            setCraftableRecipes([]);
            console.error('Failed to fetch craftable recipes:', err);
        }
    }, []);

    const handleBuy = async (itemId : number) => {
        console.log('Покупка товара с ID:', itemId);
        try {
            await api.trade.buyTrade(itemId);
            onRefreshUserData();
            fetchTrade();
        } catch (err) {
            console.error('Failed to fetch trade:', err);
        }
    };

    useEffect(() => {
        onRefreshInventory(fetchInventory);
    }, [onRefreshInventory, fetchInventory]); 

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
            await api.trade.setTrade(selectedItem.itemId, sellPrice);
            setIsSellModalOpen(false);
            setSelectedItem(null);
            fetchInventory(); 
            onRefreshUserData(); 
        } catch (err) {
            console.error('Failed to set trade:', err);
        }
    };

    const handleCraft = async (recipeId: number) => {
        console.log('Crafting item with recipe ID:', recipeId);
        try {
            let res = await api.craft.craftItem(recipeId);
            if(res)
            {
                alert('Предмет успешно создан!');
                fetchInventory();
                onRefreshUserData();
            }
            else
            {
                alert('Не удалось создать предмет');
            }
        } catch (err) {
            console.error('Failed to craft item:', err);
        }
    };

    useEffect(() => {
        if (activePanel === 'inventory') {
            fetchInventory();
        } else if (activePanel === 'trade') {
            fetchTrade();
        } else if (activePanel === 'craft') {
            fetchCraftableRecipes();
        }
    }, [activePanel, fetchInventory, fetchTrade, fetchCraftableRecipes]);

    return (
        <div className="panel">
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

            <div className="panel-content">
                {activePanel === 'inventory' && (
                    <div className="inventory-list">
                        {inventoryItems.map((item, index) => {
                            const metadata = getItemMetadata(item.itemId);
                            return (
                                <div key={`${item.itemId}-${index}`} className="inventory-row">
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
                    <div className="inventory-list"> 
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
                    <div className="craft-list">
                        {craftableRecipes.map((recipe) => {
                            const craftedItemMetadata = getItemMetadata(recipe.resultItemId);
                            return (
                                <div key={recipe.id} className="inventory-row craft-row">
                                    <img
                                        src={craftedItemMetadata.icon}
                                        alt={craftedItemMetadata.name}
                                        className="item-icon"
                                    />
                                    <span className="item-name">{craftedItemMetadata.name} x{recipe.resultQuantity}</span>
                                    <span className="craft-chance">Chance: {(recipe.chanceOfSuccess * 100).toFixed(0)}%</span>
                                    <span className="craft-time">Time: {recipe.craftingTimeSeconds}s</span>
                                    
                                    <div className="craft-resources">
                                        {recipe.requiredItems?.map((resource) => {
                                            const resourceMetadata = getItemMetadata(resource.itemId);
                                            return (
                                                <div key={resource.id} className="craft-resource-item">
                                                    <img
                                                        src={resourceMetadata.icon}
                                                        alt={resourceMetadata.name}
                                                        className="resource-icon"
                                                    />
                                                    <span className="resource-quantity">x{resource.quantity}</span>
                                                    <span className="item-name">{ getItemMetadata(resource.itemId).name}</span>

                                                </div>
                                            );
                                        })}
                                    </div>
                                    <button className="craft-button" onClick={() => handleCraft(recipe.id)}>Craft</button>
                                </div>
                            );
                        })}
                    </div>
                )}
            </div>

            {/* Sell Modal */}
            {isSellModalOpen && selectedItem && (
                <div className="modal-overlay">
                    <div className="modal-content">
                        <h3>Sell {getItemMetadata(selectedItem.itemId).name}</h3>
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