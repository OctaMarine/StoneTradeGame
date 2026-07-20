interface Props {
  edges: { from: string; to: string }[];
  nodes: Record<string, { x: number; y: number }>;
  activeNodes: string[];
}

export function Edges({ edges, nodes, activeNodes }: Props) {
  return (
    <svg 
      style={{ 
        position: 'absolute', 
        top: 0, 
        left: 0, 
        width: '100%', 
        height: '100%', 
        pointerEvents: 'none' 
      }}
    >
      {edges.map(edge => {
        const from = nodes[edge.from];
        const to = nodes[edge.to];
        const isActive = activeNodes.includes(edge.from) && activeNodes.includes(edge.to);
        
        return (
          <line
            key={`${edge.from}-${edge.to}`}
            className={`skill-edge ${isActive ? 'active' : ''}`}
            x1={from.x} y1={from.y}
            x2={to.x}   y2={to.y}
          />
        );
      })}
    </svg>
  );
}