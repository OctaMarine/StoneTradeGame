import React, { useCallback, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import '@/components/features/Leveling/Leveling.css';
import { SkillTreeView } from './SkillTree';
import { api, type UserSkillNode } from '@/lib/api';

export default function Leveling() {
    const [skillTree, setSkillTree] = useState<UserSkillNode[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);

    const fetchTree = useCallback(async () => {
        try {
            setLoading(true);
            const data = await api.leveling.getSkillTree();
            setSkillTree(data);
        } catch (err) {
            setError(err instanceof Error ? err.message : 'Ошибка загрузки дерева');
        } finally {
            setLoading(false);
        }
    }, []);

    useEffect(() => {
        fetchTree();
    }, [fetchTree]);

    const handleNodeClick = async (node: UserSkillNode) => {
        // Прокачивать можно, если прогресс 100% и навык доступен (IsAvailable = true)
        if (node.progress >= 1.0 && node.isAvailable) {
            try {
                await api.leveling.upgradeSkill(node.skillId);
                await fetchTree(); // Перезагружаем дерево для отображения нового уровня
            } catch (err) {
                alert(err instanceof Error ? err.message : 'Не удалось прокачать навык');
            }
        }
    };

    if (loading) return <div className="loading">Загрузка дерева навыков...</div>;
    if (error) return <div className="error">Ошибка: {error}</div>;

    return (
        <div className="leveling-page">
            <div className="action-buttons-container-right-aligned">
                <Link to="/game">
                    <button className="action-button">Домой</button>
                </Link>
            </div>
            
            <div className="skill-tree-container">
                <SkillTreeView 
                    nodes={skillTree} 
                    onNodeClick={handleNodeClick}
                />
            </div>
        </div>
    );
}