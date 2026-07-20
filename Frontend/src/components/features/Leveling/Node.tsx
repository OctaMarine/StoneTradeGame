import type { SkillNode, NodeState } from './SkillTreeTypes';

interface Props {
  node: SkillNode;
  state: NodeState;
  onClick: () => void;
}

export function Node({ node, state, onClick }: Props) {
  const size = 50;
  
  return (
    <div
      className={`skill-node ${state}`}
      onClick={state !== 'locked' ? onClick : undefined}
      title={`${node.name}\n${node.description}\n${node.bonus.stat}: +${node.bonus.value}`}
      style={{
        left: node.x - size / 2,
        top: node.y - size / 2,
        width: size,
        height: size,
        fontSize: 20,
      }}
    >
      {node.icon || node.name[0]}
    </div>
  );
}