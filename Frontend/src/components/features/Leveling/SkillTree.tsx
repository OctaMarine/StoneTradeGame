import React, { useMemo } from 'react';
// Убедись, что путь '@/lib/api' правильный. Если api.ts лежит в src/api.ts, то должно быть '@/api'
import { type UserSkillNode } from '@/lib/api'; 
import { Node } from './Node';
import { Edges } from './Edges';
import '@/components/features/Leveling/SkillTree.css';

interface Props {
  nodes: UserSkillNode[];
  onNodeClick: (node: UserSkillNode) => void;
}

type NodeState = 'locked' | 'active' | 'available' | 'maxed';

interface RenderableNode extends UserSkillNode {
  state: NodeState;
}

export function SkillTreeView({ nodes, onNodeClick }: Props) {
  // 1. Рекурсивно сплющиваем дерево в Map для удобного доступа по ID и вычисления состояний
  const flatNodes = useMemo(() => {
    const map = new Map<number, RenderableNode>();
    
    const traverse = (nodeList: UserSkillNode[], parentState: NodeState = 'locked') => {
      nodeList.forEach(node => {
        let state: NodeState = 'locked';
        
        if (!node.isOpen) {
          state = 'locked';
        } else if (node.currentLevel >= (node.maxLevel ?? 10)) { // <-- SAFEGUARD: ?? 1 на случай, если maxLevel не пришел с бэка
          state = 'maxed';
        } else if (node.progress >= 1 && node.isAvailable) {
          state = 'available';
        } else if (parentState !== 'locked' || !node.parentSkillId) {
          // Если родитель открыт (или это корневой навык), мы можем набивать прогресс
          state = 'active';
        }

        map.set(node.skillId, { ...node, state });
        
        if (node.children && node.children.length > 0) {
          traverse(node.children, state);
        }
      });
    };
    
    traverse(nodes);
    return map;
  }, [nodes]);

  // 2. Вычисляем границы холста на основе координат из БД
  const worldBounds = useMemo(() => {
    let minX = Infinity, minY = Infinity, maxX = -Infinity, maxY = -Infinity;
    let hasCoords = false;

    flatNodes.forEach(node => {
      if (node.positionX !== undefined && node.positionY !== undefined) {
        minX = Math.min(minX, node.positionX);
        maxX = Math.max(maxX, node.positionX);
        minY = Math.min(minY, node.positionY);
        maxY = Math.max(maxY, node.positionY);
        hasCoords = true;
      }
    });

    if (!hasCoords) {
      // Fallback, если координаты еще не заполнены в БД
      return { width: 800, height: 600, offsetX: 0, offsetY: 0 };
    }

    return {
      width: Math.max(800, (maxX - minX) + 200), // +200 дает запас по краям
      height: Math.max(600, (maxY - minY) + 200),
      offsetX: Math.abs(Math.min(0, minX)) + 100, // +100 сдвигает всё вправо, чтобы отрицательные координаты не обрезались
      offsetY: Math.abs(Math.min(0, minY)) + 100  // +100 сдвигает всё вниз
    };
  }, [flatNodes]);

  // 3. Собираем ребра для отрисовки линий
  const edges = useMemo(() => {
    const result: { from: number; to: number; isActive: boolean }[] = [];
    flatNodes.forEach(node => {
      if (node.parentSkillId !== null && node.parentSkillId !== undefined) {
        result.push({
          from: node.parentSkillId,
          to: node.skillId,
          isActive: node.state !== 'locked'
        });
      }
    });
    return result;
  }, [flatNodes]);

  return (
    <div className="skill-tree-viewport">
      <div 
        className="skill-tree-world" 
        style={{ 
          position: 'relative', 
          width: worldBounds.width, 
          height: worldBounds.height 
        }}
      >
        <Edges 
          edges={edges} 
          nodes={Object.fromEntries(flatNodes.entries())} 
          offsetX={worldBounds.offsetX}
          offsetY={worldBounds.offsetY}
        />
        
        {Array.from(flatNodes.values()).map(node => (
          <Node
            key={node.skillId}
            node={node}
            offsetX={worldBounds.offsetX}
            offsetY={worldBounds.offsetY}
            onClick={() => onNodeClick(node)}
          />
        ))}
      </div>
    </div>
  );
}