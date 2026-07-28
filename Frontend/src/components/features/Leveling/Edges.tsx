import React from 'react';
import type { UserSkillNode } from '@/lib/api';

interface Props {
  edges: { from: number; to: number; isActive: boolean }[];
  nodes: Record<number, UserSkillNode>;
  offsetX: number;
  offsetY: number;
}

export function Edges({ edges, nodes, offsetX, offsetY }: Props) {
  return (
    <svg 
      style={{ 
        position: 'absolute', 
        top: 0, 
        left: 0, 
        width: '100%', 
        height: '100%', 
        pointerEvents: 'none',
        zIndex: 0 // Линии под нодами
      }}
    >
      {edges.map((edge, index) => {
        const fromNode = nodes[edge.from];
        const toNode = nodes[edge.to];
        
        if (!fromNode || !toNode || fromNode.positionX === undefined || toNode.positionX === undefined) {
            return null;
        }

        const x1 = fromNode.positionX + offsetX;
        const y1 = fromNode.positionY + offsetY;
        const x2 = toNode.positionX + offsetX;
        const y2 = toNode.positionY + offsetY;
        
        return (
          <line
            key={`edge-${index}`}
            x1={x1} y1={y1}
            x2={x2} y2={y2}
            stroke={edge.isActive ? '#555' : '#222'}
            strokeWidth={edge.isActive ? 3 : 2}
            strokeDasharray={edge.isActive ? '0' : '5,5'} // Пунктир для закрытых связей
          />
        );
      })}
    </svg>
  );
}