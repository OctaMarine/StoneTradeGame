'use client';

import { useEffect, useState } from 'react';
import { api } from '@/lib/api';
import Link from 'next/link';

export default function Home() {
  const [numberValue, setNumberValue] = useState<number>(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchData = async () => {
    try {
      setLoading(true);
      console.log('Начинаем запрос к API...');

      const response = await api.users.getAll();
      console.log('Ответ получен:', response);

      // Предполагаем, что сервер возвращает число напрямую или в поле data
      const receivedNumber = parseInt(response )

      setNumberValue(receivedNumber);
      setError(null);
    } catch (err: any) {
      console.error('Полная ошибка:', err);
      console.error('URL запроса:', err.config?.url);
      console.error('Метод запроса:', err.config?.method);
      console.error('Статус ошибки:', err.response?.status);
      console.error('Данные ошибки:', err.response?.data);

      setError(`Ошибка: ${err.message || 'Неизвестная ошибка'}`);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const incrementNumber = () => {
    setNumberValue(prev => prev + 1);
  };

  const decrementNumber = () => {
    setNumberValue(prev => prev - 1);
  };

  const resetNumber = () => {
    setNumberValue(0);
  };

  if (loading) {
    return (
        <div className="min-h-screen flex items-center justify-center">
          <div className="text-lg">Загрузка числа с сервера...</div>
        </div>
    );
  }

  return (
      <main>
        <div className="max-w-4xl mx-auto">
          <Link href="/login">
            <button className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors mr-4">
              Play
            </button>
          </Link>
          <h1 className="text-3xl font-bold mb-8 text-center">Число с сервера</h1>

          {error && (
              <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-6">
                <strong>Ошибка:</strong> {error}
                <div className="mt-2 text-sm">Проверьте соединение с сервером</div>
                <button
                    onClick={fetchData}
                    className="mt-2 bg-red-600 text-white px-3 py-1 rounded"
                >
                  Повторить запрос
                </button>
              </div>
          )}

          {!error && (
              <>
                {/* Основное число */}
                <div className="bg-white rounded-lg shadow-lg p-8 mb-6 text-center">
                  <h2 className="text-2xl font-semibold mb-4 text-gray-700">Текущее значение:</h2>
                  <div className="text-6xl font-bold text-blue-600 mb-6">
                    {numberValue}
                  </div>

                  {/* Кнопки управления */}
                  <div className="flex justify-center space-x-4 mb-6">
                    <button
                        onClick={decrementNumber}
                        className="bg-red-500 hover:bg-red-600 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors"
                    >
                      -1
                    </button>

                    <button
                        onClick={resetNumber}
                        className="bg-gray-500 hover:bg-gray-600 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors"
                    >
                      Сброс
                    </button>

                    <button
                        onClick={incrementNumber}
                        className="bg-green-500 hover:bg-green-600 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors"
                    >
                      +1
                    </button>
                  </div>
                </div>

                {/* Информация о числе */}
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-6">
                  <div className="bg-blue-100 p-4 rounded-lg text-center">
                    <h3 className="font-semibold mb-2">Квадрат</h3>
                    <p className="text-2xl">{numberValue * numberValue}</p>
                  </div>

                  <div className="bg-green-100 p-4 rounded-lg text-center">
                    <h3 className="font-semibold mb-2">Четное?</h3>
                    <p className="text-2xl">{numberValue % 2 === 0 ? 'Да' : 'Нет'}</p>
                  </div>

                  <div className="bg-purple-100 p-4 rounded-lg text-center">
                    <h3 className="font-semibold mb-2">Абсолютное</h3>
                    <p className="text-2xl">{Math.abs(numberValue)}</p>
                  </div>
                </div>

                {/* Кнопки обновления */}
                <div className="text-center">
                  <button
                      onClick={fetchData}
                      className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors mr-4"
                  >
                    Обновить с сервера
                  </button>

                  <button
                      onClick={() => setNumberValue(0)}
                      className="bg-gray-600 hover:bg-gray-700 text-white px-6 py-3 rounded-lg text-lg font-semibold transition-colors"
                  >
                    Сброс к 0
                  </button>
                </div>
              </>
          )}
        </div>
      </main>
  );
}