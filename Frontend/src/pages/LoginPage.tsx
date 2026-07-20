import { useState} from 'react';
import type { FormEvent } from 'react';
import { api } from '@/lib/api';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';

export default function LoginPage() {
  const [userName, setUserName] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  
  const handleSubmit = async (event: FormEvent) => {
    event.preventDefault();
    setError(null);
    setLoading(true);

    try {
      await api.auth.login(userName, password);
      navigate('/game');
    } catch (error) {
      console.error('An error occurred during login:', error);
      setError(error instanceof Error ? error.message : 'An unknown error occurred.');
    } finally {
      setLoading(false);
    }
  };

  // --- JSX ЗНАЧИТЕЛЬНО УПРОЩЕН ---
  return (
    // Используем flex для центрирования формы на экране
    <main className="flex flex-col items-center justify-center min-h-screen">
      
      {/* Форма с вертикальным расположением элементов и отступами */}
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 w-80">
        
        <h2 className="text-2xl text-center font-bold">Login</h2>
        
        <input
          style={{ padding: '10px', border: '1px solid #555', background: '#333', color: 'white' }}
          type="text"
          placeholder="Username"
          value={userName}
          onChange={(e) => setUserName(e.target.value)}
          disabled={loading}
          required
        />
        
        <input
          style={{ padding: '10px', border: '1px solid #555', background: '#333', color: 'white' }}
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          disabled={loading}
          required
        />
        
        {/* Простой пример кнопки с Tailwind */}
        <button
          className="bg-blue-600 text-white p-2 rounded disabled:bg-gray-500"
          type="submit"
          disabled={loading}
        >
          {loading ? 'Logging in...' : 'Login'}
        </button>
        
        {/* Сообщение об ошибке */}
        {error && <p className="text-red-500 text-center">{error}</p>}
        
        {/* Ссылка на регистрацию */}
        <div className="text-center">
          <Link to="/registration" className="text-blue-400 hover:underline">
            Register
          </Link>
        </div>

      </form>
    </main>
  );
}