export interface ItemMetadata {
    id: string;
    name: string;
    icon: string;
}

export const ITEMS_METADATA: Record<string, ItemMetadata> = {
    'ca8880a8-f49f-4ebd-acbb-d7fa9c01c617': {
        id: 'ca8880a8-f49f-4ebd-acbb-d7fa9c01c617',
        name: 'Iron',
        icon: '/icons/chaos_stone.png'
    },
    '449c71ac-20d2-4464-ae30-27db63f1a28f': {
        id: '449c71ac-20d2-4464-ae30-27db63f1a28f',
        name: 'Titan',
        icon: '/icons/dark_stone.png'
    }
};

export const getItemMetadata = (itemId: string): ItemMetadata => {
    console.log(`[Debug] getItemMetadata called with ID: "'${itemId}'"`);
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