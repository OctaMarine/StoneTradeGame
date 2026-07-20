import { useMemo } from "react";
import type { NodeState, SkillTree} from "./SkillTreeTypes";
import { Edges } from "./Edges";
import { Node } from "./Node";
import '@/components/features/Leveling/SkillTree.css';
import { myTree } from './SkillTreeTypes';



interface Props {
  activeNodes: string[]; // какие ноды уже взяты игроком
  points: number;        // свободные очки
  onNodeClick: (nodeId: string) => void;
}

export function SkillTreeView({ activeNodes, points, onNodeClick }: Props) {
    const tree = myTree 
  // Состояние каждой ноды вычисляем на лету
  const nodeStates = useMemo<Record<string, NodeState>>(() => {
    const states: Record<string, NodeState> = {};
    for (const id in tree.nodes) {
      if (activeNodes.includes(id)) {
        states[id] = 'active';
      } else {
        const node = tree.nodes[id];
        const reqsMet = !node.requires || node.requires.every(r => activeNodes.includes(r));
        states[id] = reqsMet && points > 0 ? 'available' : 'locked';
      }
    }
    return states;
  }, [tree, activeNodes, points]);

  return (
    <div className="skill-tree-viewport">
      <div className="skill-tree-world" style={{ position: 'relative', width: 800, height: 600 }}>
        {/* Линии рисуем ПЕРВЫМИ, чтобы они были под нодами */}
        <Edges edges={tree.edges} nodes={tree.nodes} activeNodes={activeNodes} />
        
        {/* Ноды поверх */}
        {Object.values(tree.nodes).map(node => (
          <Node
            key={node.id}
            node={node}
            state={nodeStates[node.id]}
            onClick={() => onNodeClick(node.id)}
          />
        ))}
      </div>
    </div>
  );
}