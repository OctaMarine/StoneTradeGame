export interface ItemMetadata {
    id: number;
    name: string;
    icon: string;
}

export const ITEMS_METADATA: Record<number, ItemMetadata> = {
    1: {
        id: 1,
        name: 'Stone',
        icon: '/icons/stone.png'
    },
    2: {
        id: 2,
        name: 'Mana Dust',
        icon: '/icons/mana_dust.png'
    },
    3: {
        id: 3,
        name: 'Enchanted Stone',
        icon: '/icons/enchanted_stone.png'
    }
};

export const getItemMetadata = (itemId: number): ItemMetadata => {
    const metadata = ITEMS_METADATA[itemId];
    if (!metadata) {
        console.warn(`[Debug] Metadata not found for ID: "'${itemId}'"`);
    }
    return metadata || {
        id: itemId,
        name: `Item ${itemId}`,
        icon: '/icons/unknown.png'
    };
};