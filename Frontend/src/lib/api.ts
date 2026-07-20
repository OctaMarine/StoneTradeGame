const url_host_dev: string = 'http://localhost:5000/api/v1/';
//const url_host_prod: string = 'http://192.168.0.142:5000/api/v1/';
const url_host: string = url_host_dev;

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
        login: (userName: string, password: string) => {
            return fetch(url_host+'login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ UserName: userName, Password: password }),
                credentials: 'include', // Важно для отправки и получения cookie
            }).then(response => {
                if (!response.ok) {
                    return response.text().then(text => {
                        throw new Error('Login failed: ' + text);
                    });
                }
                return response.ok; // Ожидаем успешный статус, cookie установится автоматически
            });
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
        getUserLevelData: () => {
            return fetch(url_host+'getuserleveldata', {
                credentials: 'include',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok for /getuserleveldata. Status: ' + response.status);
                    }
                    return response.json();
                });
        },
        addLevelUp: () => {
            return fetch(url_host+'addlevelup', {
                method: 'POST',
                credentials: 'include',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok for /addlevelup. Status: ' + response.status);
                    }
                    return response.json();
                });
        },
        addSkillUp: (skillId: number) => {
            return fetch(url_host+'addskillup', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ SkillId: skillId }),
                credentials: 'include',
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok for /addskillup. Status: ' + response.status);
                    }
                    return response.json();
                });
        }
    }
};

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

export interface UserLevelData {
    level: number;
    xp: number;
    xpToNextLevel: number;
    skills: Array<{
        id: number;
        name: string;
        description: string;
        cost: number;
        learned: boolean;
    }>;
}
