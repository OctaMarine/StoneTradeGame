export type NodeState = 'locked' | 'available' | 'active';

export interface SkillNode {
  id: string;
  x: number;           
  y: number;
  name: string;
  description: string;
  bonus: { stat: string; value: number };
  requires?: string[];
  icon?: string;
}

export interface SkillEdge {
  from: string;
  to: string;
}

export interface SkillTree {
  id: string;
  nodes: Record<string, SkillNode>;
  edges: SkillEdge[];
  startNodeId: string;
}

export const myTree: SkillTree = {
  id: '1',
  startNodeId: 'core',
  nodes: {
    // Главная нода сверху
    core: { 
      id: 'core', 
      x: 400, 
      y: 80, 
      name: 'Сердце', 
      description: 'Начальная нода', 
      bonus: { stat: 'hp', value: 10 },
      icon: '❤️'
    },
    
    // Левая ветвь - Сила (уходит влево-вниз)
    str1: { 
      id: 'str1', 
      x: 250, 
      y: 200, 
      name: 'Сила I', 
      description: '+5 к силе', 
      bonus: { stat: 'str', value: 5 }, 
      requires: ['core'],
      icon: '💪'
    },
    str2: { 
      id: 'str2', 
      x: 200, 
      y: 320, 
      name: 'Мощь', 
      description: '+10 к силе', 
      bonus: { stat: 'str', value: 10 }, 
      requires: ['str1'],
      icon: '⚔️'
    },
    str3: { 
      id: 'str3', 
      x: 150, 
      y: 440, 
      name: 'Разрушитель', 
      description: '+20 к силе', 
      bonus: { stat: 'str', value: 20 }, 
      requires: ['str2'],
      icon: '🔥'
    },
    
    // Центральная ветвь - Ловкость (идет прямо вниз)
    agi1: { 
      id: 'agi1', 
      x: 400, 
      y: 200, 
      name: 'Ловкость I', 
      description: '+5 к ловкости', 
      bonus: { stat: 'agi', value: 5 }, 
      requires: ['core'],
      icon: '🏃'
    },
    agi2: { 
      id: 'agi2', 
      x: 400, 
      y: 320, 
      name: 'Скорость', 
      description: '+10 к ловкости', 
      bonus: { stat: 'agi', value: 10 }, 
      requires: ['agi1'],
      icon: '⚡'
    },
    agi3: { 
      id: 'agi3', 
      x: 400, 
      y: 440, 
      name: 'Молния', 
      description: '+20 к ловкости', 
      bonus: { stat: 'agi', value: 20 }, 
      requires: ['agi2'],
      icon: '💨'
    },
    
    // Правая ветвь - Разум (уходит вправо-вниз)
    int1: { 
      id: 'int1', 
      x: 550, 
      y: 200, 
      name: 'Разум I', 
      description: '+5 к разуму', 
      bonus: { stat: 'int', value: 5 }, 
      requires: ['core'],
      icon: '🧠'
    },
    int2: { 
      id: 'int2', 
      x: 600, 
      y: 320, 
      name: 'Мудрость', 
      description: '+10 к разуму', 
      bonus: { stat: 'int', value: 10 }, 
      requires: ['int1'],
      icon: '📚'
    },
    int3: { 
      id: 'int3', 
      x: 650, 
      y: 440, 
      name: 'Архимаг', 
      description: '+20 к разуму', 
      bonus: { stat: 'int', value: 20 }, 
      requires: ['int2'],
      icon: '✨'
    },
  },
  edges: [
    // Связи от core к трем ветвям
    { from: 'core', to: 'str1' },
    { from: 'core', to: 'agi1' },
    { from: 'core', to: 'int1' },
    
    // Левая ветвь (Сила)
    { from: 'str1', to: 'str2' },
    { from: 'str2', to: 'str3' },
    
    // Центральная ветвь (Ловкость)
    { from: 'agi1', to: 'agi2' },
    { from: 'agi2', to: 'agi3' },
    
    // Правая ветвь (Разум)
    { from: 'int1', to: 'int2' },
    { from: 'int2', to: 'int3' },
  ],
};