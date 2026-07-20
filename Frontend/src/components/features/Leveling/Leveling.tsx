import React from 'react';
import { Link } from 'react-router-dom';
import '@/components/features/Leveling/Leveling.css';
import { SkillTreeView } from './SkillTree';

export default function Leveling() {
    return (
        <div className="leveling-page">
            <div className="action-buttons-container-right-aligned">
                <Link to="/game">
                    <button className="action-button">
                        Домой
                    </button>
                </Link>
            </div>
            
            <div className="skill-tree-container">
                <SkillTreeView  
                    activeNodes={[]} 
                    points={0} 
                    onNodeClick={(nodeId: string) => {
                        console.log('Clicked node:', nodeId);
                    }}
                />
            </div>
        </div>
    );
}