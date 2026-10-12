import React from 'react';
import type { UserSkillNode } from '@/lib/api'; // Проверь путь к твоему api.ts

interface Props {
  node: UserSkillNode & { state: 'locked' | 'active' | 'available' | 'maxed' };
  offsetX: number;
  offsetY: number;
  onClick: () => void;
}

export function Node({ node, offsetX, offsetY, onClick }: Props) {
  const isClickable = node.state === 'available';
  
  const iconPath = node.iconFileName 
    ? `/icons/skills/${node.iconFileName + '.png'}` 
    : '/icons/unknown.png';

  const tooltip = `${node.skillName} (Ур. ${node.currentLevel}/${node.maxLevel})\n${node.description || 'Нет описания'}\nПрогресс: ${Math.floor(node.progress)}%`;

  return (
    <div
      className={`skill-node ${node.state}`}
      onClick={isClickable ? onClick : undefined}
      title={tooltip}
      style={{
        left: (node.positionX || 0) + offsetX,
        top: (node.positionY || 0) + offsetY,

        transform: 'translate(-50%, -50%)', 
      }}
    >

      <img 
        src={iconPath} 
        alt={node.skillName}
        onError={(e) => {
            (e.target as HTMLImageElement).src = '/icons/unknown.png';
        }}
      />
      

      <span style={{ marginTop: '2px' }}>
        {node.currentLevel}
      </span>


      {node.state === 'active' && (
        <>

          {node.progress > 0.0 && (
            <div className="progress-text">
              {node.progress*100}%
            </div>
          )}
          

          <div className="progress-container">
            <div 
              className="progress-fill" 
              style={{ width: `${node.progress*100}%` }} 
            />
          </div>
        </>
      )}
    </div>
  );
}