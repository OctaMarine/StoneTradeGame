export interface ItemMetadata {
    id: number;
    name: string;
    icon: string;
}

export const ITEMS_METADATA: Record<number, ItemMetadata> = {
    1: {
        id: 1,
        name: 'Iron',
        icon: '/icons/iron.png'
    },
    2: {
        id: 2,
        name: 'Iron Dust',
        icon: '/icons/iron_dust.png'
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