const url_host_dev: string = 'http://localhost:5000/api/v1/';
const url_host_prod: string = 'http://192.168.0.142:5000/api/v1/';
const url_host_proxy: string = '/api/v1/';
const url_host: string =url_host_dev;

export const api = {
    
    users: {
        getAll: () => {
            return fetch(url_host+'users')
                .then(response => {
                    if (!response.ok) throw new Error('Network response was not ok for /users');
                    return response.text();
                });
        }
    },
    auth: {
        login: async (userName: string, password: string) => {
    try {
        const url = url_host + 'login';
        console.log('[DEBUG] Запрос на:', url);
        
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ UserName: userName, Password: password }),
            credentials: 'include',
        });

        console.log('[DEBUG] Статус ответа:', response.status);
        const text = await response.text();
        console.log('[DEBUG] Тело ответа:', text);

        if (!response.ok) {
            const err = `❌ ОШИБКА ЛОГИНА: ${response.status}\n${text}`;
            alert(err); // Покажет ошибку прямо на телефоне
            throw new Error(err);
        }


        return true;
    } catch (err: any) {
        // Срабатывает при Network Error, CORS или разрыве соединения
        const netErr = `🚫 СЕТЕВАЯ/КОРС ОШИБКА: ${err.message || err}`;
        alert(netErr);
        console.error('[DEBUG CATCH]', netErr);
        throw err;
    }
},
        register: (userName: string, password: string, email: string) => {
            const formData = new FormData();
            formData.append('userName', userName);
            formData.append('password', password);
            formData.append('email', email);

            return fetch(url_host+'register', {
                method: 'POST',
                body: formData, // Отправляем как FormData
                credentials: 'include', // Важно для отправки и получения cookie
            }).then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error('Registration failed: ' + text);
                    });
                }
                return response.ok;
            });
        }
    },
    inventory : {
        userData: () => {
            return fetch(url_host+'userdata', {
                credentials: 'include', // Автоматически отправляет cookie с токеном
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok for /userdata. Status: ' + response.status);
                }
                return response.json();
            });
        },
        getUserInventoryItems: () => {
            return fetch(url_host+'userinventoryitems', {
                credentials: 'include',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok for /userinventoryitems. Status: ' + response.status);
                    }
                    return response.json();
                });
        },
        getTradeItems: () => {
            return fetch(url_host+'getalltrade', {
                credentials: 'include',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok for /getalltrade. Status: ' + response.status);
                    }
                    return response.json();
                });
        },
        addSupply: () => {
            return fetch(url_host+'addsupply', {
                method: 'POST',
                credentials: 'include',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok for /supply. Status: ' + response.status);
                    }
                    return response.ok;
                });
        }
    },
    trade : {
        buyTrade: (tradeId: number) => {
            return fetch(url_host+'buytrade', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ TradeId: tradeId }),
                credentials: 'include', // Важно для отправки и получения cookie
            }).then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok for /buytrade. Status: ' + response.status);
                }
                return response.ok; // Возвращаем просто статус успешности
            }) 
        },
        setTrade: (itemId: number, price: number) => {
            const formData = new FormData();
            formData.append('itemId', itemId.toString());
            formData.append('price', price.toString());
    
            return fetch(url_host + 'settrade', {
                method: 'POST',
                body: formData,
                credentials: 'include',
            }).then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error('Set trade failed: ' + text);
                    });
                }
                return response.ok;
            });
        }
    },
    craft: {
        getAvailableRecipes: (): Promise<CraftingRecipe[]> => {
            return fetch(url_host + 'recipes', {
                credentials: 'include',
            })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok for /craft/recipes. Status: ' + response.status);
                }
                return response.json();
            });
        },
        craftItem: (craftingRecipeId: number) => {
            return fetch(url_host + 'craft', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ CraftingRecipeId: craftingRecipeId }),
                credentials: 'include',
            })
            .then(async response => {
                const data = await response.json();
                if (!response.ok) {
                    throw new Error('Network response was not ok for /craft. Status: ' + response.status);
                }
                return data.result;
            });
        }
    },
    leveling: {
    getSkillTree: (): Promise<UserSkillNode[]> => {
      return fetch(url_host + 'leveling/skills', {
        credentials: 'include',
      }).then(response => {
        if (!response.ok) throw new Error(`Failed to fetch skills: ${response.status}`);
        return response.json();
      });
    },

    upgradeSkill: (skillId: number): Promise<void> => {
      return fetch(url_host + 'leveling/upgrade', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ skillId }), // Профессиональный JSON-запрос
        credentials: 'include',
      }).then(async response => {
        if (!response.ok) {
          const errorData = await response.json().catch(() => ({}));
          throw new Error(errorData.message || `Failed to upgrade skill: ${response.status}`);
        }
      });
    }
  }
};

export default api;

export interface CraftingIngredient {
  id: number;
  itemId: number;
  quantity: number;
}

export interface CraftingRecipe {
  id: number;
  resultItemId: number;
  resultQuantity: number;
  chanceOfSuccess: number;
  requiredItems: CraftingIngredient[];
  craftingTimeSeconds: number;
  craftingType: number;
}
export interface UserSkillNode {
  skillId: number;
  skillName: string;
  description?: string;
  iconFileName?: string; // Имя файла с бэкенда
  parentSkillId: number | null;
  currentLevel: number;
  maxLevel: number;
  progress: number;
  isOpen: boolean;
  isAvailable: boolean;
  positionX: number;
  positionY: number;
  children: UserSkillNode[];
}
